using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.model.interfaces;
using OptionsTradeWell.Properties;

namespace OptionsTradeWell.model
{
    public class OptionsQuikDdeDataCollector : ITerminalOptionDataCollector
    {
        private static Logger LOGGER = LogManager.GetCurrentClassLogger();
        private static string SERVER_NAME = Settings.Default.ServerName;
        private static string OPTIONS_DESK = Settings.Default.OptionsTableName;
        private static string FUTURES_DESK = Settings.Default.FuturesTableName;
        private static double MINIMUM_UPDATE_TIME_SEC = Settings.Default.MinActualStrikeUpdateTimeSec;

        private static Dictionary<string, int> TOPICS_AND_ROWS_LENGTH_MAP = CreateCustomDdeTableMap();

        public event EventHandler<OptionEventArgs> OnOptionsDeskChanged;
        public event EventHandler<OptionEventArgs> OnSpotPriceChanged;

        private QuikServerDde server;
        private SortedDictionary<double, Option> callMap;
        private SortedDictionary<double, Option> putMap;

        private Futures basicFutures;
        private Option infoOption;
        private double lastTrackingStrike;
        private DateTime lastTrackingUpdate;

        private bool futRecievedDataFlag;
        private bool optRecievedDataFlag;

        public OptionsQuikDdeDataCollector()
        {
            LOGGER.Info("OptionsQuikDdeDataCollector creation...");
            this.server = new QuikServerDde(SERVER_NAME, TOPICS_AND_ROWS_LENGTH_MAP);
            server.OnDataUpdate += CollectAndSortServerDataByMaps;

            if (Settings.Default.NumberOfTrackingOptions % 2 == 0)
            {
                this.NumberOfTrackingOptions = Settings.Default.NumberOfTrackingOptions;
            }
            else
            {
                this.NumberOfTrackingOptions = Settings.Default.NumberOfTrackingOptions - 1;
            }

            this.callMap = new SortedDictionary<double, Option>();
            this.putMap = new SortedDictionary<double, Option>();
            this.lastTrackingStrike = 0.0;
            this.lastTrackingUpdate = DateTime.Now;

            this.futRecievedDataFlag = false;
            this.optRecievedDataFlag = false;
            LOGGER.Info("OptionsQuikDdeDataCollector created");
        }

        private static Dictionary<string, int> CreateCustomDdeTableMap()
        {
            Dictionary<string, int> resultMap = new Dictionary<string, int>();
            resultMap.Add(FUTURES_DESK, 10);
            resultMap.Add(OPTIONS_DESK, 14);

            return resultMap;
        }

        public int NumberOfTrackingOptions { get; set; }

        public bool IsConnected()
        {
            LOGGER.Debug("checking IsConnected flags: {0}, {1}, {2}", server.IsRegistered, futRecievedDataFlag, optRecievedDataFlag);
            return server.IsRegistered && futRecievedDataFlag && optRecievedDataFlag;
        }

        public void EstablishConnection()
        {
            server.Register();
        }

        public void BreakConnection()
        {
            server.Unregister();
        }

        public double GetBid(double strike, OptionType type)
        {
            return GetOption(strike, type).GetTradeBlotter().BidPrice;
        }

        public double GetAsk(double strike, OptionType type)
        {
            return GetOption(strike, type).GetTradeBlotter().AskPrice;
        }

        public double GetCoverMarginRequirement(double strike, OptionType type)
        {
            return GetOption(strike, type).MarginRequirementCover;
        }

        public double GetNotCoverMarginRequirement(double strike, OptionType type)
        {
            return GetOption(strike, type).MarginRequirementNotCover;
        }

        public double GetBuyerMarginRequirement(double strike, OptionType type)
        {
            return GetOption(strike, type).MarginRequirementBuyer;
        }

        public DateTime GetExpirationDate(double strike, OptionType type)
        {
            return GetOption(strike, type).ExpirationDate;
        }

        public double GetPriceStep(double strike, OptionType type)
        {
            return GetOption(strike, type).PriceStep;
        }

        public double GetPriceStepValue(double strike, OptionType type)
        {
            return GetOption(strike, type).PriceStepValue;
        }

        public double CalculateMinImportantStrike()
        {
            if (GetBasicFutures() == null)
            {
                throw new QuikDdeException("Basic futures still null : " + basicFutures);
            }

            return CalculateActualStrike() - Settings.Default.StrikeStep * NumberOfTrackingOptions / 2;
        }

        public double CalculateMaxImportantStrike()
        {
            if (GetBasicFutures() == null)
            {
                throw new QuikDdeException("Basic futures is still null : " + basicFutures);
            }

            return CalculateActualStrike() + Settings.Default.StrikeStep * NumberOfTrackingOptions / 2;
        }

        public double CalculateActualStrike()
        {
            return Math.Round(basicFutures.GetTradeBlotter().AskPrice / Settings.Default.StrikeStep, 0) * Settings.Default.StrikeStep;
        }

        public Option GetOption(double strike, OptionType type)
        {
            Option tempOption;
            if (!GetSuitableOptionsMap(type).TryGetValue(strike, out tempOption))
            {
                throw new QuikDdeException("Such option does not represent in map yet. Check DDE-data export. " + type + " " + strike);
            }

            return tempOption;
        }

        public Futures GetBasicFutures()
        {
            return basicFutures;
        }

        public bool IsOptionExist(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type).ContainsKey(strike);
        }

        private void CollectAndSortServerDataByMaps(string topic, string[] data)
        {
            LOGGER.Debug("Starting process of colleting and sorting data. Topic: {0}, data array: {1} ", topic, String.Join(" ", data));
            if (futRecievedDataFlag == false || optRecievedDataFlag == false)
            {
                CheckConnectionFlags();
            }

            //DDE ORDER IS: futures -> options
            if (topic.Equals(FUTURES_DESK))
            {
                if (GetBasicFutures() == null)
                {
                    LOGGER.Debug("Initializing basic futures.");
                    string ticker = data[0];
                    DateTime maturity = DateTime.Parse(data[1]);
                    double commission = Convert.ToDouble(data[2]);
                    double marginRequirement = Convert.ToDouble(data[3]);
                    double priceStep = Convert.ToDouble(data[4]);
                    double priceStepValue = Convert.ToDouble(data[5]);


                    TradeBlotter futuresBlotter = new TradeBlotter();
                    futuresBlotter.BidPrice = Convert.ToDouble(data[6]);
                    futuresBlotter.BidSize = Convert.ToDouble(data[7]);
                    futuresBlotter.AskPrice = Convert.ToDouble(data[8]);
                    futuresBlotter.AskSize = Convert.ToDouble(data[9]);

                    basicFutures = new Futures(ticker, maturity, commission, marginRequirement, priceStep,
                        priceStepValue);
                    basicFutures.AssignTradeBlotter(futuresBlotter);

                    lastTrackingStrike = CalculateActualStrike();

                    LOGGER.Debug("Initializing completed. Futures instance: {0}", basicFutures);
                }
                else
                {
                    LOGGER.Debug("Updating basic futures.");

                    TradeBlotter futuresBlotter = basicFutures.GetTradeBlotter();
                    futuresBlotter.BidPrice = Convert.ToDouble(data[6]);
                    futuresBlotter.BidSize = Convert.ToDouble(data[7]);
                    futuresBlotter.AskPrice = Convert.ToDouble(data[8]);
                    futuresBlotter.AskSize = Convert.ToDouble(data[9]);

                    LOGGER.Debug("Updating completed.");
                }

                if (OnSpotPriceChanged != null && infoOption != null && futRecievedDataFlag == true)
                {
                    OnSpotPriceChanged(this, new OptionEventArgs(infoOption));
                }

            }
            else if (topic.Equals(OPTIONS_DESK))
            {
                OptionType optionType = (OptionType)Enum.Parse(typeof(OptionType), data[0]);
                double strike = Convert.ToDouble(data[1]);
                SortedDictionary<double, Option> suitOptionsMap = GetSuitableOptionsMap(optionType);
                Option tempOption;

                if (suitOptionsMap.ContainsKey(strike))
                {
                    LOGGER.Debug("Initializing option: {0}, {1}", optionType, strike);

                    tempOption = suitOptionsMap[strike];
                    tempOption.MarginRequirementNotCover = Convert.ToDouble(data[2]);
                    tempOption.MarginRequirementCover = Convert.ToDouble(data[3]);
                    tempOption.MarginRequirementBuyer = Convert.ToDouble(data[4]);

                    TradeBlotter optionsBlotter = tempOption.GetTradeBlotter();
                    optionsBlotter.BidPrice = Convert.ToDouble(data[10]);
                    optionsBlotter.BidSize = Convert.ToDouble(data[11]);
                    optionsBlotter.AskPrice = Convert.ToDouble(data[12]);
                    optionsBlotter.AskSize = Convert.ToDouble(data[13]);

                    LOGGER.Debug("Initializing completed. Option instance: {0}", tempOption);
                }
                else
                {
                    LOGGER.Debug("Updating option: {0}, {1}", optionType, strike);

                    double marginRequirementCover = Convert.ToDouble(data[2]); ;
                    double marginRequirementNotCover = Convert.ToDouble(data[3]); ;
                    double marginRequirementBuyer = Convert.ToDouble(data[4]); ;
                    string ticker = data[5];
                    double priceStep = Convert.ToDouble(data[6]);
                    double priceStepValue = Convert.ToDouble(data[7]);
                    DateTime expirationDate = DateTime.Parse(data[8]);
                    int remainingDays = Convert.ToInt32(data[9]);


                    tempOption = new Option(basicFutures, optionType, strike, marginRequirementCover, marginRequirementNotCover, marginRequirementBuyer,
                        ticker, priceStep, priceStepValue, expirationDate, remainingDays);

                    TradeBlotter optionsBlotter = new TradeBlotter();
                    optionsBlotter.BidPrice = Convert.ToDouble(data[10]);
                    optionsBlotter.BidSize = Convert.ToDouble(data[11]);
                    optionsBlotter.AskPrice = Convert.ToDouble(data[12]);
                    optionsBlotter.AskSize = Convert.ToDouble(data[13]);

                    tempOption.AssignTradeBlotter(optionsBlotter);

                    suitOptionsMap.Add(strike, tempOption);

                    LOGGER.Debug("Updating completed.");
                }


                if (infoOption == null)
                {
                    //just for access to general options field
                    infoOption = suitOptionsMap[strike];
                    LOGGER.Debug("info option created: {0}", infoOption);
                }

                if (OnOptionsDeskChanged != null
                    && optRecievedDataFlag == true
                    && tempOption.Strike <= CalculateMaxImportantStrike()
                    && tempOption.Strike >= CalculateMinImportantStrike())
                {
                    OnOptionsDeskChanged(this, new OptionEventArgs(tempOption));
                }
            }
            else
            {
                throw new QuikDdeException("table with a such name wasn't mapped: " + String.Join(" ", TOPICS_AND_ROWS_LENGTH_MAP.Keys));
            }

        }

        private void CheckConnectionFlags()
        {
            if (GetBasicFutures() != null)
            {
                futRecievedDataFlag = true;
                LOGGER.Debug("Futures flag changed on {0}", futRecievedDataFlag);
            }

            if (futRecievedDataFlag == true
                && callMap.Count > 0
                && putMap.Count > 0
                && callMap.Keys.Max() >= Settings.Default.MaxOptionStrikeInQuikDesk
                && putMap.Keys.Max() >= Settings.Default.MaxOptionStrikeInQuikDesk)
            {
                optRecievedDataFlag = true;
                LOGGER.Debug("Options flag changed on {0}", optRecievedDataFlag);
            }
        }

        private SortedDictionary<double, Option> GetSuitableOptionsMap(OptionType type)
        {
            return type == OptionType.Call ? callMap : putMap;
        }
    }
}
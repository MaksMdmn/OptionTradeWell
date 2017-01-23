using System;
using System.Collections.Generic;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.model.interfaces;
using OptionsTradeWell.Properties;

namespace OptionsTradeWell.model
{
    public class OptionsQuikDdeDataCollector : ITerminalOptionDataCollector
    {
        private static string SERVER_NAME = Settings.Default.ServerName;
        private static string OPTIONS_DESK = Settings.Default.OptionsTableName;
        private static string FUTURES_DESK = Settings.Default.FuturesTableName;
        private static double MINIMUM_UPDATE_TIME_SEC = Settings.Default.MinActualStrikeUpdateTimeSec;

        private static Dictionary<string, int> TOPICS_AND_ROWS_LENGTH_MAP = CreateCustomDdeTableMap();

        public event EventHandler<OptionEventArgs> OnOptionsDeskChanged;
        public event EventHandler<OptionEventArgs> OnSpotPriceChanged;
        public event EventHandler OnBasedParametersChanged;

        private QuikServerDde server;
        private Dictionary<double, Option> callMap;
        private Dictionary<double, Option> putMap;

        private Futures basicFutures;
        private Option infoOption;
        private int numberOfTrackingOptions;
        private double lastTrackingStrike;
        private DateTime lastTrackingUpdate;

        private bool futRecievedDataFlag;
        private bool optRecievedDataFlag;

        public OptionsQuikDdeDataCollector(int numberOfTrackingOptions)
        {
            this.server = new QuikServerDde(SERVER_NAME, TOPICS_AND_ROWS_LENGTH_MAP);
            server.OnDataUpdate += CollectAndSortServerDataByMaps;

            if (numberOfTrackingOptions % 2 == 0)
            {
                this.NumberOfTrackingOptions = numberOfTrackingOptions;
            }
            else
            {
                this.NumberOfTrackingOptions = numberOfTrackingOptions - 1;
            }

            this.callMap = new Dictionary<double, Option>();
            this.putMap = new Dictionary<double, Option>();
            this.lastTrackingStrike = 0.0;
            this.lastTrackingUpdate = DateTime.Now;

            this.futRecievedDataFlag = false;
            this.optRecievedDataFlag = false;
        }

        private static Dictionary<string, int> CreateCustomDdeTableMap()
        {
            Dictionary<string, int> resultMap = new Dictionary<string, int>();
            resultMap.Add(FUTURES_DESK, 10);
            resultMap.Add(OPTIONS_DESK, 14);

            return resultMap;
        }

        public int NumberOfTrackingOptions
        {
            get
            {
                return numberOfTrackingOptions;
            }
            set
            {
                numberOfTrackingOptions = value;
                if (OnBasedParametersChanged != null)
                {
                    OnBasedParametersChanged(this, EventArgs.Empty);
                }
            }
        }

        public bool IsConnected()
        {
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
            return GetSuitableOptionsMap(type)[strike].GetTradeBlotter().BidPrice;
        }

        public double GetAsk(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type)[strike].GetTradeBlotter().AskPrice;
        }

        public double GetCoverMarginRequirement(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type)[strike].MarginRequirementCover;
        }

        public double GetNotCoverMarginRequirement(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type)[strike].MarginRequirementNotCover;
        }

        public double GetBuyerMarginRequirement(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type)[strike].MarginRequirementBuyer;
        }

        public DateTime GetExpirationDate(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type)[strike].ExpirationDate;
        }

        public double GetPriceStep(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type)[strike].PriceStep;
        }

        public double GetPriceStepValue(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type)[strike].PriceStepValue;
        }

        public double CalculateMinImportantStrike()
        {
            if (basicFutures == null)
            {
                throw new QuikDdeException("Basic futures still null : " + basicFutures);
            }

            return CalculateActualStrike() - NumberOfTrackingOptions / 2;
        }

        public double CalculateMaxImportantStrike()
        {
            if (basicFutures == null)
            {
                throw new QuikDdeException("Basic futures still null : " + basicFutures);
            }

            return CalculateActualStrike() + NumberOfTrackingOptions / 2;
        }

        public double CalculateActualStrike()
        {
            return Math.Round(basicFutures.GetTradeBlotter().AskPrice, 0);
        }

        public Option GetOption(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type)[strike];
        }

        private void CollectAndSortServerDataByMaps(string topic, string[] data)
        {
            if (futRecievedDataFlag == false || optRecievedDataFlag == false)
            {
                CheckConnectionFlags();
            }

            //DDE ORDER IS: futures -> options
            if (topic.Equals(FUTURES_DESK))
            {
                if (basicFutures == null)
                {
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
                }
                else
                {
                    TradeBlotter futuresBlotter = basicFutures.GetTradeBlotter();
                    futuresBlotter.BidPrice = Convert.ToDouble(data[6]);
                    futuresBlotter.BidSize = Convert.ToDouble(data[7]);
                    futuresBlotter.AskPrice = Convert.ToDouble(data[8]);
                    futuresBlotter.AskSize = Convert.ToDouble(data[9]);
                }

                if (OnSpotPriceChanged != null && infoOption != null)
                {
                    OnSpotPriceChanged(this, new OptionEventArgs(infoOption));
                }


                if (OnBasedParametersChanged != null 
                    && Math.Abs(lastTrackingStrike - CalculateActualStrike()) > 0.01
                    && DateTime.Now > lastTrackingUpdate)
                {
                    lastTrackingUpdate = DateTime.Now.AddSeconds(MINIMUM_UPDATE_TIME_SEC);
                    OnBasedParametersChanged(this, EventArgs.Empty);
                }
            }
            else if (topic.Equals(OPTIONS_DESK))
            {
                OptionType optionType = (OptionType)Enum.Parse(typeof(OptionType), data[0]);
                double strike = Convert.ToDouble(data[1]);
                Dictionary<double, Option> suitOptionsMap = GetSuitableOptionsMap(optionType);
                Option tempOption;

                if (strike < CalculateMinImportantStrike() || strike > CalculateMaxImportantStrike())
                {
                    return;
                }

                if (suitOptionsMap.ContainsKey(strike))
                {
                    tempOption = suitOptionsMap[strike];
                    tempOption.MarginRequirementNotCover = Convert.ToDouble(data[2]);
                    tempOption.MarginRequirementCover = Convert.ToDouble(data[3]);
                    tempOption.MarginRequirementBuyer = Convert.ToDouble(data[4]);

                    TradeBlotter optionsBlotter = tempOption.GetTradeBlotter();
                    optionsBlotter.BidPrice = Convert.ToDouble(data[10]);
                    optionsBlotter.BidSize = Convert.ToDouble(data[11]);
                    optionsBlotter.AskPrice = Convert.ToDouble(data[12]);
                    optionsBlotter.AskSize = Convert.ToDouble(data[13]);
                }
                else
                {
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

                }


                if (infoOption == null)
                {
                    //just for access to general options field
                    infoOption = suitOptionsMap[strike];
                }

                if (OnOptionsDeskChanged != null)
                {
                    OnOptionsDeskChanged(this, new OptionEventArgs(tempOption));
                }
            }
            else
            {
                throw new QuikDdeException("table with a such name wasn't mapped: " + TOPICS_AND_ROWS_LENGTH_MAP.Keys);
            }

        }

        private void CheckConnectionFlags()
        {
            if (basicFutures != null)
            {
                futRecievedDataFlag = true;
            }

            if (futRecievedDataFlag == true
                && callMap.ContainsKey(CalculateMinImportantStrike())
                && callMap.ContainsKey(CalculateMaxImportantStrike())
                && putMap.ContainsKey(CalculateMinImportantStrike())
                && putMap.ContainsKey(CalculateMaxImportantStrike()))
            {
                optRecievedDataFlag = true;
            }
        }

        private Dictionary<double, Option> GetSuitableOptionsMap(OptionType type)
        {
            return type == OptionType.Call ? callMap : putMap;
        }

        private void InitDefaultDerivativesValues()
        {
            basicFutures = new Futures("", DateTime.Now, 0.0, 0.0, 0.0, 0.0); //HERE'S A LOT OF SHIT CAN BE
            DateTime expirationDate = DateTime.Now;
        }
    }
}
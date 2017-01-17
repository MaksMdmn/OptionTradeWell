using System;
using System.Collections.Generic;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model
{
    public class OptionsQuikDdeDataCollector : ITerminalOptionDataCollector
    {
        private static string SERVER_NAME = "OTWserver";
        private static string OPTIONS_DESK = "OPTIONS_DESK";
        private static string FUTURES_DESK = "FUTURES_DESK";

        private static Dictionary<string, int> TOPICS_AND_ROWS_LENGTH_MAP = CreateCustomDdeTableMap();

        public event EventHandler<OptionEventArgs> OnOptionsDeskChanged;
        public event EventHandler<FuturesEventArgs> OnSpotPriceChanged;
        public event EventHandler OnBasedParametersChanged;

        private QuikServerDde server;
        private Dictionary<double, Option> callMap;
        private Dictionary<double, Option> putMap;
        //default values
        private Futures basicFutures;
        private int numberOfTrackingOptions;
        private double lastTrackingStrike;

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
            return server.IsRegistered;
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

            return CalculateTrackingStrike() - NumberOfTrackingOptions / 2;
        }

        public double CalculateMaxImportantStrike()
        {
            if (basicFutures == null)
            {
                throw new QuikDdeException("Basic futures still null : " + basicFutures);
            }

            return CalculateTrackingStrike() + NumberOfTrackingOptions / 2;
        }

        public Option GetOption(double strike, OptionType type)
        {
            return GetSuitableOptionsMap(type)[strike];
        }

        private void CollectAndSortServerDataByMaps(string topic, string[] data)
        {
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

                    lastTrackingStrike = CalculateTrackingStrike();
                }
                else
                {
                    TradeBlotter futuresBlotter = basicFutures.GetTradeBlotter();
                    futuresBlotter.BidPrice = Convert.ToDouble(data[6]);
                    futuresBlotter.BidSize = Convert.ToDouble(data[7]);
                    futuresBlotter.AskPrice = Convert.ToDouble(data[8]);
                    futuresBlotter.AskSize = Convert.ToDouble(data[9]);
                }

                if (OnSpotPriceChanged != null)
                {
                    int tempOptExpDays = 0;
                    if (callMap.Count != 0)
                    {
                        tempOptExpDays = Convert.ToInt32(callMap[CalculateTrackingStrike()].RemainingDays);
                    }

                    OnSpotPriceChanged(this, new FuturesEventArgs(basicFutures, tempOptExpDays));
                }


                if (OnBasedParametersChanged != null && Math.Abs(lastTrackingStrike - CalculateTrackingStrike()) > 0.01)
                {
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

        private Dictionary<double, Option> GetSuitableOptionsMap(OptionType type)
        {
            return type == OptionType.Call ? callMap : putMap;
        }

        private void InitDefaultDerivativesValues()
        {
            basicFutures = new Futures("", DateTime.Now, 0.0, 0.0, 0.0, 0.0); //HERE'S A LOT OF SHIT CAN BE
            DateTime expirationDate = DateTime.Now;
        }

        private double CalculateTrackingStrike()
        {
            return Math.Round(basicFutures.GetTradeBlotter().AskPrice, 0);
        }
    }
}
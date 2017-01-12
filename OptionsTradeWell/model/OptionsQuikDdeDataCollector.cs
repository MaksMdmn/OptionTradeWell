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

        private QuikServerDde server;
        private Dictionary<double, Option> callMap;
        private Dictionary<double, Option> putMap;
        private int numberOfImportantOptions;
        //default values
        private Futures basicFutures;

        public OptionsQuikDdeDataCollector(int numberOfImportantOptions)
        {
            this.server = new QuikServerDde(SERVER_NAME, TOPICS_AND_ROWS_LENGTH_MAP);
            server.OnDataUpdate += CollectAndSortServerDataByMaps;

            this.numberOfImportantOptions = numberOfImportantOptions;

            this.callMap = new Dictionary<double, Option>();
            this.putMap = new Dictionary<double, Option>();
        }

        private void CollectAndSortServerDataByMaps(string topic, string[] data)
        {
            //DDE ORDER IS: futures -> margins -> options
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
                }
                else
                {
                    TradeBlotter futuresBlotter = basicFutures.GetTradeBlotter();
                    futuresBlotter.BidPrice = Convert.ToDouble(data[6]);
                    futuresBlotter.BidSize = Convert.ToDouble(data[7]);
                    futuresBlotter.AskPrice = Convert.ToDouble(data[8]);
                    futuresBlotter.AskSize = Convert.ToDouble(data[9]);
                }
            }

            if (topic.Equals(OPTIONS_DESK))
            {
                OptionType optionType = (OptionType)Enum.Parse(typeof(OptionType), data[0]);
                double strike = Convert.ToDouble(data[1]);
                Dictionary<double, Option> suitOptionsMap = GetSuitableOptionsMap(optionType);
                Option tempOption;

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
            }


            if (topic.Equals(OPTIONS_DESK))
            {
                double strike = Convert.ToDouble(data[2]);
                Option tempCall = callMap[strike];
                Option tempPut = putMap[strike];

                tempCall.GetTradeBlotter().BidPrice = Convert.ToDouble(data[0]);
                tempCall.GetTradeBlotter().AskPrice = Convert.ToDouble(data[1]);
                tempPut.GetTradeBlotter().BidPrice = Convert.ToDouble(data[3]);
                tempPut.GetTradeBlotter().AskPrice = Convert.ToDouble(data[4]);


            }

            throw new QuikDdeException("table with a such name wasn't mapped: " + TOPICS_AND_ROWS_LENGTH_MAP.Keys);
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

        private Dictionary<double, Option> GetSuitableOptionsMap(OptionType type)
        {
            return type == OptionType.Call ? callMap : putMap;
        }

        private void InitDefaultDerivativesValues()
        {
            basicFutures = new Futures("", DateTime.Now, 0.0, 0.0, 0.0, 0.0); //HERE'S A LOT OF SHIT CAN BE
            DateTime expirationDate = DateTime.Now;
        }

        private static Dictionary<string, int> CreateCustomDdeTableMap()
        {
            Dictionary<string, int> resultMap = new Dictionary<string, int>();
            resultMap.Add(OPTIONS_DESK, 14);
            resultMap.Add(FUTURES_DESK, 10);

            return resultMap;
        }

    }
}
using System;
using System.Collections.Generic;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model
{
    public class OptionsQuikDdeDataCollector : ITerminalOptionDataCollector
    {
        private static string SERVER_NAME = "OTWserver";
        private static string OPTIONS_DESK = "OPTION_DESK";
        private static string MARGIN_TABLE = "MARGIN_TABLE";
        private static string FUTURES_DESK = "FUTURES_DESK";

        private static Dictionary<string, int> TOPICS_AND_ROWS_LENGTH_MAP = CreateCustomDdeTableMap();

        private QuikServerDde server;
        private Dictionary<double, Option> callMap;
        private Dictionary<double, Option> putMap;
        private int numberOfImportantOptions;

        //default values
        private Futures basicFutures;
        private double priceStep;
        private double priceStepValue;
        private DateTime expirationDate;

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
            if (topic.Equals(OPTIONS_DESK))
            {
                Option tempOption = new Option();
            }

            if (topic.Equals(MARGIN_TABLE))
            {

            }

            if (topic.Equals(FUTURES_DESK))
            {

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
            priceStep = 0.0d;
            priceStepValue = 0.0d;
            DateTime expirationDate = DateTime.Now;
        }

        private static Dictionary<string, int> CreateCustomDdeTableMap()
        {
            Dictionary<string, int> resultMap = new Dictionary<string, int>();
            resultMap.Add(OPTIONS_DESK, 5);
            resultMap.Add(MARGIN_TABLE, 10);
            resultMap.Add(FUTURES_DESK, 6);

            return resultMap;
        }

    }
}
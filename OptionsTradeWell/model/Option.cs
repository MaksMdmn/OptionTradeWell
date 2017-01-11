using System;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model
{
    public class Option : ITradable
    {
        private TradeBlotter blotter = null;
        private static double DAYS_IN_YEAR = 365;

        public Option(Futures futures, OptionType optionType, int remainingDays, double strike, double marginRequirementCover, double marginRequirementNoCover, double marginRequirementBuyer)
        {
            this.Futures = futures;
            this.MarginRequirementCover = marginRequirementCover;
            this.MarginRequirementNoCover = marginRequirementNoCover;
            this.MarginRequirementBuyer = marginRequirementBuyer;
            this.OptionType = optionType;
            this.RemainingDays = remainingDays;
            this.Strike = strike;
        }

        public Futures Futures { get; }

        public double RemainingDays { get; }

        public double Strike { get; }
        public double MarginRequirementCover { get; }

        public double MarginRequirementNoCover { get; }

        public double MarginRequirementBuyer { get; }

        public OptionType OptionType { get; }

        public void AssignTradeBlotter(TradeBlotter blotter)
        {
            if (blotter != null)
            {
                this.blotter = blotter;
            }
            else
            {
                Console.WriteLine("argument is null, bud.");
            }
        }

        public bool IsTradeBlotterAssigned()
        {
            return blotter != null && blotter.IsTradeBlotterActive();
        }

        public TradeBlotter GetTradeBlotter()
        {
            return this.blotter;
        }

        public double Delta { get; private set; }

        public double Gamma { get; private set; }

        public double Vega { get; private set; }

        public double Theta { get; private set; }

        public double ImplVol { get; private set; }

        public double BuyVol { get; private set; }

        public double SellVol { get; private set; }
        public void UpdateAllGreeksTogether()
        {
            double optionTime = this.RemainingDays / DAYS_IN_YEAR;
            double spotPrice = this.Futures.GetTradeBlotter().AskPrice;

            double vola = GreeksCalculator.CalculateImpliedVolatility(this.OptionType, spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR,
                this.GetTradeBlotter().AskPrice, 0.5);

            double distr_d1 = GreeksCalculator.GreeksDistribution(GreeksCalculator.Calculate_d1(spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, vola));

            Delta = GreeksCalculator.CalculateDelta(this.OptionType, distr_d1);
            Gamma = GreeksCalculator.CalculateGamma(spotPrice, distr_d1, optionTime, vola);
            Vega = GreeksCalculator.CalculateVega(spotPrice, distr_d1, optionTime);
            Theta = GreeksCalculator.CalculateTheta(spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, vola, true);
            ImplVol = vola;
            BuyVol = GreeksCalculator.CalculateImpliedVolatility(this.OptionType, GetTradeBlotter().AskPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR,
                this.GetTradeBlotter().AskPrice, 0.5);
            SellVol = GreeksCalculator.CalculateImpliedVolatility(this.OptionType, GetTradeBlotter().BidPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR,
                this.GetTradeBlotter().AskPrice, 0.5);

        }

        //public double GetUpdatedDelta()
        //{
        //    double optionPrice = this.GetTradeBlotter().AskPrice;
        //    double spotPrice = this.Futures.GetTradeBlotter().AskPrice;

        //    double vola = GreeksCalculator.CalculateImpliedVolatility(this.OptionType, spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, optionPrice, 0.5);

        //    return GreeksCalculator.CalculateDelta(this.OptionType, spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, vola);
        //}

        //public double GetUpdatedGamma()
        //{
        //    double optionPrice = this.GetTradeBlotter().AskPrice;
        //    double spotPrice = this.Futures.GetTradeBlotter().AskPrice;

        //    double vola = GreeksCalculator.CalculateImpliedVolatility(this.OptionType, spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, optionPrice, 0.5);

        //    return GreeksCalculator.CalculateGamma(spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, vola);
        //}

        //public double GetUpdatedVega()
        //{
        //    double optionPrice = this.GetTradeBlotter().AskPrice;
        //    double spotPrice = this.Futures.GetTradeBlotter().AskPrice;

        //    double vola = GreeksCalculator.CalculateImpliedVolatility(this.OptionType, spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, optionPrice, 0.5);

        //    return GreeksCalculator.CalculateVega(spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, vola);
        //}

        //public double GetUpdatedTheta()
        //{
        //    double optionPrice = this.GetTradeBlotter().AskPrice;
        //    double spotPrice = this.Futures.GetTradeBlotter().AskPrice;

        //    double vola = GreeksCalculator.CalculateImpliedVolatility(this.OptionType, spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, optionPrice, 0.5);

        //    return GreeksCalculator.CalculateTheta(spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, vola);
        //}

        //public double GetUpdatedImplVol()
        //{
        //    double optionPrice = this.GetTradeBlotter().AskPrice;
        //    double spotPrice = this.Futures.GetTradeBlotter().AskPrice;

        //    return GreeksCalculator.CalculateImpliedVolatility(this.OptionType, spotPrice, this.Strike, this.RemainingDays, DAYS_IN_YEAR, optionPrice, 0.5);
        //}


    }
}

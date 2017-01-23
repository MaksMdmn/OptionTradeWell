using System;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model
{
    public class Option : ITradable
    {
        private TradeBlotter blotter = null;

        public Option(Futures futures, OptionType optionType, double strike, double marginRequirementCover, double marginRequirementNotCover, double marginRequirementBuyer, string ticker, double priceStep, double priceStepValue, DateTime expirationDate, int remainingDays)
        {
            this.Futures = futures;
            this.MarginRequirementCover = marginRequirementCover;
            this.MarginRequirementNotCover = marginRequirementNotCover;
            this.MarginRequirementBuyer = marginRequirementBuyer;
            this.PriceStep = priceStep;
            this.PriceStepValue = priceStepValue;
            this.ExpirationDate = expirationDate;
            this.Ticker = ticker;
            this.OptionType = optionType;
            this.RemainingDays = remainingDays;
            this.Strike = strike;
        }

        public Futures Futures { get; }

        public double RemainingDays { get; }

        public DateTime ExpirationDate { get; }
        public double Strike { get; }

        public string Ticker { get; }
        public double PriceStep { get; }
        public double PriceStepValue { get; }

        public double MarginRequirementCover { get; set; }

        public double MarginRequirementNotCover { get; set; }

        public double MarginRequirementBuyer { get; set; }

        public OptionType OptionType { get; }

        public double Delta { get; private set; }

        public double Gamma { get; private set; }

        public double Vega { get; private set; }

        public double Theta { get; private set; }

        public double ImplVol { get; private set; }

        public double BuyVol { get; private set; }

        public double SellVol { get; private set; }

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

        public void UpdateAllGreeksTogether()
        {
            double optionTime = this.RemainingDays / GreeksCalculator.DAYS_IN_YEAR;
            double spotPrice = this.Futures.GetTradeBlotter().AskPrice;

            double vola = GreeksCalculator.GetFilteredVolatilityValue(GreeksCalculator.CalculateImpliedVolatility(this.OptionType, spotPrice, this.Strike, this.RemainingDays, GreeksCalculator.DAYS_IN_YEAR,
                this.GetTradeBlotter().AskPrice, 0.5));

            if (Math.Abs(vola) < 0.0001) // vola==0
            {
                Delta = 0;
                Gamma = 0;
                Vega = 0;
                Theta = 0;
            }
            else
            {
                double deltaDistr_d1 = GreeksCalculator.CalculateDistributionOfStNrmDstr(GreeksCalculator.Calculate_d1(spotPrice, this.Strike, this.RemainingDays, GreeksCalculator.DAYS_IN_YEAR, vola));
                double distr_d1 = GreeksCalculator.GreeksDistribution(GreeksCalculator.Calculate_d1(spotPrice, this.Strike, this.RemainingDays, GreeksCalculator.DAYS_IN_YEAR, vola));

                Delta = GreeksCalculator.CalculateDelta(this.OptionType, deltaDistr_d1);
                Gamma = GreeksCalculator.CalculateGamma(spotPrice, distr_d1, optionTime, vola);
                Vega = GreeksCalculator.CalculateVega(spotPrice, distr_d1, optionTime);
                Theta = GreeksCalculator.CalculateTheta(spotPrice, distr_d1, GreeksCalculator.DAYS_IN_YEAR, optionTime, vola, true);
            }


            ImplVol = vola;
            BuyVol = GreeksCalculator.GetFilteredVolatilityValue(
                GreeksCalculator.CalculateImpliedVolatility(
                    this.OptionType,
                    spotPrice,
                    this.Strike,
                    this.RemainingDays,
                    GreeksCalculator.DAYS_IN_YEAR,
                    this.GetTradeBlotter().AskPrice,
                    0.5));
            SellVol = GreeksCalculator.GetFilteredVolatilityValue(
                GreeksCalculator.CalculateImpliedVolatility(
                    this.OptionType,
                    spotPrice,
                    this.Strike,
                    this.RemainingDays,
                    GreeksCalculator.DAYS_IN_YEAR,
                    this.GetTradeBlotter().BidPrice,
                    0.5));

        }

        public override string ToString()
        {
            return $"{nameof(RemainingDays)}: {RemainingDays}, {nameof(ExpirationDate)}: {ExpirationDate}, {nameof(Strike)}: {Strike}, {nameof(Ticker)}: {Ticker}, {nameof(PriceStep)}: {PriceStep}, {nameof(PriceStepValue)}: {PriceStepValue}, {nameof(MarginRequirementCover)}: {MarginRequirementCover}, {nameof(MarginRequirementNotCover)}: {MarginRequirementNotCover}, {nameof(MarginRequirementBuyer)}: {MarginRequirementBuyer}, {nameof(OptionType)}: {OptionType}, {nameof(Delta)}: {Delta}, {nameof(Gamma)}: {Gamma}, {nameof(Vega)}: {Vega}, {nameof(Theta)}: {Theta}, {nameof(ImplVol)}: {ImplVol}, {nameof(BuyVol)}: {BuyVol}, {nameof(SellVol)}: {SellVol}";
        }

        public string ShowGreeks()
        {
            return "Type: " + OptionType + "\n"
                   + "Delta: " + Delta + "\n"
                   + "Gamma: " + Gamma + "\n"
                   + "Vega: " + Vega + "\n"
                   + "Theta: " + Theta + "\n"
                   + "ImplVol, BuyVol, SellVol:  " + ImplVol + " | " + BuyVol + " | " + SellVol;
        }

    }
}

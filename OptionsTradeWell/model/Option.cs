using System;
using NLog;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model
{
    public class Option : ITradable
    {
        private TradeBlotter blotter = null;
        private static Logger LOGGER = LogManager.GetCurrentClassLogger();
        public Option(Futures futures, OptionType optionType, double strike, double marginRequirementCover, double marginRequirementNotCover, double marginRequirementBuyer, string ticker, double priceStep, double priceStepValue, DateTime expirationDate, double remainingDays)
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
            this.Position = new Position();
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

        public Position Position { get; }

        public static Option GetFakeOption(OptionType type, double strike, double enterOptPrice,
              double remainingDays, int optPosition, TradeBlotter futBlotter, TradeBlotter optBlotter, double priceStep, double priceVal)
        {
            Futures fut = Futures.GetFakeFutures(0.0, 0, futBlotter, priceStep, priceVal);

            Option option = new Option(fut, type, strike, 0.0, 0.0, 0.0, "", priceStep, priceVal, DateTime.Now, remainingDays);
            option.AssignTradeBlotter(optBlotter);
            option.Position.EnterPrice = enterOptPrice;
            option.Position.Quantity = optPosition;

            option.UpdateAllGreeksTogether();

            return option;

        }

        public void AssignTradeBlotter(TradeBlotter blotter)
        {
            if (blotter != null)
            {
                this.blotter = blotter;
            }
            else
            {
                throw new BasicModelException("Passed blotter to option is null: " + blotter);
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
            LOGGER.Debug("All greeks update requested.");
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

            LOGGER.Debug("Greeks update complete, result: Delta: {0}, Gamma: {1}, Vega: {2}, Theta: {3}, ImplVol: {4}, BuyVol: {5}, SellVol: {6}",
                Delta, Gamma, Vega, Theta, ImplVol, BuyVol, SellVol)
            ;
        }

        public double DependOnPosDelta()
        {
            if (this.Position.Quantity == 0)
            {
                return this.Delta;
            }
            else
            {
                return this.Position.Quantity * Delta;
            }
        }

        public double DependOnPosGamma()
        {
            if (this.Position.Quantity == 0)
            {
                return this.Gamma;
            }
            else
            {
                return this.Position.Quantity * Gamma;
            }
        }

        public double DependOnPosVega()
        {
            if (this.Position.Quantity == 0)
            {
                return this.Vega;
            }
            else
            {
                return this.Position.Quantity * Vega;
            }
        }

        public double DependOnPosTheta()
        {
            if (this.Position.Quantity == 0)
            {
                return this.Theta;
            }
            else
            {
                if (this.Position.Quantity == 0)
                {
                    return this.Theta;
                }
                else
                {
                    return this.Position.Quantity * Theta;
                }
            }
        }

        public override string ToString()
        {
            return $"{nameof(blotter)}: {blotter}, {nameof(Futures)}: {Futures}, {nameof(RemainingDays)}: {RemainingDays}, {nameof(ExpirationDate)}: {ExpirationDate}, {nameof(Strike)}: {Strike}, {nameof(Ticker)}: {Ticker}, {nameof(PriceStep)}: {PriceStep}, {nameof(PriceStepValue)}: {PriceStepValue}, {nameof(MarginRequirementCover)}: {MarginRequirementCover}, {nameof(MarginRequirementNotCover)}: {MarginRequirementNotCover}, {nameof(MarginRequirementBuyer)}: {MarginRequirementBuyer}, {nameof(OptionType)}: {OptionType}, {nameof(Delta)}: {Delta}, {nameof(Gamma)}: {Gamma}, {nameof(Vega)}: {Vega}, {nameof(Theta)}: {Theta}, {nameof(ImplVol)}: {ImplVol}, {nameof(BuyVol)}: {BuyVol}, {nameof(SellVol)}: {SellVol}, {nameof(Position)}: {Position}";
        }
    }
}

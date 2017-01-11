using System;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model
{
    public class Futures : ITradable
    {
        private TradeBlotter blotter = null;

        public Futures(string fullName, string shortName, DateTime maturity, double commission, double marginRequirement, double priceStep, double priceStepValue)
        {
            this.FullName = fullName;
            this.ShortName = shortName;
            this.Maturity = maturity;
            this.Commission = commission;
            this.MarginRequirement = marginRequirement;
            this.PriceStep = priceStep;
            this.PriceStepValue = priceStepValue;
        }

        public string FullName { get; }

        public string ShortName { get; }

        public DateTime Maturity { get; }

        public double Commission { get; }

        public double MarginRequirement { get; }

        public double PriceStep { get; }

        public double PriceStepValue { get; }

        public override string ToString()
        {
            return $"{nameof(FullName)}: {FullName}, {nameof(ShortName)}: {ShortName}, {nameof(Maturity)}: {Maturity}, {nameof(Commission)}: {Commission}, {nameof(MarginRequirement)}: {MarginRequirement}, {nameof(PriceStep)}: {PriceStep}, {nameof(PriceStepValue)}: {PriceStepValue}";
        }

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
    }
}

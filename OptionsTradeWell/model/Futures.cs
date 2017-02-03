using System;
using OptionsTradeWell.model.interfaces;

namespace OptionsTradeWell.model
{
    public class Futures : ITradable
    {
        private TradeBlotter blotter = null;

        public Futures(string ticker, DateTime maturity, double commission, double marginRequirement, double priceStep, double priceStepValue)
        {
            this.Ticker = ticker;
            this.Maturity = maturity;
            this.Commission = commission;
            this.MarginRequirement = marginRequirement;
            this.PriceStep = priceStep;
            this.PriceStepValue = priceStepValue;
            this.Position = new Position();
        }

        public string Ticker { get; }

        public DateTime Maturity { get; }

        public double Commission { get; }

        public double MarginRequirement { get; }

        public double PriceStep { get; }

        public double PriceStepValue { get; }

        public Position Position { get; }

        public static Futures GetFakeFutures(double enterFutPrice, int futPosition, TradeBlotter blotter, double priceStep, double priceVal)
        {
            Futures fut = new Futures("", DateTime.Now, 0.0, 0.0, priceStep, priceVal);
            fut.AssignTradeBlotter(blotter);
            fut.Position.Quantity = futPosition;
            fut.Position.EnterPrice = enterFutPrice;
            return fut;
        }

        public override string ToString()
        {
            return $"{nameof(Ticker)}: {Ticker}, {nameof(Maturity)}: {Maturity}, {nameof(Commission)}: {Commission}, {nameof(MarginRequirement)}: {MarginRequirement}, {nameof(PriceStep)}: {PriceStep}, {nameof(PriceStepValue)}: {PriceStepValue}";
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

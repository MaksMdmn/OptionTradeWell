namespace OptionsTradeWell.model
{
    public class TradeBlotter
    {
        public TradeBlotter()
        {
        }

        public double BidPrice { get; set; }

        public double BidSize { get; set; }

        public double AskPrice { get; set; }

        public double AskSize { get; set; }

        public bool IsTradeBlotterActive()
        {
            return BidPrice != null && AskPrice != null && BidSize != null && AskSize != null;
        }

        public override string ToString()
        {
            return $"{nameof(BidPrice)}: {BidPrice}, {nameof(BidSize)}: {BidSize}, {nameof(AskPrice)}: {AskPrice}, {nameof(AskSize)}: {AskSize}";
        }
    }
}

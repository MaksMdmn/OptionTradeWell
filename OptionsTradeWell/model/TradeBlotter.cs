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
    }
}

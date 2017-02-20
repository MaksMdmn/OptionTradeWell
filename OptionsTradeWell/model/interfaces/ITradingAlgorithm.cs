namespace OptionsTradeWell.model.interfaces
{
    public interface ITradingAlgorithm
    {
        TradingSignal GetSignal(out string ticker, out int size, out double price);
    }
}
namespace OptionsTradeWell.model.interfaces
{
    public interface ITerminalDataCollector
    {
        bool IsConnected();
        void EstablishConnection();
        void BreakConnection();

    }
}
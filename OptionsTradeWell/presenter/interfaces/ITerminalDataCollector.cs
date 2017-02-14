namespace OptionsTradeWell.presenter.interfaces
{
    public interface ITerminalDataCollector
    {
        bool IsConnected();
        void EstablishConnection();
        void BreakConnection();

    }
}
using OptionsTradeWell.model;

namespace OptionsTradeWell.presenter.interfaces
{
    public interface ITerminalTransactionsImporter
    {
        bool ConnectToTerminal();
        bool DisconnectFromTerminal();
        bool IsConnected();
        double SendLimitBuyOrder(string ticker, double price, int size);
        double SendLimitSellOrder(string ticker, double price, int size);
        double RollLimitOrder(double id, double price, int size);
        double SendMarketBuyOrder(string ticker, int size);
        double SendMarketSellOrder(string ticker, int size);
        bool CancelOrder(double id);
        bool CancelAllOrders(DerivativesClasses cls);
    }
}
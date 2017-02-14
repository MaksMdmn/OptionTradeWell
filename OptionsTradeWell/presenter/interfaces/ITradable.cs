using OptionsTradeWell.model;

namespace OptionsTradeWell.presenter.interfaces
{
    public interface ITradable
    {
        void AssignTradeBlotter(TradeBlotter blotter);

        TradeBlotter GetTradeBlotter();

        bool IsTradeBlotterAssigned();

    }
}

using OptionsTradeWell.model.entities;

namespace OptionsTradeWell.model.interfaces
{
    public interface ITradable
    {
        void AssignTradeBlotter(TradeBlotter blotter);

        TradeBlotter GetTradeBlotter();

        bool IsTradeBlotterAssigned();
    }
}

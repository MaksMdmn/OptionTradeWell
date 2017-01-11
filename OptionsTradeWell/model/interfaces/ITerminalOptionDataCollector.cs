using System;

namespace OptionsTradeWell.model.interfaces
{
    public interface ITerminalOptionDataCollector : ITerminalDataCollector
    {
        double GetBid(double strike, OptionType type);
        double GetAsk(double strike, OptionType type);
        double GetCoverMarginRequirement(double strike, OptionType type);
        double GetNotCoverMarginRequirement(double strike, OptionType type);
        double GetBuyerMarginRequirement(double strike, OptionType type);
        DateTime GetExpirationDate(double strike, OptionType type);
        double GetPriceStep(double strike, OptionType type);
        double GetPriceStepValue(double strike, OptionType type);
    }
}
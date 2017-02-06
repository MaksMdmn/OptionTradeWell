using System;

namespace OptionsTradeWell.model.interfaces
{
    public interface ITerminalOptionDataCollector : ITerminalDataCollector
    {
        event EventHandler<OptionEventArgs> OnOptionsDeskChanged;
        event EventHandler<OptionEventArgs> OnSpotPriceChanged;
        int NumberOfTrackingOptions { get; set; }
        double GetBid(double strike, OptionType type);
        double GetAsk(double strike, OptionType type);
        double GetCoverMarginRequirement(double strike, OptionType type);
        double GetNotCoverMarginRequirement(double strike, OptionType type);
        double GetBuyerMarginRequirement(double strike, OptionType type);
        DateTime GetExpirationDate(double strike, OptionType type);
        double GetPriceStep(double strike, OptionType type);
        double GetPriceStepValue(double strike, OptionType type);
        Option GetOption(double strike, OptionType type);
        Futures GetBasicFutures();
        bool IsOptionExist(double strike, OptionType type);
        double CalculateMinImportantStrike();
        double CalculateMaxImportantStrike();
        double CalculateActualStrike();
    }

    public class OptionEventArgs : EventArgs
    {
        public Option opt;

        public OptionEventArgs(Option opt)
        {
            this.opt = opt;
        }
    }
}
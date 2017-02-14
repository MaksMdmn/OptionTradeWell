using System;
using OptionsTradeWell.model;

namespace OptionsTradeWell.presenter.interfaces
{
    public interface ITerminalOptionDataCollector : ITerminalDataCollector
    {
        event EventHandler<OptionEventArgs> OnOptionsDeskChanged;
        event EventHandler<OptionEventArgs> OnSpotPriceChanged;
        event EventHandler<TerminalPosEventArgs> OnActualPosChanged;

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
        Option GetOption(string ticker);
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

    public class TerminalPosEventArgs : EventArgs
    {
        public DerivativesClasses cls;
        public string ticker;
        public int actualPos;

        public TerminalPosEventArgs(DerivativesClasses cls, string ticker, int actualPos)
        {
            this.cls = cls;
            this.ticker = ticker;
            this.actualPos = actualPos;
        }
    }
}
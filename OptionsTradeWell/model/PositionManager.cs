using System;
using System.Collections.Generic;
using System.Linq;
using OptionsTradeWell.Properties;

namespace OptionsTradeWell.model
{
    public class PositionManager
    {
        public PositionManager()
        {
            Options = new List<Option>();
            Futures = null;
        }

        public List<Option> Options { get; set; }

        public Futures Futures { get; set; }

        public double FixedPnL { get; set; }

        public double TotalDelta { get; private set; }
        public double TotalGamma { get; private set; }
        public double TotalVega { get; private set; }
        public double TotalTheta { get; private set; }

        public void AddOption(Option option)
        {
            Option tempOption = GetIfSuchOptionHasAlreadyExist(option);
            if (tempOption != null)
            {
                if (tempOption.Position.Quantity == option.Position.Quantity * -1)
                {
                    FixedPnL += tempOption.Position.CloseExistingPosAndGetFixedPnL(tempOption.GetTradeBlotter());
                    Options.Remove(tempOption);
                }
                else
                {
                    tempOption.Position.AddToExistingPos(option.Position.EnterPrice, option.Position.Quantity);
                }
            }
            else
            {
                Options.Add(option);
            }
        }

        public void AddFutures(Futures futures)
        {
            if (Futures == null)
            {
                Futures = futures;
            }
            else
            {
                if (Futures.Position.Quantity == futures.Position.Quantity * -1)
                {
                    FixedPnL += Futures.Position.CloseExistingPosAndGetFixedPnL(futures.GetTradeBlotter());
                    Futures = null;
                }
                else
                {
                    Futures.Position.AddToExistingPos(futures.Position.EnterPrice, futures.Position.Quantity);
                }
            }
        }

        public void AddOrChangeExistingOptionsPosition(Option opt)
        {
            Option tempOption = GetIfSuchOptionHasAlreadyExist(opt);

            if (tempOption == null)
            {
                Options.Add(opt);
            }
            else
            {
                tempOption.Position.Quantity = opt.Position.Quantity;
                tempOption.Position.EnterPrice = opt.Position.EnterPrice;
            }
        }

        public void UpdateGeneralParametres()
        {
            foreach (Option option in Options)
            {
                TotalDelta += option.DependOnPosDelta();
                TotalGamma += option.DependOnPosGamma();
                TotalVega += option.DependOnPosVega();
                TotalTheta += option.DependOnPosTheta();
            }

            if (Futures != null)
            {
                TotalDelta += Convert.ToDouble(Futures.Position.Quantity);
            }
        }

        public double CalculatePositionPnL()
        {
            double tempPnL = 0.0;

            tempPnL += Futures == null ? 0.0 : Futures.Position.CalcCurrentPnL(Futures.GetTradeBlotter());
            tempPnL += FixedPnL;

            foreach (Option opt in Options)
            {
                tempPnL += opt.Position.CalcCurrentPnL(opt.GetTradeBlotter());
            }

            return tempPnL;
        }

        public double CalculatePositionCurPnL()
        {
            double tempPnL = 0.0;

            if (Futures == null && Options.Count == 0)
            {
                return tempPnL;
            }

            if (Futures == null)
            {
                tempPnL += (FixedPnL / Options[0].PriceStep * Options[0].PriceStepValue);
            }
            else
            {
                tempPnL += Futures.Position.CalcCurrentPnLInCurrency(Futures.GetTradeBlotter(), Futures.PriceStep, Futures.PriceStepValue);
                tempPnL += (FixedPnL / Futures.PriceStep * Futures.PriceStepValue);
            }

            foreach (Option opt in Options)
            {
                tempPnL += opt.Position.CalcCurrentPnLInCurrency(opt.GetTradeBlotter(), opt.PriceStep, opt.PriceStepValue);
            }

            return tempPnL;
        }


        public double CalculateCurApproxPnL(double futPrice)
        {
            double result = 0.0;
            double exitPrice;
            foreach (Option opt in Options)
            {
                exitPrice = GreeksCalculator.CalculateOptionPrice_BS(opt.OptionType, futPrice, opt.Strike,
                    opt.RemainingDays, GreeksCalculator.DAYS_IN_YEAR, opt.ImplVol);
                result += opt.Position.CalcCurrentPnL(exitPrice);
            }

            result += Futures == null ? 0.0 : Futures.Position.CalcCurrentPnL(futPrice);
            result += FixedPnL;

            return result;

        }

        public double CalculateExpirationPnL(double futPrice)
        {
            double tempPnL = 0.0;

            TradeBlotter tempBlotter = new TradeBlotter();
            tempBlotter.AskPrice = futPrice;
            tempBlotter.BidPrice = futPrice;

            tempPnL += Futures == null ? 0.0 : Futures.Position.CalcCurrentPnL(tempBlotter);
            tempPnL += FixedPnL;

            foreach (Option opt in Options)
            {
                tempPnL += GreeksCalculator.CalculateOptionPnLOnExpiration(opt, futPrice);
            }

            return tempPnL;
        }

        public void CleanAllPositions()
        {
            Options.Clear();
            Futures = null;
            TotalDelta = 0.0;
            TotalGamma = 0.0;
            TotalVega = 0.0;
            TotalTheta = 0.0;
        }

        public void ResetFixedPnLValue()
        {
            FixedPnL = 0.0;
        }

        public bool IsOptionsPositionExists()
        {
            return Options.Count > 0;
        }

        public bool IsAnyPositionsExists()
        {
            return IsOptionsPositionExists() || Futures != null;
        }

        public bool GetCurrentPriceIfPossible(out double price)
        {
            price = 0.0;
            bool result = false;

            if (Options.Count > 0)
            {
                price = Options[0].Futures.GetTradeBlotter().AskPrice;
                result = true;
            }
            else if (Futures != null)
            {
                price = Futures.GetTradeBlotter().AskPrice;
                result = true;
            }

            return result;
        }

        public double GetCurrentPrice()
        {
            double price = 0.0;

            if (Options.Count > 0)
            {
                price = Options[0].Futures.GetTradeBlotter().AskPrice;
            }
            else if (Futures != null)
            {
                price = Futures.GetTradeBlotter().AskPrice;
            }

            return price;
        }

        private Option GetIfSuchOptionHasAlreadyExist(Option option)
        {
            Option answer = null;

            double searchStrike = option.Strike;
            OptionType searchType = option.OptionType;
            foreach (Option opt in Options)
            {
                if (Math.Abs(opt.Strike - searchStrike) < 0.0001)
                {
                    if (opt.OptionType == searchType)
                    {
                        answer = opt;
                        break;
                    }
                }
            }

            return answer;
        }
    }
}
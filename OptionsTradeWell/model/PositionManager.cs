using System;
using System.Collections.Generic;
using OptionsTradeWell.model.interfaces;
using OptionsTradeWell.Properties;

namespace OptionsTradeWell.model
{
    public class PositionManager
    {
        private double[] guidelineArrForApproxPos;

        public PositionManager()
        {
            Options = new List<Option>();
            Futures = null;
            guidelineArrForApproxPos = GetGuidelineArr();
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

            tempPnL += Futures == null ? 0.0 : Futures.Position.CalcCurrentPnLInCurrency(Futures.GetTradeBlotter(), Futures.PriceStep, Futures.PriceStepValue);
            tempPnL += Futures == null ? 0.0 : (FixedPnL / Futures.PriceStep * Futures.PriceStepValue);

            foreach (Option opt in Options)
            {
                tempPnL += opt.Position.CalcCurrentPnLInCurrency(opt.GetTradeBlotter(), opt.PriceStep, opt.PriceStepValue);
            }

            return tempPnL;
        }


        public double CalculateCurApproxPnL(double futPrice, double minStr, double maxStr)
        {
            double result = 0.0;
            for (double i = minStr; i <= maxStr; i++)
            {
                if (i >= futPrice)
                {
                    result = guidelineArrForApproxPos[(int)(i - minStr)] * CalculateExpirationPnL(futPrice);
                    break;
                }
            }

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

        private double[] GetGuidelineArr()
        {
            int n = Convert.ToInt32(Settings.Default.OptDeskStrikesNumber);
            double tempI = 0.001;
            double koefI = 1.5;
            double[] answer = new double[n];

            if (n % 2 == 0)
            {
                for (int i = 0; i < n / 2; i++)
                {
                    tempI = tempI * koefI;
                    answer[n / 2 - 1 - i] = 1 + tempI;
                    answer[n / 2 + i] = 1 + tempI;
                }
            }
            else
            {
                for (int i = 0; i < (n - 1) / 2; i++)
                {
                    tempI = tempI * koefI;
                    answer[(n - 1) / 2 - 1 - i] = 1 + tempI;
                    answer[(n - 1) / 2 + 1 + i] = 1 + tempI;
                }
                tempI = tempI * koefI;
                answer[(n - 1) / 2] = 1 + tempI;
            }

            return answer;
        }
    }
}
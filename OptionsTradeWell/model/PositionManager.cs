using System;
using System.Collections.Generic;
using OptionsTradeWell.model.interfaces;
using OptionsTradeWell.Properties;

namespace OptionsTradeWell.model
{
    public class PositionManager
    {

        private double[] myLogArr = new double[]
        {
            0.9,
            0.8,
            0.6,
            0.25,
            0.05,
            0.01,
        };

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

        private double CalcMyKoef(double distance, double calcStep)
        {
            int tempIndex = (int)(distance / calcStep);

            if (tempIndex >= myLogArr.Length)
            {
                tempIndex = myLogArr.Length - 1;
            }

            return myLogArr[tempIndex];
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using OptionsTradeWell.Properties;

namespace OptionsTradeWell.model
{
    public class GO_Calculator
    {
        private PositionManager positionManager;
        private double volaUpDeviation;
        private double volaDownDeviation;
        private double volaAdditional;

        private double minPrice;
        private double maxPrice;
        private double d;
        private double calcVol;
        private double strikeStep;

        public GO_Calculator(PositionManager positionManager, double volaUpDeviation, double volaDownDeviation, double volaAdditional)
        {
            this.positionManager = positionManager;
            this.volaUpDeviation = volaUpDeviation;
            this.volaDownDeviation = volaDownDeviation;
            this.volaAdditional = volaAdditional;
        }

        public double CalculateTotalPosition_GO(double actualStrikeVola, double currentPrice)
        {
            UpdateCalcParametresIfCalculationPossible(actualStrikeVola);

            double worstTotalResult = double.MaxValue;
            double worstPrice = 0.0;
            double tempResult;

            double minStrike = Math.Round((currentPrice - d) / strikeStep, 0) * strikeStep;
            double maxStrike = Math.Round((currentPrice + d) / strikeStep, 0) * strikeStep;

            for (double i = minStrike; i <= maxStrike; i += strikeStep)
            {
                tempResult = positionManager.CalculateCurApproxPnL(i);

                if (tempResult < worstTotalResult)
                {
                    worstTotalResult = tempResult;
                    worstPrice = i;
                }
            }

            double[] possibleGOs = new double[]
            {
                d*CalculatePositionsAbsDelta_GO(
                    positionManager.Options,
                    positionManager.Futures,
                    worstPrice,
                    calcVol * (1 - volaDownDeviation)),
                d*CalculatePositionsAbsDelta_GO(
                    positionManager.Options,
                    positionManager.Futures,
                    worstPrice,
                    calcVol),
                d*CalculatePositionsAbsDelta_GO(
                    positionManager.Options,
                    positionManager.Futures,
                    worstPrice,
                    calcVol * (1 + volaUpDeviation)),
                worstTotalResult
            };


            return possibleGOs.Min();
        }

        private bool UpdateCalcParametresIfCalculationPossible(double actualStrikeVola)
        {
            if (positionManager.Options.Count == 0)
            {
                return false;
            }

            minPrice = positionManager.Options[0].Futures.MinPriceLimit;
            maxPrice = positionManager.Options[0].Futures.MaxPriceLimit;
            d = maxPrice - minPrice;
            calcVol = Math.Ceiling(actualStrikeVola * 100.0) / 100.0 + volaAdditional;
            strikeStep = Settings.Default.StrikeStep;

            return true;
        }

        private double CalculatePositionsAbsDelta_GO(List<Option> options, Futures fut, double worstPrice, double worstVola)
        {
            double result = 0.0;
            double tempDelta;

            foreach (Option opt in options)
            {
                tempDelta = GreeksCalculator.CalculateDelta(
                                opt.OptionType,
                                worstPrice,
                                opt.Strike, opt.RemainingDays,
                                GreeksCalculator.DAYS_IN_YEAR,
                                worstVola) * opt.Position.Quantity;

                result += tempDelta;
            }

            if (fut != null)
            {
                result += fut.Position.Quantity;
            }

            return -Math.Abs(result);
        }

    }
}
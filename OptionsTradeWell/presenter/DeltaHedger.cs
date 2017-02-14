using System;
using System.Collections.Generic;
using OptionsTradeWell.model;
using OptionsTradeWell.presenter.interfaces;

namespace OptionsTradeWell.presenter
{
    public class DeltaHedger
    {
        private ITerminalTransactionsImporter transactionsImporter;

        private int maxFutQuantity;
        private double guidePriceForCrossing;
        private double deltaStep;
        private List<double> priceHedgeLevels;
        private bool isMaxFutQuantityEnabled;
        private bool isDeltaStepEnabled;
        private bool isPriceHedgeLevelsEnabled;

        public DeltaHedger(PositionManager posManager, ITerminalTransactionsImporter transactionsImporter, double guidePriceForCrossing)
        {
            this.PosManager = posManager;
            this.transactionsImporter = transactionsImporter;
            this.guidePriceForCrossing = guidePriceForCrossing;

        }

        public PositionManager PosManager { get; }

        public void SetMaxFutQuantityProperty(int quantity)
        {
            if (quantity > 0)
            {
                maxFutQuantity = quantity;
                isMaxFutQuantityEnabled = true;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void DisableMaxFutQuantityProperty()
        {
            isMaxFutQuantityEnabled = false;
        }

        public void SetDeltaStepProperty(double stepValue)
        {
            if (stepValue > 0)
            {
                deltaStep = stepValue;
                isDeltaStepEnabled = true;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void DisableDeltaStepProperty()
        {
            isDeltaStepEnabled = false;
        }

        public void SetPriceHedgeLevelsProperty(List<double> priceLevels)
        {
            foreach (double priceLevel in priceLevels)
            {
                if (priceLevel <= 0)
                {
                    throw new NotImplementedException();
                }
            }

            priceHedgeLevels = priceLevels;
            isPriceHedgeLevelsEnabled = true;
        }

        public void DisableMaxFutQuantity()
        {
            isPriceHedgeLevelsEnabled = false;
        }

        public void DoDeltaHedge()
        {

            if (!PosManager.IsOptionsPositionExists())
            {
                return;
            }

            PosManager.UpdateGeneralParametres();
            double currentDelta = PosManager.TotalDelta;
            double currentPrice = PosManager.GetCurrentPrice();
            double dStep = 1.0;
            int minFutN = 0;
            int maxFutN;

            if (isMaxFutQuantityEnabled)
            {
                maxFutN = maxFutQuantity;
            }
            else
            {
                maxFutN = 0;
                foreach (Option opt in PosManager.Options)
                {
                    maxFutN += opt.GetOptionsPossibleObligation();
                }
            }

            if (isPriceHedgeLevelsEnabled)
            {
                if (IsPriceCrossedNearestLevel(currentPrice))
                {
                    DoHedgeIfNeeded(currentDelta, dStep, minFutN, maxFutN);
                }
            }
            else if (isDeltaStepEnabled)
            {
                dStep = deltaStep;
                DoHedgeIfNeeded(currentDelta, dStep, minFutN, maxFutN);
            }

        }

        private bool DoHedgeIfNeeded(double deltaSum, double dStep, int minHedgeSize, int maxHedgeSize)
        {
            bool result = false;

            int stepsCount = (int)Math.Truncate(deltaSum / dStep);
            int hedgeSize = (int)dStep * stepsCount * -1;


            if (hedgeSize > 0
                && hedgeSize >= minHedgeSize
                && hedgeSize <= maxHedgeSize)
            {
                transactionsImporter.SendMarketBuyOrder(PosManager.Options[0].Futures.Ticker, hedgeSize);
                result = true;
            }
            else if (hedgeSize < 0
                && hedgeSize <= minHedgeSize * -1
                && hedgeSize >= maxHedgeSize * -1)
            {
                transactionsImporter.SendMarketSellOrder(PosManager.Options[0].Futures.Ticker, hedgeSize * -1);
                result = true;
            }


            return result;
        }

        private bool IsPriceCrossedNearestLevel(double currentPrice)
        {
            bool result = false;
            double level;

            if (currentPrice >= guidePriceForCrossing)
            {
                level = GetNearestPriceLevel(currentPrice, CrossDirection.UP);
                if (currentPrice >= level)
                {
                    result = true;
                    guidePriceForCrossing = currentPrice * 1.1; // JUST TO AVOID SITUATION WHEN currentPrice == guide == level
                }
            }
            else
            {
                level = GetNearestPriceLevel(currentPrice, CrossDirection.DOWN);
                if (currentPrice <= level)
                {
                    result = true;
                    guidePriceForCrossing = currentPrice * 0.9; // JUST TO AVOID SITUATION WHEN currentPrice == guide == level
                }
            }

            return result;
        }

        private double GetNearestPriceLevel(double currentPrice, CrossDirection direction)
        {
            double result = Double.MaxValue;
            double tempDiff = 0.0;

            foreach (double priceLevel in priceHedgeLevels)
            {
                tempDiff = Math.Abs(currentPrice - priceLevel);

                if (direction == CrossDirection.UP)
                {
                    if (tempDiff < result && priceLevel >= guidePriceForCrossing)
                    {
                        result = priceLevel;
                    }
                }
                else
                {
                    if (tempDiff < result && priceLevel <= guidePriceForCrossing)
                    {
                        result = priceLevel;
                    }
                }
            }

            return result;
        }

        private enum CrossDirection
        {
            UP,
            DOWN
        }
    }
}
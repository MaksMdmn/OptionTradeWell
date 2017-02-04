using System;
using OptionsTradeWell.model.exceptions;

namespace OptionsTradeWell.model
{
    public class Position
    {
        public int Quantity { get; set; }
        public double EnterPrice { get; set; }
        public Position()
        {
            Quantity = 0;
            EnterPrice = 0.0;
        }

        public double CalcMoney()
        {
            return PosCalcsRoudning(EnterPrice * Quantity * -1.0);
        }

        public double CalcCurrentPnL(TradeBlotter blotter)
        {
            if (Quantity > 0)
            {
                return PosCalcsRoudning((blotter.BidPrice - EnterPrice) * Quantity);
            }
            else if (Quantity < 0)
            {
                return PosCalcsRoudning((blotter.AskPrice - EnterPrice) * Quantity);
            }
            else
            {
                return 0.0;
            }
        }

        public double CalcCurrentPnL(double price)
        {
            TradeBlotter tradeBlotter = new TradeBlotter();
            tradeBlotter.AskPrice = price;
            tradeBlotter.BidPrice = price;
            return CalcCurrentPnL(tradeBlotter);
        }

        public double CalcCurrentPnLInCurrency(TradeBlotter blotter, double step, double stepVal)
        {
            return PosCalcsRoudning(CalcCurrentPnL(blotter) / step * stepVal);
        }

        public double GetMarketPriceToClose(TradeBlotter blotter)
        {
            if (Quantity > 0)
            {
                return blotter.BidPrice;
            }
            else if (Quantity < 0)
            {
                return blotter.AskPrice;
            }
            else
            {
                return 0.0;
            }
        }

        public void AddToExistingPos(double price, int quantity)
        {
            int tempPos = Quantity + quantity;
            if (tempPos == 0)
            {
                throw new ModelCalcsException("Position wouldn't be added, but closed. Please use another method CloseExistingPosAndGetFixedPnL for that.");
            }

            double tempMoney = price * quantity * -1 + CalcMoney();
            EnterPrice = PosCalcsRoudning(Math.Abs(tempMoney / (double)tempPos));
            Quantity = tempPos;

        }

        public double CloseExistingPosAndGetFixedPnL(TradeBlotter blotter)
        {
            double result = PosCalcsRoudning(CalcCurrentPnL(blotter));
            Quantity = 0;
            return result;
        }

        private double PosCalcsRoudning(double value)
        {
            return Math.Round(value, 2);
        }

    }
}
using System.Collections.Generic;
using System.Threading;
using OptionsTradeWell.model.interfaces;
using OptionsTradeWell.presenter.interfaces;

namespace OptionsTradeWell.model
{
    public class TradingAltorithmExecutor
    {
        private ITerminalTransactionsImporter transactionsImporter;
        private ITradingAlgorithm algorithm;
        private bool isWorking;

        public TradingAltorithmExecutor(ITerminalTransactionsImporter transactionsImporter, ITradingAlgorithm algorithm)
        {
            this.transactionsImporter = transactionsImporter;
            this.algorithm = algorithm;
            isWorking = false;
        }

        public void StartExecuteAlgorithm()
        {
            isWorking = true;
            new Thread(new ThreadStart(() =>
            {
                TradingSignal currentSignal;
                string ticker;
                int size;
                double price;

                while (isWorking)
                {
                    currentSignal = algorithm.GetSignal(out ticker, out size, out price);

                    switch (currentSignal)
                    {
                        case (TradingSignal.L_BUY):
                            break;
                        case (TradingSignal.L_SELL):
                            break;
                        case (TradingSignal.M_BUY):
                            transactionsImporter.SendMarketBuyOrder(ticker, size);
                            break;
                        case (TradingSignal.M_SELL):
                            transactionsImporter.SendMarketSellOrder(ticker, size);
                            break;
                        case (TradingSignal.NOTHING):
                            break;
                    }
                }

            })).Start();
        }

        public void StopExecuteAlgorithm()
        {
            isWorking = false;
        }

        public bool IsAlgorithmProcessed()
        {
            return false;
        }

    }
}
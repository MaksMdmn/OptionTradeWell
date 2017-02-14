using System;
using System.Collections.Generic;
using System.Text;
using OptionsTradeWell.assistants;
using OptionsTradeWell.model;
using OptionsTradeWell.presenter.interfaces;

namespace OptionsTradeWell.presenter
{
    public class QuikTransactionsImporter : ITerminalTransactionsImporter
    {
        //make private in future!!!!!!!
        public static string ACCOUNT = "";
        public static string BASE_CONTRACT = "";
        public static string TERMINAL_PATH = "";
        private static int transactionId = 0;

        public QuikTransactionsImporter()
        {
            OrderMap = new Dictionary<double, QuikOrder>();
        }

        public Dictionary<double, QuikOrder> OrderMap { get; }

        public bool ConnectToTerminal()
        {
            return SetConnectionToQuik();
        }

        public bool DisconnectFromTerminal()
        {
            return BreakConnectionToQuik();
        }

        public bool IsConnected()
        {
            return IsQuikConnected() && IsDllConnected();
        }

        public double SendLimitBuyOrder(string ticker, double price, int size)
        {

            string derivativeClass = GetActualClassToTicker(ticker);

            string inputParams = GetTransactionsInputParams(new string[]
            {
                "ACCOUNT=" + ACCOUNT,
                "TRANS_ID=" + GetNewTransactionId(),
                "CLASSCODE=" + derivativeClass,
                "SECCODE=" + ticker,
                "TYPE=" + T2QParametres.TYPE_DEAL_LIMIT,
                "OPERATION=" + T2QParametres.OPERATION_BUY,
                "ACTION=" + T2QParametres.ACTION_NEW_ORDER,
                "PRICE=" + Convert.ToString(price),
                "QUANTITY=" + Convert.ToString(size)
            });

            double result = SendSyncTransactionToQuik(inputParams);

            QuikOrder tempOrder = new QuikOrder(result, ticker, QuikOrderType.LIMIT, QuikOrderOperation.BUY, price, size);
            tempOrder.OrderStatus = QuikOrderStatus.ACTIVE;
            tempOrder.CreationTime = DateTime.Now;

            OrderMap.Add(result, tempOrder);

            return result;
        }

        public double SendLimitSellOrder(string ticker, double price, int size)
        {
            string derivativeClass = GetActualClassToTicker(ticker);

            string inputParams = GetTransactionsInputParams(new string[]
            {
                "ACCOUNT=" + ACCOUNT,
                "TRANS_ID=" + GetNewTransactionId(),
                "CLASSCODE=" + derivativeClass,
                "SECCODE=" + ticker,
                "TYPE=" + T2QParametres.TYPE_DEAL_LIMIT,
                "OPERATION=" + T2QParametres.OPERATION_SELL,
                "ACTION=" + T2QParametres.ACTION_NEW_ORDER,
                "PRICE=" + Convert.ToString(price),
                "QUANTITY=" + Convert.ToString(size)
            });

            double result = SendSyncTransactionToQuik(inputParams);

            QuikOrder tempOrder = new QuikOrder(result, ticker, QuikOrderType.LIMIT, QuikOrderOperation.SELL, price, size);
            tempOrder.OrderStatus = QuikOrderStatus.ACTIVE;
            tempOrder.CreationTime = DateTime.Now;

            OrderMap.Add(result, tempOrder);

            return result;
        }

        public double RollLimitOrder(double id, double price, int size)
        {
            QuikOrder existingOrder;
            if (!OrderMap.TryGetValue(id, out existingOrder))
            {
                throw new NotImplementedException();
            }

            string derivativeClass = GetActualClassToTicker(existingOrder.Ticker);

            string inputParams = GetTransactionsInputParams(new string[]
            {
                "TRANS_ID=" + GetNewTransactionId(),
                "CLASSCODE=" + derivativeClass,
                "SECCODE=" + existingOrder.Ticker,
                "MODE=" + T2QParametres.MODE,
                "ACTION=" + T2QParametres.ACTION_MOVE_ORDER,
                "FIRST_ORDER_NUMBER=" + (int) id,
                "FIRST_ORDER_NEW_PRICE=" + Convert.ToString(price),
                "FIRST_ORDER_NEW_QUANTITY=" + Convert.ToString(size)
            });

            double result = SendSyncTransactionToQuik(inputParams);

            existingOrder.LastTime = DateTime.Now;
            existingOrder.OrderStatus = QuikOrderStatus.CANCELED;
            //AND IF IT WAS EXECUTED?

            QuikOrder tempOrder = new QuikOrder(result, existingOrder.Ticker, QuikOrderType.LIMIT, QuikOrderOperation.SELL, price, size);
            tempOrder.OrderStatus = QuikOrderStatus.ACTIVE;
            tempOrder.CreationTime = DateTime.Now;

            OrderMap.Add(result, tempOrder);

            return result;
        }

        public double SendMarketBuyOrder(string ticker, int size)
        {
            string derivativeClass = GetActualClassToTicker(ticker);

            string inputParams = GetTransactionsInputParams(new string[]
            {
                "ACCOUNT=" + ACCOUNT,
                "TRANS_ID=" + GetNewTransactionId(),
                "CLASSCODE=" + derivativeClass,
                "SECCODE=" + ticker,
                "TYPE=" + T2QParametres.TYPE_DEAL_MARKET,
                "OPERATION=" + T2QParametres.OPERATION_BUY,
                "ACTION=" + T2QParametres.ACTION_NEW_ORDER,
                "PRICE=" + Convert.ToString(0.0),
                "QUANTITY=" + Convert.ToString(size)
            });

            double result = SendSyncTransactionToQuik(inputParams);

            QuikOrder tempOrder = new QuikOrder(result, ticker, QuikOrderType.MARKET, QuikOrderOperation.BUY, 0.0, size);
            tempOrder.OrderStatus = QuikOrderStatus.EXECUTED;
            tempOrder.CreationTime = DateTime.Now;
            tempOrder.LastTime = DateTime.Now;

            OrderMap.Add(result, tempOrder);

            return result;
        }

        public double SendMarketSellOrder(string ticker, int size)
        {
            string derivativeClass = GetActualClassToTicker(ticker);

            string inputParams = GetTransactionsInputParams(new string[]
            {
                "ACCOUNT=" + ACCOUNT,
                "TRANS_ID=" + GetNewTransactionId(),
                "CLASSCODE=" + derivativeClass,
                "SECCODE=" + ticker,
                "TYPE=" + T2QParametres.TYPE_DEAL_MARKET,
                "OPERATION=" + T2QParametres.OPERATION_SELL,
                "ACTION=" + T2QParametres.ACTION_NEW_ORDER,
                "PRICE=" + Convert.ToString(0.0),
                "QUANTITY=" + Convert.ToString(size)
            });

            double result = SendSyncTransactionToQuik(inputParams);

            QuikOrder tempOrder = new QuikOrder(result, ticker, QuikOrderType.MARKET, QuikOrderOperation.SELL, 0.0, size);
            tempOrder.OrderStatus = QuikOrderStatus.EXECUTED;
            tempOrder.CreationTime = DateTime.Now;
            tempOrder.LastTime = DateTime.Now;

            OrderMap.Add(result, tempOrder);

            return result;
        }

        public bool CancelOrder(double id)
        {
            string inputParams = GetTransactionsInputParams(new string[]
             {
                "TRANS_ID=" + GetNewTransactionId(),
                "CLASSCODE=" + GetActualClassToTicker(OrderMap[id].Ticker),
                "ACTION=" + T2QParametres.ACTION_CANCEL_ORDER,
                "ORDER_KEY=" + (int)id
             });

            double result = -1;
            result = SendSyncTransactionToQuik(inputParams);

            if (result != -1)
            {
                OrderMap[id].LastTime = DateTime.Now;
                OrderMap[id].OrderStatus = QuikOrderStatus.CANCELED;

                return true;
            }

            return false;
        }

        public bool CancelAllOrders(DerivativesClasses cls)
        {
            string derivativeClass = cls == DerivativesClasses.FUTURES
                ? T2QParametres.CLASS_CODE_FUT
                : T2QParametres.CLASS_CODE_OPT;

            string inputParams = GetTransactionsInputParams(new string[]
             {
                "ACCOUNT=" + ACCOUNT,
                "TRANS_ID=" + GetNewTransactionId(),
                "CLASSCODE=" + derivativeClass,
                "ACTION=" + T2QParametres.ACTION_CANCEL_ALL_ORDERS,
                "BASE_CONTRACT=" + BASE_CONTRACT
             });

            double result = -1;
            result = SendSyncTransactionToQuik(inputParams);

            if (result != -1)
            {
                foreach (QuikOrder order in OrderMap.Values)
                {
                    order.LastTime = DateTime.Now;
                    order.OrderStatus = QuikOrderStatus.CANCELED;
                }

                return true;
            }

            return false;
        }

        private double SendSyncTransactionToQuik(string inputParams)
        {
            int errCode;
            int replCode;
            uint transID;
            double orderNum;
            StringBuilder statusStringBuilder = new StringBuilder(512);
            StringBuilder errorMessageBuilder = new StringBuilder(512);

            T2Q.TRANS2QUIK_SEND_SYNC_TRANSACTION(inputParams, out replCode, out transID, out orderNum,
                statusStringBuilder, 512, out errCode, errorMessageBuilder, 512);

            //switch (errCode)
            //{
            //    case (int)T2QResultsCode.Failed:
            //        throw new System.NotImplementedException();
            //    default:
            //        throw new System.NotImplementedException();
            //}


            return orderNum;
        }

        private bool SetConnectionToQuik()
        {
            int errCode;
            StringBuilder errorMessageBuilder = new StringBuilder(512);
            return T2Q.TRANS2QUIK_CONNECT(TERMINAL_PATH, out errCode, errorMessageBuilder, 512) == 0;
        }

        private bool BreakConnectionToQuik()
        {
            int errCode;
            StringBuilder errorMessageBuilder = new StringBuilder(512);
            return T2Q.TRANS2QUIK_DISCONNECT(out errCode, errorMessageBuilder, 512) == 0;
        }

        private bool IsQuikConnected()
        {
            int errCode;
            StringBuilder errorMessageBuilder = new StringBuilder(512);
            return T2Q.TRANS2QUIK_IS_QUIK_CONNECTED(out errCode, errorMessageBuilder, 512) == 8;
        }

        private bool IsDllConnected()
        {
            int errCode;
            StringBuilder errorMessageBuilder = new StringBuilder(512);
            return T2Q.TRANS2QUIK_IS_DLL_CONNECTED(out errCode, errorMessageBuilder, 512) == 10;
        }

        private string GetTransactionsInputParams(string[] args)
        {
            if (args.Length == 0)
            {
                return "";
            }
            return String.Join("; ", args) + ";";
        }

        private string GetActualClassToTicker(string ticker)
        {
            return ticker.Length > 4 ? T2QParametres.CLASS_CODE_OPT : T2QParametres.CLASS_CODE_FUT;
        }
        private int GetNewTransactionId()
        {
            return ++transactionId;
        }

    }
}
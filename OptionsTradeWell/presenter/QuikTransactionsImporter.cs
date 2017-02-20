using System;
using System.Collections.Generic;
using System.Text;
using OptionsTradeWell.assistants;
using OptionsTradeWell.model;
using OptionsTradeWell.presenter.interfaces;
using OptionsTradeWell.Properties;

namespace OptionsTradeWell.presenter
{
    public class QuikTransactionsImporter : ITerminalTransactionsImporter
    {
        private static int transactionId = 0;
        private string account = Settings.Default.Account;
        private string terminalPath = Settings.Default.PathToQuik;

        #region ITerminalTransactionsImporter_Implementation
        public QuikTransactionsImporter()
        {
            OrderMap = new Dictionary<double, TerminalOrder>();
        }

        public QuikTransactionsImporter(string account, string path)
        {
            this.account = account;
            this.terminalPath = path;
            OrderMap = new Dictionary<double, TerminalOrder>();
        }


        public Dictionary<double, TerminalOrder> OrderMap { get; }

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
                "ACCOUNT=" + account,
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

            TerminalOrder tempOrder = new TerminalOrder(result, ticker, TerminalOrderType.LIMIT, TerminalOrderOperation.BUY,
                price, size, TerminalOrderStatus.ACTIVE);
            tempOrder.CreationTime = DateTime.Now;

            OrderMap.Add(result, tempOrder);

            return result;
        }

        public double SendLimitSellOrder(string ticker, double price, int size)
        {
            string derivativeClass = GetActualClassToTicker(ticker);

            string inputParams = GetTransactionsInputParams(new string[]
            {
                "ACCOUNT=" + account,
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

            TerminalOrder tempOrder = new TerminalOrder(result, ticker, TerminalOrderType.LIMIT, TerminalOrderOperation.SELL,
                price, size, TerminalOrderStatus.ACTIVE);
            tempOrder.CreationTime = DateTime.Now;

            OrderMap.Add(result, tempOrder);

            return result;
        }

        public double RollLimitOrder(double id, double price, int size)
        {
            TerminalOrder existingOrder;
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
            existingOrder.OrderStatus = TerminalOrderStatus.CANCELED;
            //AND IF IT WAS EXECUTED?

            TerminalOrder tempOrder = new TerminalOrder(result, existingOrder.Ticker, TerminalOrderType.LIMIT, TerminalOrderOperation.SELL,
                price, size, TerminalOrderStatus.ACTIVE);
            tempOrder.CreationTime = DateTime.Now;

            OrderMap.Add(result, tempOrder);

            return result;
        }

        public double SendMarketBuyOrder(string ticker, int size)
        {
            string derivativeClass = GetActualClassToTicker(ticker);

            string inputParams = GetTransactionsInputParams(new string[]
            {
                "ACCOUNT=" + account,
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

            TerminalOrder tempOrder = new TerminalOrder(result, ticker, TerminalOrderType.MARKET, TerminalOrderOperation.BUY,
                0.0, size, TerminalOrderStatus.EXECUTED);
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
                "ACCOUNT=" + account,
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

            TerminalOrder tempOrder = new TerminalOrder(result, ticker, TerminalOrderType.MARKET, TerminalOrderOperation.SELL,
                0.0, size, TerminalOrderStatus.EXECUTED);
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
                OrderMap[id].OrderStatus = TerminalOrderStatus.CANCELED;

                return true;
            }

            return false;
        }

        public bool CancelGroupOfOrders(DerivativesClasses cls, string baseContract)
        {
            string derivativeClass = cls == DerivativesClasses.FUTURES
                ? T2QParametres.CLASS_CODE_FUT
                : T2QParametres.CLASS_CODE_OPT;

            string inputParams = GetTransactionsInputParams(new string[]
             {
                "ACCOUNT=" + account,
                "TRANS_ID=" + GetNewTransactionId(),
                "CLASSCODE=" + derivativeClass,
                "ACTION=" + T2QParametres.ACTION_CANCEL_ALL_ORDERS,
                "BASE_CONTRACT=" + baseContract
             });

            double result = -1;
            result = SendSyncTransactionToQuik(inputParams);

            if (result != -1)
            {
                foreach (TerminalOrder order in OrderMap.Values)
                {
                    order.LastTime = DateTime.Now;
                    order.OrderStatus = TerminalOrderStatus.CANCELED;
                }

                return true;
            }

            return false;
        }

        public bool CancelAllOrders()
        {
            throw new NotImplementedException();
        }
        #endregion


        #region TRANS2QUIK_DDL_Implementation
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


            Console.WriteLine();
            Console.WriteLine("QUIK MESSAGE BEGINS");
            Console.WriteLine("error code: " + errCode);
            Console.WriteLine("reply code: " + replCode);
            Console.WriteLine("trans id: " + transID);
            Console.WriteLine("order id: " + orderNum);
            Console.WriteLine("status message: " + statusStringBuilder);
            Console.WriteLine("error message: " + errorMessageBuilder);
            Console.WriteLine("QUIK MESSAGE ENDS");
            Console.WriteLine();

            return orderNum;
        }

        private bool SetConnectionToQuik()
        {
            int errCode;
            StringBuilder errorMessageBuilder = new StringBuilder(512);
            return T2Q.TRANS2QUIK_CONNECT(terminalPath, out errCode, errorMessageBuilder, 512) == 0;
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

        public double TestSubscribeOrders(string classCode, string secCode)
        {
            return T2Q.TRANS2QUIK_SUBSCRIBE_ORDERS(classCode, secCode);
        }

        public double TestStartGetOrders()
        {
            return T2Q.TRANS2QUIK_START_ORDERS(OrdersCallBack);
        }

        public double TestUnsubscribeOrders()
        {
            return T2Q.TRANS2QUIK_UNSUBSCRIBE_ORDERS();
        }


        public double TestSubscribeTrades(string classCode, string secCode)
        {
            return T2Q.TRANS2QUIK_SUBSCRIBE_TRADES(classCode, secCode);
        }

        public double TestStartGetTrades()
        {
            return T2Q.TRANS2QUIK_START_TRADES(TradesCallBack);
        }

        public double TestUnsubscribeTrades()
        {
            return T2Q.TRANS2QUIK_UNSUBSCRIBE_TRADES();
        }

        private void OrdersCallBack(int nmode, uint dwtransid, double dnumber, string lpstrclasscode, string lpstrseccode, 
            double dprice, long nbalance, double dvalue, int nlssell, int nstatus)
        {
            Console.WriteLine("ORDERS UPDATE:");
            Console.WriteLine("nmode: " + nmode);
            Console.WriteLine("dwtransid: " + dwtransid);
            Console.WriteLine("dnumber: " + dnumber);
            Console.WriteLine("lpstrclasscode: " + lpstrclasscode);
            Console.WriteLine("lpstrseccode: " + lpstrseccode);
            Console.WriteLine("dprice: " + dprice);
            Console.WriteLine("nbalance: " + nbalance);
            Console.WriteLine("dvalue: " + dvalue);
            Console.WriteLine("nlssell: " + nlssell);
            Console.WriteLine("nstatus: " + nstatus);
            Console.WriteLine("ORDERS UPDATE END");
            Console.WriteLine();

        }

        private void TradesCallBack(int nmode, double dnumber, double dordernum, string lpstrclasscode, string lpstrseccode,
            double dprice, Int64 nqty, double dvalue, int nlssell)
        {
            Console.WriteLine("TRADES UPDATE:");
            Console.WriteLine("nmode: " + nmode);
            Console.WriteLine("dnumber: " + dnumber);
            Console.WriteLine("dordernum: " + dordernum);
            Console.WriteLine("lpstrclasscode: " + lpstrclasscode);
            Console.WriteLine("lpstrseccode: " + lpstrseccode);
            Console.WriteLine("dprice: " + dprice);
            Console.WriteLine("nqty: " + nqty);
            Console.WriteLine("dvalue: " + dvalue);
            Console.WriteLine("nlssell: " + nlssell);
            Console.WriteLine("TRADES UPDATE END");
            Console.WriteLine();

        }


        #endregion

        private void UpdateOrderMap(TerminalOrder order)
        {
            if (OrderMap.ContainsKey(order.Id))
            {
                OrderMap[order.Id] = order;
            }
            else
            {
                OrderMap.Add(order.Id, order);
            }
        }
    }
}
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OptionsTradeWell.assistants
{
    public static class T2Q
    {
        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_CONNECT
        (
        String lpcstrConnectionParamsString,
        out Int32 pnExtendedErrorCode,
        [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpstrErrorMessage,
        UInt32 dwErrorMessageSize
        );

        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_DISCONNECT
        (
        out Int32 pnExtendedErrorCode,
        [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpstrErrorMessage,
        UInt32 dwErrorMessageSize
        );


        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_IS_QUIK_CONNECTED
        (
        out Int32 pnExtendedErrorCode,
        [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpstrErrorMessage,
        UInt32 dwErrorMessageSize
        );

        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_IS_DLL_CONNECTED
        (
        out Int32 pnExtendedErrorCode,
        [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpstrErrorMessage,
        UInt32 dwErrorMessageSize
        );

        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_SEND_SYNC_TRANSACTION
        (
            String lpstTransactionString,
            out Int32 pnReplyCode,
            out UInt32 pdwTransId,
            out Double pdOrderNum,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpstrResultMessage,
            UInt32 dwResultMessageSize,
            out Int32 pnExtendedErrorCode,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpstErrorMessage,
            UInt32 dwErrorMessageSize
        );

        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_SUBSCRIBE_ORDERS
        (
            String lpstrClassCode,
            String lpstrSeccodes
        );

        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_START_ORDERS
        (
            TRANS2QUIK_ORDERS_STATUS_CALLBACK pfnTradesStatusCallback
        );

        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_UNSUBSCRIBE_ORDERS
        (
        );

        public delegate void TRANS2QUIK_ORDERS_STATUS_CALLBACK
        (
            Int32 nMode,
            UInt32 dwTransID,
            Double dNumber,
            String lpstrClassCode,
            String lpstrSecCode,
            Double dPrice,
            Int64 nBalance,
            Double dValue,
            Int32 nlsSell,
            Int32 nStatus
        );

        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_SUBSCRIBE_TRADES
        (
            String lpstrClassCode,
            String lpstrSeccodes
        );

        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_START_TRADES
        (
            TRANS2QUIK_TRADES_STATUS_CALLBACK pfnTradessStatusCallback
        );

        [DllImport("TRANS2QUIK.dll")]
        public static extern int TRANS2QUIK_UNSUBSCRIBE_TRADES
        (
        );

        public delegate void TRANS2QUIK_TRADES_STATUS_CALLBACK
        (
            Int32 nMode,
            Double dNumber,
            Double dOrderNum,
            String lpstrClassCode,
            String lpstrSecCode,
            Double dPrice,
            Int64 nQty,
            Double dValue,
            Int32 nlsSell
        );
    }
}
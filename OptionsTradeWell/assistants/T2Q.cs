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

        //[DllImport("TRANS2QUIK.dll")]
        //static extern int TRANS2QUIK_SEND_ASYNC_TRANSACTION
        //(
        //String lpstTransactionString,
        //out Int32 pnExtendedErrorCode,
        //[MarshalAs(UnmanagedType.LPStr)] StringBuilder lpstErrorMessage,
        //UInt32 dwErrorMessageSize
        //);
    }
}
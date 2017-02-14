namespace OptionsTradeWell.assistants
{
    public enum T2QResultsCode
    {
        Success = 0,
        Failed = 1,
        QuikTerminalNotFound = 2,
        DllVersionNotSupported = 3,
        DllAlreadyConnectedToQuik = 4,
        WrongSyntax = 5,
        QuikNotConnected = 6,
        DllNotConnected = 7,
        QuikConnected = 8,
        QuikDisconnected = 9,
        DllConnected = 10,
        DllDisconnected = 11,
        MemoryAllocationError = 12,
        WrongConnectionHandle = 13,
        WrongInputParams = 14
    }
}
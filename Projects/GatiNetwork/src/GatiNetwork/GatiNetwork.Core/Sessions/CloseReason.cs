namespace GatiNetwork.Core.Sessions
{
    public enum CloseReason
    {
        NormalClosure,
        ServerShutdown,
        Timeout,
        ProtocolError,
        UnknownError
    }
}

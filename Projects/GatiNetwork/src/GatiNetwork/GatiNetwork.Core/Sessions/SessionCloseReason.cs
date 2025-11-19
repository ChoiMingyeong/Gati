namespace GatiNetwork.Core.Sessions
{
    public enum SessionCloseReason
    {
        Normal,
        RemoteClosed,
        Timeout,
        ProtocolError,
        TransportError,
        ServerShutdown,
        Kicked,
        Unknown
    }
}

using GatiNetwork.Core.RecordStructs;

namespace GatiNetwork.Core.Sessions
{
    public class SessionClosedEventArgs(SessionID sessionID, SessionCloseReason reason, string? description, Exception? exception) : EventArgs
    {
        public SessionID SessionID { get; } = sessionID;

        public SessionCloseReason CloseReason { get; } = reason;

        public string? Description { get; } = description;

        public Exception? Exception { get; } = exception;
    }
}

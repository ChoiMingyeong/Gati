using GatiNetwork.Core.RecordStructs;

namespace GatiNetwork.Core.Sessions
{
    public class SessionFaultedEventArgs(SessionID sessionID, Exception exception) : EventArgs
    {
        public SessionID SessionID { get; } = sessionID;

        public Exception Exception { get; } = exception;

    }
}

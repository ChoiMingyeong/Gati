using GatiNetwork.Core.RecordStructs;

namespace GatiNetwork.Core.Sessions
{
    public class SessionConnectedEventArgs(SessionID sessionID) : EventArgs
    {
        public SessionID SessionID { get; } = sessionID;
    }
}

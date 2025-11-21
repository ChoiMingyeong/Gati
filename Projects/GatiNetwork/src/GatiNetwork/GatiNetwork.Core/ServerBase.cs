using GatiNetwork.Core.PacketHandlers;

namespace GatiNetwork.Core
{
    public abstract class ServerBase : IServer
    {
        public ServerBase()
        {
            PacketHandlerRegistry.Initialize();
        }

        public abstract void Initialize();

        public abstract void Start();

        public abstract void Stop();
    }
}

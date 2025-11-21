using GatiNetwork.Core.PacketHandlers;

namespace GatiNetwork.Server
{
    public abstract class ServerBase<TServerArgs> : IServer<TServerArgs>
        where TServerArgs : IServerArgs
    {
        protected bool _initialize = false;

        public ServerBase()
        {
            PacketHandlerRegistry.Initialize();
        }

        public abstract void Initialize(in TServerArgs args);

        public abstract Task Run();

        public abstract void Stop();
    }
}

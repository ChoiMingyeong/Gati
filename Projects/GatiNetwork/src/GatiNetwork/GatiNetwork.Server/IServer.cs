using GatiNetwork.Core;

namespace GatiNetwork.Server
{
    internal interface IServer<TServerArgs> 
        where TServerArgs : IServerArgs
    {
        void Initialize(in TServerArgs args);

        Task Run();

        void Stop();
    }
}

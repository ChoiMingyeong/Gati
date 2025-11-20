using GatiNetwork.Core.Sessions;

namespace GatiNetwork.Core.PacketHandlers
{
    public sealed class PacketHandlerInvoker<TPacket> : IPacketHandlerInvoker
    where TPacket : IPacket
    {
        private readonly IPacketHandler<TPacket> _inner;

        public PacketHandlerInvoker(IPacketHandler<TPacket> inner)
        {
            _inner = inner;
        }

        public Task ExecuteAsync(ClientSession session, IPacket packet)
        {
            // 타입 체크 + 캐스팅
            if (packet is not TPacket typed)
            {
                throw new InvalidOperationException($"Handler for {typeof(TPacket).Name} got {packet.GetType().Name}");
            }

            return _inner.ExecuteAsync(session, typed);
        }
    }
}

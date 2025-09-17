using Microsoft.Win32;

namespace GatiWeb.Core.Packet
{
    public interface IPacketRouter
    {
        public void RegisterProtocolMethods();
    }
}

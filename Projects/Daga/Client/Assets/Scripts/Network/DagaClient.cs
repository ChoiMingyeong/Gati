using UnityEngine;
using WebCore.Socket.Client;

namespace Assets.Scripts.Network
{
    public class DagaClient : MonoBehaviour
    {
        private GatiClient<DagaClientRouter> _gatiClient = new();
        public bool IsConnected => IsConnected;

        private readonly string _address;

        public DagaClient(string ip, int port)
        {
            _gatiClient.ConnectAsync($"ws://{ip}:{port}/ws/").GetAwaiter();
        }

        public void Send(string message)
        {
            _gatiClient.SendAsync(new TestCommon.Shared.C2S.RequestTest
            {
                Message = message
            }).GetAwaiter();
        }
    }
}

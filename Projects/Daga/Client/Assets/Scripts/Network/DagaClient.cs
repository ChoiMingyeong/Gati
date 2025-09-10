using System.Threading.Tasks;
using UnityEngine;
using WebCore.Socket.Client;

namespace Assets.Scripts.Network
{
    public class DagaClient : MonoBehaviour
    {
        private GatiClient<DagaClientRouter> _gatiClient = new();
        public bool IsConnected => IsConnected;

        public DagaClient()
        {
        }

        [ContextMenu("Connect")]
        public async Task ConnectAsync()
        {
            await _gatiClient.ConnectAsync($"ws://localhost:5060/ws/");
        }

        [ContextMenu("Send")]
        public async Task SendAsync()
        {
            await _gatiClient.SendAsync(new TestCommon.Shared.C2S.RequestTest
            {
                Message = "Test"
            });
        }
    }
}

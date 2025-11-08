using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

namespace EclipseRealms.Net
{
    /// <summary>
    /// Small helper to start/stop FishNet sessions from UI buttons without wiring custom scripts each time.
    /// </summary>
    [RequireComponent(typeof(NetworkManager))]
    public class FishNetBootstrapper : MonoBehaviour
    {
        private NetworkManager networkManager;

        private void Awake()
        {
            networkManager = GetComponent<NetworkManager>();
        }

        [ContextMenu("Start Host")]
        public void StartHost()
        {
            networkManager.ServerManager.StartConnection();
            networkManager.ClientManager.StartConnection();
        }

        [ContextMenu("Start Client")]
        public void StartClient()
        {
            networkManager.ClientManager.StartConnection();
        }

        [ContextMenu("Stop All")]
        public void StopAll()
        {
            networkManager.ClientManager.StopConnection();
            networkManager.ServerManager.StopConnection(true);
        }
    }
}

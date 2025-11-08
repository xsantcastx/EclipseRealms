using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using UnityEngine;

namespace EclipseRealms.Net.Debugging
{
    /// <summary>
    /// Logs FishNet connection state transitions to help validate host/client sessions quickly.
    /// </summary>
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkSessionValidator : MonoBehaviour
    {
        [SerializeField] private bool logRemoteConnections = false;

        private NetworkManager networkManager;

        private void Awake()
        {
            networkManager = GetComponent<NetworkManager>();
        }

        private void OnEnable()
        {
            networkManager.ServerManager.OnServerConnectionState += HandleServerState;
            networkManager.ClientManager.OnClientConnectionState += HandleClientState;

            if (logRemoteConnections)
            {
                networkManager.ServerManager.OnRemoteConnectionState += HandleRemoteState;
            }
        }

        private void OnDisable()
        {
            if (networkManager == null)
            {
                return;
            }

            networkManager.ServerManager.OnServerConnectionState -= HandleServerState;
            networkManager.ClientManager.OnClientConnectionState -= HandleClientState;

            if (logRemoteConnections)
            {
                networkManager.ServerManager.OnRemoteConnectionState -= HandleRemoteState;
            }
        }

        private void HandleServerState(ServerConnectionStateArgs args)
        {
            Debug.Log($"[FishNet] Server state: {args.ConnectionState}");
        }

        private void HandleClientState(ClientConnectionStateArgs args)
        {
            Debug.Log($"[FishNet] Client state: {args.ConnectionState}");
        }

        private void HandleRemoteState(FishNet.Connection.NetworkConnection conn, RemoteConnectionStateArgs args)
        {
            Debug.Log($"[FishNet] Remote client {conn.ClientId} -> {args.ConnectionState}");
        }
    }
}

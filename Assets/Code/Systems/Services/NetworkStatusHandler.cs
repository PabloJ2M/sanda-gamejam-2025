using UnityEngine;
using UnityEngine.Events;

namespace Unity.Netcode
{
    public class NetworkStatusHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool> _onLouding;
        [SerializeField] private UnityEvent<bool> _onDisplayChanged;
        private NetworkManager _manager;

        private void Start()
        {
            _onLouding.Invoke(false);
            _onDisplayChanged.Invoke(true);

            _manager = NetworkManager.Singleton;
            _manager.OnClientStarted += HandlerClientStarted;
            _manager.OnClientConnectedCallback += HandleClientConnected;
            _manager.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        private void HandlerClientStarted() => _onLouding.Invoke(true);
        private void HandleClientConnected(ulong clientId) { _onDisplayChanged.Invoke(false); _onLouding.Invoke(false); }
        private void HandleClientDisconnect(ulong clientId) => _onDisplayChanged.Invoke(true);
    }
}
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.Multiplayer.Widgets
{
    public class NetworkStatusHandler : SingletonBasic<NetworkStatusHandler>
    {
        [SerializeField] private UnityEvent<bool> _onLouding;
        [SerializeField] private UnityEvent<bool> _onDisplayChanged;

        private void Start()
        {
            _onLouding.Invoke(false);
            _onDisplayChanged.Invoke(true);
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconect;
        }
        private void OnDisable() => NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconect;

        public void HandleJoinStarted() => _onLouding.Invoke(true);
        public void HandleJoinFaulure() => _onLouding.Invoke(false);
        public void HandleJoinSucceded() { _onDisplayChanged.Invoke(false); _onLouding.Invoke(false); }
        public void HandleLeaveSession() => _onDisplayChanged.Invoke(true);
        private void HandleClientDisconect(ulong id)
        {
            if (NetworkManager.Singleton.LocalClientId == id)
                HandleLeaveSession();
        }
    }
}
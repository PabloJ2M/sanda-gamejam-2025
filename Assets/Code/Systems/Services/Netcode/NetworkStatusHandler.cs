using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

namespace Unity.Multiplayer.Widgets
{
    public class NetworkStatusHandler : SingletonBasic<NetworkStatusHandler>
    {
        [SerializeField] private UnityEvent<bool> _onLouding;
        [SerializeField] private UnityEvent<bool> _onDisplayChanged;
        private readonly WaitForSeconds _delay = new(2f);

        private void Start() => NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconect;

        public void HandleJoinStarted() => _onLouding.Invoke(true);
        public void HandleJoinFaulure() => _onLouding.Invoke(false);
        public void HandleJoinSucceded() => StartCoroutine(DelayJoinnedAction());
        public void HandleLeaveSession() => _onDisplayChanged.Invoke(true);

        private void HandleClientDisconect(ulong id)
        {
            if (NetworkManager.Singleton.LocalClientId != id) return;
            HandleLeaveSession();
        }
        private IEnumerator DelayJoinnedAction()
        {
            _onDisplayChanged.Invoke(false);

            yield return _delay;
            _onLouding.Invoke(false);
        }
    }
}
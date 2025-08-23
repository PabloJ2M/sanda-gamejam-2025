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

        private void Start()
        {
            _onLouding.Invoke(false);
            _onDisplayChanged.Invoke(true);
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconect;
        }

        public void HandleJoinStarted() => _onLouding.Invoke(true);
        public void HandleJoinFaulure() => _onLouding.Invoke(false);
        public void HandleJoinSucceded() => StartCoroutine(DelayJoinnedAction());
        public void HandleLeaveSession() => _onDisplayChanged.Invoke(true);

        private void HandleClientDisconect(ulong id)
        {
            if (NetworkManager.Singleton.LocalClientId == id)
                HandleLeaveSession();
        }
        private IEnumerator DelayJoinnedAction()
        {
            yield return _delay;
            _onDisplayChanged.Invoke(false);
            _onLouding.Invoke(false);
        }
    }
}
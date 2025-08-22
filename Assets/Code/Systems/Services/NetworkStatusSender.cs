using UnityEngine;

namespace Unity.Multiplayer.Widgets
{
    public class NetworkStatusSender : MonoBehaviour
    {
        public void OnJoinningSession() => NetworkStatusHandler.Instance.HandleJoinStarted();
        public void OnJoinedSession() => NetworkStatusHandler.Instance.HandleJoinSucceded();
        public void OnFailure() => NetworkStatusHandler.Instance.HandleJoinFaulure();
    }
}
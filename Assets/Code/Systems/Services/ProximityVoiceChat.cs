using Unity.Netcode;
using Unity.Services.Vivox;

public class ProximityVoiceChat : NetworkBehaviour
{
    private IVivoxService _service;
    private string _channelName;

    private void Awake() => _service = VivoxService.Instance;
    private void OnEnable() => _service.ChannelJoined += OnJoinedChannel;
    private void OnDisable() => _service.ChannelJoined -= OnJoinedChannel;

    private void OnJoinedChannel(string channelName) => _channelName = channelName;

    private void FixedUpdate()
    {
        if (!IsOwner || string.IsNullOrEmpty(_channelName)) return;
        _service.Set3DPosition(gameObject, _channelName, true);
    }
}
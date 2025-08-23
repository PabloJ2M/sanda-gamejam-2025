using UnityEngine;
using UnityEngine.UI;

public class SettingsAudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixerChannels _channel;
    [SerializeField] private Slider _slider;
    private Settings _settings;

    private void Awake() => _settings = Settings.Instance;
    private void Start() => _slider.SetValueWithoutNotify(_settings.GetVolumeType(_channel));
    private void OnEnable() => _slider.onValueChanged.AddListener(PerformeValueChange);
    private void OnDisable() => _slider.onValueChanged.RemoveListener(PerformeValueChange);

    private void PerformeValueChange(float value) => _settings.SetVolumeType(_channel, value);
}
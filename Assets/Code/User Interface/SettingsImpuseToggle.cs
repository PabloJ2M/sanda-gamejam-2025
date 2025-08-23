using UnityEngine;
using UnityEngine.UI;

public class SettingsImpuseToggle : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private int _index;
    private Settings _settings;

    private void Awake() => _settings = Settings.Instance;
    private void Start() => _toggle.isOn = _settings.GetImpulseType() == _index;
    private void OnEnable() => _toggle.onValueChanged.AddListener(PerformeValueChange);
    private void OnDisable() => _toggle.onValueChanged.RemoveListener(PerformeValueChange);

    private void PerformeValueChange(bool value)
    {
        if (!value) return;
        _settings.SetImpulseType(_index);
    }
}
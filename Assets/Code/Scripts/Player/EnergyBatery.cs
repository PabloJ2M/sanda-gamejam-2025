using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBatery : SingletonBasic<EnergyBatery>
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _consumeSpeed, _recoverySpeed;

    private IImpulseStrategy _impulse;
    private bool _lockedEngine;

    private void Start() => ReadInputType();
    private void OnEnable() => Settings.OnSaveValues += ReadInputType;
    private void OnDisable() => Settings.OnSaveValues -= ReadInputType;
    private void ReadInputType() => _impulse = Settings.ImpulseType;

    private void FixedUpdate()
    {
        if (_slider.value >= 1f || _lockedEngine) return;
        _slider.value += _impulse.GetImpulse() * _recoverySpeed * Time.fixedDeltaTime;
    }
    public bool Impulse()
    {
        if (_lockedEngine) return false;
        
        _slider.value -= _consumeSpeed * Time.fixedDeltaTime;
        _lockedEngine = _slider.value <= 0f;

        if (_lockedEngine) StartCoroutine(RecoveryTime());
        return !_lockedEngine;
    }

    private IEnumerator RecoveryTime()
    {
        yield return new WaitForSeconds(5f);
        _lockedEngine = false;
    }
}
using UnityEngine;
using UnityEngine.Events;

public class Visibility : MonoBehaviour
{
    [SerializeField] private UnityEvent _onVisible, _onInvisible;

    private void OnBecameVisible() => _onVisible?.Invoke();
    private void OnBecameInvisible() => _onInvisible?.Invoke();
}
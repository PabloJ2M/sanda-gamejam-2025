using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KeyPressed : MonoBehaviour
{
    [SerializeField] private InputAction _action;
    [SerializeField] private UnityEvent<bool> _onStatusChanged;

    private bool _isPressed;

    private void Awake() => _action.performed += PerformeActionCallback;
    private void OnEnable() => _action.Enable();
    private void OnDisable() => _action.Disable();

    private void PerformeActionCallback(InputAction.CallbackContext ctx)
    {
        _isPressed = !_isPressed;
        _onStatusChanged.Invoke(_isPressed);
    }
}
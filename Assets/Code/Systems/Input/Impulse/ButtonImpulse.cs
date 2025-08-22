using UnityEngine.InputSystem;

public class ButtonImpulse : IImpulseStrategy
{
    private InputAction _action;

    public ButtonImpulse(InputAction action)
    {
        _action = action;
        _action.Enable();
    }

    public float GetImpulse() => _action.ReadValue<float>();
}
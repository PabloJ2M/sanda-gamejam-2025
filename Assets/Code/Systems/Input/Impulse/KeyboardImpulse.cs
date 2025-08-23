using UnityEngine;
//using UnityEngine.InputSystem;

public class KeyboardImpulse : IImpulseStrategy
{
    //private InputAction _action;

    //public KeyboardImpulse(InputAction action)
    //{
    //    _action = action;
    //    _action.Enable();
    //}

    public float GetImpulse() => Mathf.Clamp01(Input.GetAxisRaw("Vertical")); /*_action.ReadValue<float>();*/
}
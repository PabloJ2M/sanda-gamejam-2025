using Unity.Cinemachine;
using UnityEngine.InputSystem;

public interface IMovementState
{
    public CinemachineCameraType CameraType { get; }

    public void Start(MovementController move);
    public void Close();

    public void HandleInput(InputValue input);
    public void FixedUpdate(MovementController move);
}
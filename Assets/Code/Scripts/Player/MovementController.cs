using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Unity.Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : NetworkBehaviour
{
    private IMovementState _currentStage;
    private CinemachineStateCamera _cameras;

    public Transform Head { get; private set; }
    [field: SerializeField] public Transform Mesh { get; private set; }
    [field: SerializeField] public Rigidbody RigidBody { get; private set; }

    public IImpulseStrategy Impulse { get; private set; }
    public IGravityStrategy Atraction { get; private set; }

    private void Awake()
    {
        Head = Camera.main.transform;
        _cameras = CinemachineStateCamera.Instance;
    }
    protected override void OnNetworkPostSpawn()
    {
        base.OnNetworkPostSpawn();
        if (!IsOwner) return;

        _cameras.SetTarget(transform);
        SetImpulse(new VoiceImpulse());
        SwitchState(new SpaceMovement(), null);
    }

    private void FixedUpdate() { if (IsOwner) _currentStage?.FixedUpdate(this); }
    private void OnMove(InputValue value) { if (IsOwner) _currentStage?.HandleInput(value); }
    private void OnJump() { if (_currentStage is not SpaceMovement) SwitchState(new SpaceMovement(), null); }

    public void SetImpulse(IImpulseStrategy strategy) => Impulse = strategy;
    public void SwitchState(IMovementState newState, IGravityStrategy gravity)
    {
        Atraction = gravity;
        _currentStage?.Close();
        _currentStage = newState;
        _currentStage?.Start(this);
        _cameras?.SetCamera(newState.CameraType);
    }
}
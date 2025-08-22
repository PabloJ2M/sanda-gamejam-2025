using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlanetMovement : IMovementState
{
    public CinemachineCameraType CameraType => CinemachineCameraType.ThreeRig;

    private readonly LayerMask _ground = LayerMask.NameToLayer("Ground");
    private const QueryTriggerInteraction _query = QueryTriggerInteraction.Ignore;

    private MovementController _controller;
    private Transform _transform;

    private RaycastHit _groundHit;
    private Vector3 _movement, _gravityDirection;
    private Vector2 _input;

    public void Start(MovementController move)
    {
        _controller = move;
        _transform = move.transform;
        _controller.RigidBody.freezeRotation = true;
    }
    public void Close()
    {
        _controller.RigidBody.freezeRotation = false;
        _controller.Mesh.localRotation = Quaternion.identity;
        _controller.RigidBody.AddForce(-_gravityDirection * 10, ForceMode.Impulse);
    }

    public void HandleInput(InputValue input)
    {
        _input = input.Get<Vector2>();
    }
    public void FixedUpdate(MovementController move)
    {
        ApplyGravity(move);
        RotateOverSurface();

        _movement = (_controller.Head.forward * _input.y + _controller.Head.right * _input.x);
        _movement = Vector3.ProjectOnPlane(_movement, _gravityDirection).normalized;

        _controller.RigidBody.MovePosition(_controller.RigidBody.position + _movement * 3f * Time.fixedDeltaTime);
        RotateToDirection();
    }

    private void ApplyGravity(MovementController move)
    {
        _gravityDirection = (move.Atraction.Position - _transform.position).normalized;
        _controller.RigidBody.AddForce(_gravityDirection * -move.Atraction.Gravity, ForceMode.Acceleration);

        Physics.Raycast(_transform.position, _gravityDirection, out _groundHit, 10f, _ground, _query);
    }
    private void RotateOverSurface()
    {
        var gravityRotation = Quaternion.FromToRotation(_transform.up, -_gravityDirection) * _transform.rotation;
        var surfaceRotation = Quaternion.FromToRotation(_transform.up, _groundHit.normal) * _transform.rotation;
        var finalRotation = Quaternion.Lerp(gravityRotation, surfaceRotation, 0.75f);

        _transform.rotation = Quaternion.Slerp(_transform.rotation, finalRotation, 50f * Time.fixedDeltaTime);
    }
    private void RotateToDirection()
    {
        if (_movement == Vector3.zero) return;

        Vector3 moveDir = Vector3.ProjectOnPlane(_movement, _controller.Mesh.up).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(moveDir, _controller.Mesh.up);
        _controller.Mesh.rotation = Quaternion.Slerp(_controller.Mesh.rotation, targetRotation, 10f * Time.fixedDeltaTime);
    }
}
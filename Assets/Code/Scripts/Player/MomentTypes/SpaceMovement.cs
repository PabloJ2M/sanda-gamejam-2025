using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class SpaceMovement : IMovementState
{
    public CinemachineCameraType CameraType => CinemachineCameraType.Orbit;

    private MovementController _controller;
    private ParticleSystem.EmissionModule _speedEffect;
    //private Vector3 _movement;
    //private Vector2 _input;

    public void Start(MovementController move)
    {
        _controller = move;
        _speedEffect = move.Head.GetComponentInChildren<ParticleSystem>().emission;
        _speedEffect.enabled = true;
        _speedEffect.rateOverTime = 0;
    }
    public void Close() { _speedEffect.enabled = false; }

    public void HandleInput(InputValue input)
    {
        //_input = input.Get<Vector2>();
    }
    public void FixedUpdate(MovementController move)
    {
        //_movement = (move.Head.forward * _input.y + move.Head.right * _input.x);
        float impulse = 10f * move.Impulse.GetImpulse();
        _speedEffect.rateOverTime = impulse;

        MoveToDirection(move, impulse);
        RotateToDirection();
    }

    private void MoveToDirection(MovementController move, float impulse)
    {
        var speed = move.Head.forward * impulse;
        speed -= move.RigidBody.linearVelocity;

        move.RigidBody.AddForce(speed, ForceMode.Acceleration);

        if (move.RigidBody.linearVelocity.magnitude < 10f) return;
        move.RigidBody.linearVelocity = move.RigidBody.linearVelocity.normalized * 10f;
    }
    private void RotateToDirection()
    {
        if (_controller.RigidBody.linearVelocity.normalized == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(_controller.RigidBody.linearVelocity.normalized);
        _controller.RigidBody.rotation = Quaternion.Slerp(_controller.RigidBody.rotation, targetRot, 5f * Time.fixedDeltaTime);
    }
}
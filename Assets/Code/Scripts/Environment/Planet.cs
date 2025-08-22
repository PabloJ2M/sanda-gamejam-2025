using UnityEngine;

public class Planet : MonoBehaviour, IGravityStrategy
{
    [SerializeField] private float _gravity;
    private Transform _transform;

    private const string _tag = "Player";

    public Vector3 Position => _transform.position;
    public float Gravity => -_gravity;

    private void Awake() => _transform = transform;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_tag)) return;
        if (!other.TryGetComponent(out MovementController movement)) return;
        movement.SwitchState(new PlanetMovement(), this);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(_tag)) return;
        if (!other.TryGetComponent(out MovementController movement)) return;
        movement.SwitchState(new SpaceMovement(), null);
    }
}
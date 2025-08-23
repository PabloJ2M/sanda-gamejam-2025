using UnityEngine;

public class ConstantMoveRect : MonoBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _speed;

    private RectTransform _transform;

    private void Awake() => _transform = transform as RectTransform;
    private void Update() => _transform.position += _speed * Time.deltaTime * (Vector3)_direction;
}
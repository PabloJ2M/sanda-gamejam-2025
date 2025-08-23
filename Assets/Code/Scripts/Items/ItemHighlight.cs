using UnityEngine;

public class ItemHighlight : MonoBehaviour
{
    [SerializeField] private SphereCollider _collider;

    private const string _defaultLayer = "Item";
    private const string _selectedLayer = "Selectable";
    private const string _tag = "Player";

    private void Awake() => _collider.radius = Inventory.GrabDistance - 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_tag)) return;
        gameObject.layer = LayerMask.NameToLayer(_selectedLayer);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(_tag)) return;
        gameObject.layer = LayerMask.NameToLayer(_defaultLayer);
    }
}
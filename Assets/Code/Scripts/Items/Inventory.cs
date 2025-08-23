using UnityEngine;
using Unity.Netcode;

public class Inventory : NetworkBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Transform _holdPoint;

    public static float GrabDistance = 5f;

    private Collider[] _hits = new Collider[1];
    private NetworkObject _object;
    private Transform _transform;
    private Item _carriedItem;

    private void Awake()
    {
        _transform = transform;
        _object = GetComponent<NetworkObject>();
    }

    public void OnInteract()
    {
        if (!IsOwner) return;

        if (!_carriedItem) Pickup();
        else Drop();
    }
    public void DeliverItem()
    {
        if (_carriedItem == null) return;

        _carriedItem.DeliverServer();
        _carriedItem = null;
    }

    private void Pickup()
    {
        Physics.OverlapSphereNonAlloc(_transform.position, GrabDistance, _hits, _mask, QueryTriggerInteraction.Ignore);
        if (!_hits[0].TryGetComponent(out Item item)) return;

        _carriedItem = item;
        item.PickUp(_object, _holdPoint);
    }
    private void Drop()
    {
        _carriedItem.Drop();
        _carriedItem = null;
    }
}
using UnityEngine;
using UnityEngine.Pool;
using Unity.Netcode;

public class Inventory : NetworkBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Transform _holdPoint;

    public static float GrabDistance = 5f;

    private NetworkObject _object;
    private Transform _transform;
    private Item _carriedItem;

    private void Awake()
    {
        _transform = transform;
        _object = GetComponent<NetworkObject>();
    }
    public override void OnNetworkPreDespawn()
    {
        base.OnNetworkPreDespawn();
        Drop();
    }

    public void OnInteract()
    {
        if (!IsOwner) return;

        if (!_carriedItem) Pickup();
        else Drop();
    }

    private void Pickup()
    {
        Collider[] hits = Physics.OverlapSphere(_transform.position, GrabDistance, _mask, QueryTriggerInteraction.Ignore);
        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent(out Item item)) continue;
            if (!item.PickUp(_object, _holdPoint)) continue;
            _carriedItem = item;
            return;
        }
    }
    private void Drop()
    {
        _carriedItem?.Drop();
        _carriedItem = null;
    }
}
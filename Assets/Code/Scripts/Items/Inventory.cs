using UnityEngine;
using UnityEngine.Pool;
using Unity.Netcode;

public class Inventory : NetworkBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Transform _holdPoint;

    public static readonly float GrabDistance = 5f;
    private bool HasItem => _item != null;
    
    private Transform _transform;
    private Item _item;

    private void Awake()
    {
        _transform = transform;
        //_object = GetComponent<NetworkObject>();
    }
    private void FixedUpdate()
    {
        if (!IsOwner || _item == null) return;
        _item.transform.position = _holdPoint.position;
    }

    public override void OnNetworkPreDespawn()
    {
        base.OnNetworkPreDespawn();
        Drop();
    }
    public void OnInteract()
    {
        if (!IsOwner) return;

        if (!HasItem) Pickup();
        else Drop();
    }

    private void Pickup()
    {
        Collider[] hits = Physics.OverlapSphere(_transform.position, GrabDistance, _mask, QueryTriggerInteraction.Ignore);

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent(out Item item)) continue;
            if (!item.TryPickedUp(OwnerClientId)) continue;

            if (!item.IsOwner) item.NetworkObject.ChangeOwnership(OwnerClientId);
            _item = item;
            return;
        }
    }
    private void Drop()
    {
        _item?.Release();
        _item = null;
    }
}
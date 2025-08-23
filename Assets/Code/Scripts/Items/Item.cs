using UnityEngine;
using Unity.Netcode;

public class Item : NetworkBehaviour
{
    [SerializeField] private NetworkObject _object;
    [SerializeField] private Rigidbody _rigidbody;
    
    private Transform _pendingParent, _trackPoint;
    private bool isPickedUp = false;

    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();
        if (_pendingParent) ParentObject();
    }

    public void PickUp(NetworkObject client, Transform track)
    {
        if (isPickedUp) return;
        isPickedUp = true;

        _trackPoint = track;
        _pendingParent = client.transform;
        _rigidbody.isKinematic = true;

        if (!HasAuthority) _object.ChangeOwnership(client.OwnerClientId);
        else ParentObject();
    }
    public void Drop()
    {
        _object.TrySetParent((Transform)null);
        _rigidbody.isKinematic = false;
        _trackPoint = null;
        isPickedUp = false;
    }

    private void Update()
    {
        if (!IsOwner || transform.parent == null) return;
        _object.transform.position = _trackPoint.position;
    }
    private void ParentObject()
    {
        _object.TrySetParent(_pendingParent);
        _pendingParent = null;
    }

    public void DeliverServer()
    {
        NetworkObjectPool.Instance.Release(_object, gameObject);
    }
}
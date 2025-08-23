using UnityEngine;
using Unity.Netcode;

public class Item : NetworkBehaviour
{
    [SerializeField] private NetworkObject _object;
    [SerializeField] private Rigidbody _rigidbody;
    
    private Transform _pendingParent, _trackPoint;
    private NetworkVariable<bool> _isPickedUp = new(false, default, NetworkVariableWritePermission.Owner);

    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();
        if (_pendingParent) ParentObject();
    }

    public bool PickUp(NetworkObject client, Transform track)
    {
        if (_isPickedUp.Value) return false;

        _trackPoint = track;
        _pendingParent = client.transform;
        _rigidbody.isKinematic = true;

        if (!HasAuthority) _object.ChangeOwnership(client.OwnerClientId);
        else ParentObject();
        return true;
    }
    public void Drop()
    {
        _isPickedUp.Value = false;
        _object.TrySetParent((Transform)null);
        _rigidbody.isKinematic = false;
        _trackPoint = null;
    }

    private void Update()
    {
        if (!IsOwner || transform.parent == null) return;
        _object.transform.position = _trackPoint.position;
    }
    private void FixedUpdate()
    {
        if (!IsOwner || transform.parent != null) return;
        if (_rigidbody.linearVelocity.magnitude <= 0.01f) return;
        _rigidbody.AddForce(-_rigidbody.linearVelocity * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void ParentObject()
    {
        _object.TrySetParent(_pendingParent);
        _isPickedUp.Value = true;
        _pendingParent = null;
    }

    public void DeliverServer()
    {
        NetworkObjectPool.Instance.Release(_object, gameObject);
    }
}
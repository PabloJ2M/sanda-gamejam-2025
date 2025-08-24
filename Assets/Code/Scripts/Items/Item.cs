using Unity.Netcode;

namespace UnityEngine.Pool
{
    public class Item : NetworkPooledObject
    {
        [SerializeField] private NetworkObject _object;
        [SerializeField] private Rigidbody _rigidbody;
    
        private NetworkVariable<bool> _isPickedUp = new(false, default, NetworkVariableWritePermission.Owner);
        private Transform _pendingParent, _trackPoint;

        public override void OnGainedOwnership()
        {
            base.OnGainedOwnership();
            if (_pendingParent) ParentObject();
        }
        private void ParentObject()
        {
            _object.TrySetParent(_pendingParent);
            _isPickedUp.Value = true;
            _pendingParent = null;
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
    }
}
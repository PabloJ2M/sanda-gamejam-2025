using Unity.Netcode;

namespace UnityEngine.Pool
{
    public class Item : NetworkPooledObject
    {
        [SerializeField] private Rigidbody _rigidbody;
    
        private NetworkVariable<ulong> _player = new(0, default, NetworkVariableWritePermission.Owner);
        private bool IsPicked => _player.Value != 0;

        protected override void OnOwnershipChanged(ulong previous, ulong current)
        {
            if (!IsOwner || !IsPicked) return;

            if (_player.Value != current) NetworkObject.ChangeOwnership(previous);
            else _player.Value = current;
        }

        public bool TryPickedUp(ulong clientId)
        {
            if (IsPicked) return false;
            if (IsOwner) _player.Value = clientId;
            return true;
        }
        public void Release()
        {
            if (IsOwner) _player.Value = 0;
        }

        private void FixedUpdate()
        {
            if (!IsOwner || IsPicked) return;
            if (_rigidbody.linearVelocity.magnitude <= 0.01f) return;
            _rigidbody.AddForce(-_rigidbody.linearVelocity * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
}
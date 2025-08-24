namespace UnityEngine.Pool
{
    public class NetworkSpawnerItems : NetworkObjectPool
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsSessionOwner) return;
            
            foreach (var item in _items)
                for (var i = 0; i < item.count; i++)
                    _pool[item.prefab].Get();
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsSessionOwner) return;

            foreach (var item in _items)
                _pool[item.prefab].Clear();
        }

        protected override void OnGet(NetworkPooledObject ntObj)
        {
            base.OnGet(ntObj);
            ntObj.transform.SetPositionAndRotation(Random.insideUnitSphere * 50, Random.rotation);
        }
    }
}
using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace UnityEngine.Pool
{
    [Serializable] public struct PrefabGroup
    {
        public NetworkPooledObject prefab;
        public int count;
    }

    public abstract class NetworkObjectPool : NetworkBehaviour
    {
        [SerializeField] protected PrefabGroup[] _items;
        protected Dictionary<NetworkPooledObject, ObjectPool<NetworkPooledObject>> _pool = new();

        protected virtual void Awake()
        {
            foreach (var item in _items)
                _pool[item.prefab] = new(() => OnCreate(item.prefab), OnGet, OnRelease, OnDestroyObject);
        }

        protected virtual NetworkPooledObject OnCreate(NetworkPooledObject prefab)
        {
            var instance = Instantiate(prefab);
            instance.PoolReference = _pool[prefab];
            instance.NetworkObject.Spawn();
            return instance;
        }
        protected virtual void OnGet(NetworkPooledObject ntObj) => ntObj.gameObject.SetActive(true);
        protected virtual void OnRelease(NetworkPooledObject ntObj) => ntObj.gameObject.SetActive(false);
        protected virtual void OnDestroyObject(NetworkPooledObject ntObj) => ntObj.NetworkObject.Despawn();
    }
}
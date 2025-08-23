using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkObjectPool : SingletonBasic<NetworkObjectPool>
{
    [Serializable] public class PoolConfig
    {
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private PoolConfig[] _poolConfigs;
    private Dictionary<string, Queue<NetworkObject>> _pools = new();

    public void Initialize()
    {
        foreach (var config in _poolConfigs)
            _pools[config.prefab.name] = new();
    }

    public NetworkObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation, null);
    }
    public NetworkObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!_pools.ContainsKey(prefab.name)) return null;

        NetworkObject netObj;
        
        if (_pools[prefab.name].Count == 0)
        {
            var obj = Instantiate(prefab);
            obj.name = prefab.name;
            netObj = obj.GetComponent<NetworkObject>();
            netObj.Spawn();
        }
        else
        {
            netObj = _pools[prefab.name].Dequeue();
        }

        netObj.transform.SetPositionAndRotation(position, rotation);
        netObj.transform.SetParent(parent);
        netObj.gameObject.SetActive(true);

        return netObj;
    }
    public void Release(NetworkObject netObj, GameObject prefab)
    {
        netObj.transform.SetParent(null);
        netObj.gameObject.SetActive(false);

        if (_pools.ContainsKey(prefab.name))
            _pools[prefab.name].Enqueue(netObj);
        else
            print($"{prefab.name} doesnt exist");
    }
}
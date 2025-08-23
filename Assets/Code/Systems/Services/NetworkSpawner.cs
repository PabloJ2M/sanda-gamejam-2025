using System;
using UnityEngine;
using Unity.Netcode;
using Random = UnityEngine.Random;

public class NetworkSpawner : NetworkBehaviour
{
    [Serializable] private struct Prefabs
    {
        public Transform parent;
        public GameObject[] prefabs;
    }

    [SerializeField] private int _numberOfItems = 50;

    [SerializeField] private Prefabs _planetPrefabs;
    [SerializeField] private Prefabs[] _itemPrefabs;

    private NetworkObjectPool _pool;
    private Transform _transform;
    private Bounds _bounds;

    private void Awake()
    {
        _transform = transform;
        _pool = NetworkObjectPool.Instance;
        _bounds = GetComponent<Collider>().bounds;
        _pool.Initialize();
    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner && !HasAuthority) return;
        SpawnPlanets();
        SpawnItems();
    }

    private void SpawnPlanets()
    {
        foreach (var prefab in _planetPrefabs.prefabs)
            _pool.Instantiate(prefab, GetRandomPlanetPosition(), Random.rotation);
    }
    private void SpawnItems()
    {
        for (int i = 0; i < _numberOfItems; i++)
        {
            Prefabs instances = _itemPrefabs[Random.Range(0, _itemPrefabs.Length)];
            var prefab = instances.prefabs[Random.Range(0, instances.prefabs.Length)];
            _pool.Instantiate(prefab, GetRandomItemPosition(), Random.rotation);
        }
    }

    private Vector3 GetRandomPlanetPosition()
    {
        return _transform.position + new Vector3(
            Random.Range(-_bounds.size.x, _bounds.size.x),
            Random.Range(-_bounds.size.y, _bounds.size.y),
            Random.Range(-_bounds.size.z, _bounds.size.z)
        );
    }
    private Vector3 GetRandomItemPosition()
    {
        return _transform.position + new Vector3(
            Random.Range(-_bounds.size.x * 0.5f, _bounds.size.x * 0.5f),
            Random.Range(-_bounds.size.y * 0.5f, _bounds.size.y * 0.5f),
            Random.Range(-_bounds.size.z * 0.5f, _bounds.size.z * 0.5f)
        );
    }
}
using UnityEngine;
using Unity.Netcode;
using Random = UnityEngine.Random;

public class NetworkSpawner : NetworkBehaviour
{
    [SerializeField] private int _numberOfItems = 50;

    [SerializeField] private GameObject[] _planetPrefabs;
    [SerializeField] private GameObject[] _itemPrefabs;
    [SerializeField] private GizmosAreaDrawer _gizmos;

    private NetworkObjectPool _pool;
    private Transform _transform;
    private Collider _collider;
    private Bounds _bounds;

    private void Awake()
    {
        _transform = transform;
        _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }
    private void Start()
    {
        _pool = NetworkObjectPool.Instance;
        _pool.Initialize();
    }
    private void OnDrawGizmos()
    {
        if (!_collider) Awake();
        _gizmos.DrawArea(transform, _collider);
    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner && !HasAuthority) return;
        //SpawnPlanets();
        SpawnItems();
    }

    //private void SpawnPlanets()
    //{
    //    foreach (var prefab in _planetPrefabs)
    //        _pool.Instantiate(prefab, GetRandomPlanetPosition(), Random.rotation);
    //}
    private void SpawnItems()
    {
        for (int i = 0; i < _numberOfItems; i++)
        {
            var prefab = _itemPrefabs[Random.Range(0, _itemPrefabs.Length)];
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
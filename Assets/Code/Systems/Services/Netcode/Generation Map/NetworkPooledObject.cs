using Unity.Netcode;

namespace UnityEngine.Pool
{
    public abstract class NetworkPooledObject : NetworkBehaviour
    {
        public IObjectPool<NetworkPooledObject> PoolReference { get; set; }
    }
}
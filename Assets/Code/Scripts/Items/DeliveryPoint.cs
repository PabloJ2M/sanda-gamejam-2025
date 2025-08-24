using Unity.Collections;
using Unity.Netcode;

namespace UnityEngine.Pool
{
    public class DeliveryPoint : MonoBehaviour
    {
        private const string _tag = "Item";

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_tag)) return;
            if (!other.TryGetComponent(out NetworkPooledObject obj)) return;
            if (!obj.IsOwner) return;

            //var puntos = 10;
            //var clientId = NetworkManager.Singleton.LocalClientId;

            //var writer = new FastBufferWriter(sizeof(int) + sizeof(ulong), Allocator.Temp);
            //writer.WriteValueSafe(puntos);
            //writer.WriteValueSafe(clientId);

            //NetworkManager.Singleton.CustomMessagingManager.SendNamedMessageToAll(GameManager.Score, writer);
            print($"gived 10 points to {NetworkManager.Singleton.LocalClientId}");
            obj.PoolReference.Release(obj);
        }
    }
}
using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    private const string _tag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_tag)) return;
        if (!other.TryGetComponent(out Inventory inventory)) return;
        
        inventory.DeliverItem();
    }
}
using UnityEngine.Events;

namespace UnityEngine.Extra
{
    public abstract class InverseBase<T> : MonoBehaviour
    {
        [SerializeField] protected UnityEvent<T> _onValueChange;

        public abstract void SetInverseValue(T value);
    }
}
namespace UnityEngine.Extra
{
    public class InverseBool : InverseBase<bool>
    {
        public override void SetInverseValue(bool value) => _onValueChange.Invoke(!value);
    }
}
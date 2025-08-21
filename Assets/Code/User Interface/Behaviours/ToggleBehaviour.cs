namespace UnityEngine.UI
{
    public abstract class ToggleBehaviour : Toggle
    {
        protected override void OnEnable() { base.OnEnable(); onValueChanged.AddListener(OnUpdateValue); }
        protected override void OnDisable() { base.OnDisable(); onValueChanged.RemoveListener(OnUpdateValue); }

        protected override void OnValidate()
        {
            base.OnValidate();
            toggleTransition = ToggleTransition.None;
            graphic = null;
        }

        public abstract void OnUpdateValue(bool isOn);
    }
}
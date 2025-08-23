namespace UnityEngine.UI
{
    public class ToggleColor : ToggleBehaviour
    {
        [SerializeField] private Color _highlightColor = Color.white;
        private Color _defaultColor = Color.white;

        protected override void Awake()
        {
            base.Awake();
            _defaultColor = image.color;
        }
        protected override void Reset()
        {
            base.Reset();
            _defaultColor = image.color;
        }
        public override void OnUpdateValue(bool isOn)
        {
            if (!Application.isPlaying) return;
            _defaultColor.a = _highlightColor.a = 1f;
            image.color = isOn ? _defaultColor * _highlightColor : _defaultColor;
        }
    }
}
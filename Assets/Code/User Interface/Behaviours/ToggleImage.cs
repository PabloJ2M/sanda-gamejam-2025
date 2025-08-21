namespace UnityEngine.UI
{
    public class ToggleImage : ToggleBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;

        public override void OnUpdateValue(bool isOn)
        {
            if (!_icon || !_onSprite || !_offSprite) return;
            _icon.sprite = isOn ? _onSprite : _offSprite;
        }
    }
}
using UnityEngine.Animations;

namespace UnityEngine.UI
{
    public class ToggleSwipe : ToggleBehaviour
    {
        private TweenSwipe _animation;

        protected override void Awake()
        {
            base.Awake();
            _animation = GetComponentInChildren<TweenSwipe>();
        }
        public override void OnUpdateValue(bool isOn)
        {
            if (!Application.isPlaying) return;
            _animation.SetStatus(!isOn);
        }
    }
}
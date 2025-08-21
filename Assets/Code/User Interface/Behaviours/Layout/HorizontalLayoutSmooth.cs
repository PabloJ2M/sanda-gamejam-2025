namespace UnityEngine.UI.Effects
{
    public class HorizontalLayoutSmooth : HOVLayoutSmooth
    {
        protected override bool _isVertical => false;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            if (_to.Length != rectChildren.Count) CachePositions();

            SaveCurrentPositions();
            SetChildrenAlongAxis(0, _isVertical);
            SetChildrenAlongAxis(1, _isVertical);

            if (_isFirstTime) { SaveCurrentPositions(); _isFirstTime = false; }

            for (int i = 0; i < rectChildren.Count; i++)
            {
                _to[i] = rectChildren[i].anchoredPosition;
                rectChildren[i].anchoredPosition = _from[i];
            }
        }
    }
}
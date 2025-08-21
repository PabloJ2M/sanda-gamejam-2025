namespace UnityEngine.UI.Effects
{
    public class VerticalLayoutSmooth : HOVLayoutSmooth
    {
        protected override bool _isVertical => true;

        public override void CalculateLayoutInputVertical()
        {
            base.CalculateLayoutInputVertical();
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
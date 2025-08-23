using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RectOutOfFrame : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float _threshold;
    [SerializeField] private UnityEvent _onRectOutOfFrame;

    private RectTransform _rectTransform;
    private RectTransform _rectArea;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectArea = _rectTransform.parent as RectTransform;
    }
    private IEnumerator Start()
    {
        yield return new WaitWhile(IsVisibleOnScreen);
        _onRectOutOfFrame.Invoke();
    }

    private bool IsVisibleOnScreen()
    {
        float height = _rectArea.rect.height + _rectTransform.sizeDelta.y - _threshold;
        return _rectTransform.anchoredPosition.y < height;
    }
}
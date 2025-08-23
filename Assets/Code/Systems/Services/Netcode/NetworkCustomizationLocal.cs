using UnityEngine;
using UnityEngine.UI;

public class NetworkCustomizationLocal : MonoBehaviour
{
    [SerializeField] private MeshRenderer _render;
    [SerializeField] private Toggle _default;
    private Material _material;

    private Toggle _selected;

    private void Awake() => _material = _render.material;
    private void Start() => _default.isOn = true;

    public void SetColorButton(Toggle toggle)
    {
        if (!toggle.isOn || _selected == toggle) return;

        string html = ColorUtility.ToHtmlStringRGB(toggle.image.color);
        PlayerPrefs.SetString(NetworkCustomization.PlayerColor, $"#{html}");

        _material.color = toggle.image.color;
        _selected = toggle;
    }
}
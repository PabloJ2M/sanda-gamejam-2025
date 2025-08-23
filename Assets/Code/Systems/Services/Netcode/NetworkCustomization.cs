using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class NetworkCustomization : NetworkBehaviour
{
    [SerializeField] private MeshRenderer _render;

    public static readonly string PlayerColor = "PlayerColor";
    private NetworkVariable<FixedString32Bytes> _playerColor = new("#FFFFFF", default, NetworkVariableWritePermission.Owner);
    private Material _material;

    private void Awake() => _material = _render.material;
    private void OnEnable() => _playerColor.OnValueChanged += OnColorChanged;
    private void OnDisable() => _playerColor.OnValueChanged -= OnColorChanged;

    protected override void OnNetworkPostSpawn()
    {
        base.OnNetworkPostSpawn();
        if (!IsOwner) { SetColor(_playerColor.Value.ToString()); return; }

        string html = PlayerPrefs.GetString(PlayerColor, "#FFFFFF");
        _playerColor.Value = html;
    }
    private void OnColorChanged(FixedString32Bytes previusValue, FixedString32Bytes newValue)
    {
        SetColor(newValue.ToString());
    }

    private void SetColor(string value)
    {
        if (ColorUtility.TryParseHtmlString(value, out Color color))
            _material.color = color;
    }
}
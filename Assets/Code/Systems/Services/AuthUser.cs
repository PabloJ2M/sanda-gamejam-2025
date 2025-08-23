using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using TMPro;

public class AuthUser : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        SetName(_input.text);
    }

    private void OnEnable() => _input.onEndEdit.AddListener(SetName);
    private void OnDisable() => _input.onEndEdit.RemoveListener(SetName);

    private async void SetName(string name)
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(name);
    }
}
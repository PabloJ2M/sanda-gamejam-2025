using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class NetworkPlayerSpawner : MonoBehaviour
{
    [SerializeField, Scene] private string _gameplay;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton != null);
        yield return new WaitUntil(() => NetworkManager.Singleton.SceneManager != null);
        NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
    }
    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) return;
        if (NetworkManager.Singleton.SceneManager == null) return;
        NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnSceneEvent;
    }

    private void OnSceneEvent(SceneEvent sceneEvent)
    {
        bool isGameplay = sceneEvent.SceneName == _gameplay || sceneEvent.ScenePath == _gameplay;
        if (!isGameplay || sceneEvent.SceneEventType != SceneEventType.LoadComplete) return;

        if (!NetworkManager.Singleton.LocalClient.IsSessionOwner) return;
        print("scene change detected");
        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            SpawnPlayer(NetworkManager.Singleton.LocalClientId);
    }
    private void SpawnPlayer(ulong clientId)
    {
        GameObject playerInstance = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}
using System.Collections.Generic;
using Unity.Netcode;

public class GameManager : SingletonBasic<GameManager>
{
    public static readonly string Score = "AddScore";

    private NetworkManager _manager;
    private Dictionary<ulong, int> _scores = new();

    protected override void Awake() { base.Awake(); _manager = NetworkManager.Singleton; }
    private void Start() => _manager.CustomMessagingManager.RegisterNamedMessageHandler(Score, OnMessageHandler);

    private void OnMessageHandler(ulong sender, FastBufferReader reader)
    {
        reader.ReadValueSafe(out int puntos);
        reader.ReadValueSafe(out ulong playerId);

        if (!_scores.ContainsKey(playerId)) _scores[playerId] = 0;
        _scores[playerId] += puntos;
    }
    public ulong GetWinner()
    {
        if (_scores.Count == 0) return 0;

        ulong ganador = 0;
        int max = int.MinValue;

        foreach (var kv in _scores)
        {
            if (kv.Value > max)
            {
                max = kv.Value;
                ganador = kv.Key;
            }
        }

        return ganador;
    }
}
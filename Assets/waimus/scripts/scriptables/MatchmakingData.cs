using UnityEngine;

[CreateAssetMenu(fileName = "MatchmakingData", menuName = "Matchmaking/MatchmakingData")]
public class MatchmakingData : ScriptableObject
{
    [Header("Matchmaking Data")]
    public PlayerMatchmakingData[] playersData;
    public int maxPlayers;
    public string gameplayScene;
    
    private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
}
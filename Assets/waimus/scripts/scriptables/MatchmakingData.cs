using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MatchmakingData", menuName = "Matchmaking/MatchmakingData")]
public class MatchmakingData : ScriptableObject
{
    [Header("Matchmaking Data")]
    public PlayerMatchmakingData[] playersData;
    public string gameplayScene;
    
    private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
}
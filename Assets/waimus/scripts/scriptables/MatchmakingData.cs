using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MatchmakingData", menuName = "Matchmaking/MatchmakingData")]
public class MatchmakingData : ScriptableObject
{
    [Header("Matchmaking Data")]
    public PlayerMatchmakingData[] playersData;
    
    private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
}
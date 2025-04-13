using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Matchmaking/PlayerData")]
public class PlayerMatchmakingData : ScriptableObject
{
     [Header("Player Data")]
     public int playerId;
     public int kartId;
     public PlayerInput inputData;
     public bool isReady = false;
     
     // Utility
     public void Initialize(int pid, int kid, PlayerInput input)
     {
         playerId = pid;
         kartId = kid;
         inputData = input;
     }
}
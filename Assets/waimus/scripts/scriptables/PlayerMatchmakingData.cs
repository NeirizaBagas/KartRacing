using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Matchmaking/PlayerData")] [System.Serializable]
public class PlayerMatchmakingData : ScriptableObject
{
     [Header("Player Data")]
     public int playerId;
     public int kartId;
     public PlayerInput inputData;
     public bool isReady = false;
     
     private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
     
     // Utility
     public void Initialize(int pid, int kid, PlayerInput input)
     {
         playerId = pid;
         kartId = kid;
         inputData = input;
     }

     public PlayerMatchmakingData CreateInstance()
     {
         var clone = ScriptableObject.Instantiate(this);
         clone.name = clone.name.Substring(0, clone.name.Length - 7);
         return clone;
     }

     public PlayerMatchmakingData CreateInstance(int pid, int kid, PlayerInput input)
     {
         var clone = ScriptableObject.Instantiate(this);
         clone.name = clone.name.Substring(0, clone.name.Length - 7);

         clone.playerId = pid;
         clone.kartId = kid;
         clone.inputData = input;
         
         return clone;
     }
}
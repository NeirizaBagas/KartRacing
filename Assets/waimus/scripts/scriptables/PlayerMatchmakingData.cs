using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Matchmaking/PlayerData")] [System.Serializable]
public class PlayerMatchmakingData : ScriptableObject
{
     [Header("Player Data")]
     public int playerId;
     public int kartId;
     public InputActionAsset inputActions;
     public string controlScheme;
     public InputDevice inputDevice;
     public bool isReady = false;
     
     private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
     
     // Utility
     public void Initialize(int pid, int kid, InputActionAsset inputActionsAsset, string scheme, InputDevice device)
     {
         playerId = pid;
         kartId = kid;
         inputActions = inputActionsAsset;
         controlScheme = scheme;
         inputDevice = device;
         
     }

     public PlayerMatchmakingData CreateInstance()
     {
         var clone = ScriptableObject.Instantiate(this);
         clone.name = clone.name.Substring(0, clone.name.Length - 7);
         return clone;
     }

     public PlayerMatchmakingData CreateInstance(int pid, int kid, InputActionAsset inputActionsAsset, string scheme, InputDevice device)
     {
         var clone = ScriptableObject.Instantiate(this);
         clone.name = clone.name.Substring(0, clone.name.Length - 7);

         clone.playerId = pid;
         clone.kartId = kid;
         clone.inputActions = inputActionsAsset;
         clone.controlScheme = scheme;
         clone.inputDevice = device;
         
         return clone;
     }
}
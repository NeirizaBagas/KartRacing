using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMatchmakingData : ScriptableObject
{
     public void Initalize(int pid, int kid, PlayerInput input)
     {
          playerId = pid;
          kartId = kid;
          inputData = input;
     }

     public int playerId;
     public int kartId;
     public PlayerInput inputData;
     public bool isReady = false;
}
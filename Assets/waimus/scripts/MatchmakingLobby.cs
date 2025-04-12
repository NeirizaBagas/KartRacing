using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchmakingLobby : MonoBehaviour
{
    public List<PlayerMatchmakingData> playerData = new();
    public Transform kartsLabelParent;
    public Transform kartsDisplay;
    private Transform[] kartsDisplayPivots;
    private Transform[] activeKartPivots;

    [SerializeField] private PlayerInputManager _inputManager;
    [SerializeField] private Camera _camera;

    public static MatchmakingLobby Instance { get; private set; }

    private void Awake()
    {
        Instance ??= this;

        _inputManager ??= GetComponent<PlayerInputManager>();
        _inputManager.playerJoinedEvent.AddListener(OnPlayerJoined);
        _inputManager.playerLeftEvent.AddListener(OnPlayerLeft);
    }

    private void Start()
    {
        // Hide kart display
        if (!kartsDisplay) return;
        kartsDisplayPivots = new Transform[kartsDisplay.childCount];
        for (int i = 0; i < kartsDisplay.childCount; i++)
        {
            kartsDisplayPivots[i] = kartsDisplay.GetChild(i).transform;
            kartsDisplay.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        Debug.Log($"Player {player.playerIndex} Joined match");
        // UpdateCameraPosition();
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        Debug.Log($"Player {player.playerIndex} left match");
        // UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // Get any active karts display
        activeKartPivots =  Array.FindAll(kartsDisplayPivots, (p) => p.gameObject.activeInHierarchy);
        if (activeKartPivots is { Length: <= 0}) return;

        // int first = 0;
        // int last = activeKartPivots.Length - 1;
        
        // Update camera position to center between first and last active karts
        Vector3[] posRange = new Vector3[2];
        posRange[0] = activeKartPivots.First().position;
        posRange[1] = activeKartPivots.Last().position;
        Vector3 mid = new Vector3(Vector3.Lerp(posRange[0], posRange[1], 0.5f).x, 0, 0);
        _camera.transform.parent.DOMove(mid, 0.5f).SetAutoKill(true);
    }

    public void SetActiveSelectedKart(int pid, int kid)
    {
        Transform pivot = kartsDisplay.GetChild(pid);
        for (int i = 0; i < pivot.childCount; i++)
        {
            // Show kart when index is the requested id, otherwise hide
            pivot.GetChild(i).gameObject.SetActive(i == kid);
        }
    }

    // public void LoadGameplayMap()
    // {
    //     GameManager.Instance.playerCount = playerData.Count;
    //     UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Multiplayer2");
    // }

    #region Utilities
    
    public static PlayerMatchmakingData GetPlayerMatchmakingData(int id)
    {
        return !Instance ? null : Instance.playerData[id];
    }
    
    public static PlayerMatchmakingData GetPlayerMatchmakingData(PlayerMatchmakingData data)
    {
        return !Instance ? null : Instance.playerData.Find((d) => d == data);
    }

    public static void RegisterPlayer(PlayerMatchmakingData data)
    {
        Instance.playerData.Add(data);
        
        // Show kart by player index
        if (!Instance.kartsDisplay) return;
        Instance.kartsDisplay.GetChild(data.playerId).gameObject.SetActive(true);
        Instance.UpdateCameraPosition();
    }

    public static void UnregisterPlayer(PlayerMatchmakingData data)
    {
        Instance.playerData.Remove(data);
        
        // Hide kart by player index
        if (!Instance.kartsDisplay) return;
        Instance.kartsDisplay.GetChild(data.playerId).gameObject.SetActive(false);
        Instance.UpdateCameraPosition();
    }

    public static void ValidatePlayer(int id, bool isReady)
    {
        Instance.playerData[id].isReady = isReady;
        
        if (isReady) Debug.Log($"Player {id} is ready", Instance);
        else Debug.Log($"Player {id} is invalidated", Instance);
        
        // If all players ready
        if (Instance.playerData.All(p => p.isReady != false))
        {
            Debug.Log("All players ready", Instance);
        }
    }
    
    #endregion
}
using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInputManager))]
public class MatchmakingLobby : MonoBehaviour
{
    [Header("Data")]
    // public PlayerMatchmakingData[] playersData;
    public MatchmakingData matchmakingData;
    
    [Header("References")]
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

        // Reserve list space with the size of max player count
        matchmakingData.playersData = new PlayerMatchmakingData[_inputManager.maxPlayerCount];
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
        
        SetEventSystemFocus(true);
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        // Don't join except on matchmaking page
        if (PageStateManager.Instance.GetCurrentPageState() != EPageStates.Matchmaking)
        {
            Destroy(player.gameObject);
        }
        
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
        return !Instance ? null : Instance.matchmakingData.playersData[id];
    }
    
    public static PlayerMatchmakingData GetPlayerMatchmakingData(PlayerMatchmakingData data)
    {
        return !Instance ? null : Array.Find(Instance.matchmakingData.playersData, (d) => d == data);
    }

    public static void RegisterPlayer(PlayerMatchmakingData data)
    {
        if (PageStateManager.Instance.GetCurrentPageState() != EPageStates.Matchmaking) return;
        
        // Assign to list by given ID managed by InputSystem
        if (Instance.matchmakingData.playersData[data.playerId] == null)
            Instance.matchmakingData.playersData[data.playerId] = data;
        
        // Show kart by player index
        if (!Instance.kartsDisplay) return;
        Instance.kartsDisplay?.GetChild(data.playerId).gameObject?.SetActive(true);
        Instance.UpdateCameraPosition();
    }
    
    public static void UnregisterPlayer(PlayerMatchmakingData data)
    {
        if (PageStateManager.Instance.GetCurrentPageState() != EPageStates.Matchmaking) return;
        
        Instance.matchmakingData.playersData[data.playerId] = null;
        
        // Hide kart by player index
        if (!Instance.kartsDisplay) return;
        Instance.kartsDisplay?.GetChild(data.playerId).gameObject?.SetActive(false);
        Instance.UpdateCameraPosition();
    }

    public static void ValidatePlayer(int id, bool isReady)
    {
        Instance.matchmakingData.playersData[id].isReady = isReady;
        
        // if (isReady) Debug.Log($"Player {id} is ready", Instance);
        // else Debug.Log($"Player {id} is invalidated", Instance);

        // If all players ready
        var players = Instance.matchmakingData.playersData.Where((d) => d != null);
        if (players.All(p => p.isReady != false))
        {
            Debug.Log("All players ready", Instance);
            // Instance.matchmakingData.playersData = Instance.matchmakingData.playersData;
        }
    }

    public static void SetEventSystemFocus(bool isFocus)
    {
        if (isFocus)
        {
            // Focus EventSystem UI navigation
            var focus = GameObject.Find("EventSystemFocus");
            EventSystem.current.SetSelectedGameObject(focus.transform.parent.gameObject);
        }
        else
        {
            // Unfocus EventSystem UI navigation
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public static void SetEventSystemFocus(bool isFocus, float delay)
    {
        if (isFocus)
        {
            // Delayed focus
            DOVirtual.Float(0f, 1f, delay, value => { }).OnComplete(() =>
            {
                // Focus EventSystem UI navigation
                var focus = GameObject.Find("EventSystemFocus");
                EventSystem.current.SetSelectedGameObject(focus.transform.parent.gameObject);                
            }).SetAutoKill(true);
        }
        else
        {
            // Unfocus EventSystem UI navigation
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    
    #endregion
}
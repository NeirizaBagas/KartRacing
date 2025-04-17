using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Manages matchmaking data which to be transferred to GamePlayerManager via MatchmakingData
/// </summary>
[RequireComponent(typeof(PlayerInputManager))]
public class MatchmakingLobby : MonoBehaviour
{
    [Header("Data")]
    public MatchmakingData matchmakingData;
    [SerializeField] private LevelData[] _levelsData;
    [SerializeField] private UnityEngine.UI.Button startMatchButton;
    private int _maxPlayerSize;
    private bool _isMatchReady = false;
    
    [Header("References")]
    public Transform kartsLabelParent;
    public Transform kartsDisplay;
    public Transform kartsDisplayBase;
    private Transform[] _kartsDisplayPivots;
    private Transform[] _kartsDisplayBase;
    private Transform[] _activeKartPivots;

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
        _kartsDisplayPivots = new Transform[kartsDisplay.childCount];
        for (int i = 0; i < kartsDisplay.childCount; i++)
        {
            _kartsDisplayPivots[i] = kartsDisplay.GetChild(i).transform;
            kartsDisplay.GetChild(i).gameObject.SetActive(false);
        }

        if (!kartsDisplayBase) return;
        _kartsDisplayBase = new Transform[kartsDisplayBase.childCount];
        for (int i = 0; i < kartsDisplayBase.childCount; i++)
        {
            _kartsDisplayBase[i] = kartsDisplayBase.GetChild(i).transform;
            kartsDisplayBase.GetChild(i).gameObject.SetActive(false);
        }
        
        _inputManager.DisableJoining();
        
        SetEventSystemFocus(true);
    }

    private void OnEnable()
    {
        // Centralize level data via this script once object references are setup
        foreach (var ld in _levelsData)
        {
            // Level select button
            ld.associatedSelectButton.onClick.AddListener(() =>
            {
                matchmakingData.gameplayScene = ld.sceneName;
                Debug.Log($"Map to load is set to: {matchmakingData.gameplayScene}");
            });
        }
        
        // Start game button
        startMatchButton.onClick.AddListener(() =>
        {
            if (_inputManager.playerCount < _maxPlayerSize)
            {
                Debug.Log("Not enough player joined yet!\nJoin new player or pick other mode.");
                return;
            }

            if (matchmakingData.gameplayScene == "null") return;
            UnityEngine.SceneManagement.SceneManager.LoadScene(matchmakingData.gameplayScene);
        });
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        // Don't join except on matchmaking page
        if (PageStateManager.Instance.GetCurrentPageState() != EPageStates.Matchmaking)
        {
            Destroy(player.gameObject);
        }
        
        // Debug.Log($"Player {player.playerIndex} Joined match");
        // UpdateCameraPosition();
        
        if (_inputManager.playerCount >= _maxPlayerSize) _inputManager.DisableJoining();
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        if (_inputManager.playerCount < _maxPlayerSize) _inputManager.EnableJoining();
        // Debug.Log($"Player {player.playerIndex} left match");
        // UpdateCameraPosition();
    }

    private void FocusCameraToKartDisplay()
    {
        // Get any active karts display
        _activeKartPivots =  Array.FindAll(_kartsDisplayPivots, (p) => p.gameObject.activeInHierarchy);
        if (_activeKartPivots is { Length: <= 0}) return;
        
        // Update camera position to center between first and last active karts
        Vector3[] posRange = new Vector3[2];
        posRange[0] = _activeKartPivots.First().position;
        posRange[1] = _activeKartPivots.Last().position;
        Vector3 mid = new Vector3(Vector3.Lerp(posRange[0], posRange[1], 0.5f).x, 0, 0);
        _camera.transform.parent.DOMove(mid, 0.5f).SetAutoKill(true);
    }
    
    private void FocusCameraToKartBase()
    {
        // Get any active karts display
        _activeKartPivots =  Array.FindAll(_kartsDisplayBase, (p) => p.gameObject.activeInHierarchy);
        if (_activeKartPivots is { Length: <= 0}) return;
        
        // Update camera position to center between first and last active karts
        Vector3[] posRange = new Vector3[2];
        posRange[0] = _activeKartPivots.First().position;
        posRange[1] = _activeKartPivots.Last().position;
        Vector3 mid = new Vector3(Vector3.Lerp(posRange[0], posRange[1], 0.5f).x, 0, 0);
        _camera.transform.parent.DOMove(mid, 0.5f).SetAutoKill(true);
    }

    public void SetActiveSelectedKart(int pid, int kid)
    {
        if (PageStateManager.Instance.GetCurrentPageState() != EPageStates.Matchmaking) return;
        
        Transform pivot = kartsDisplay.GetChild(pid);
        for (int i = 0; i < pivot.childCount; i++)
        {
            // Show kart when index is the requested id, otherwise hide
            pivot.GetChild(i).gameObject.SetActive(i == kid);
        }
    }

    public void CreateGame(int playerSize)
    {
        _maxPlayerSize = playerSize;
        // Reserve list space with the size of max player count
        matchmakingData.playersData = new PlayerMatchmakingData[_maxPlayerSize];
        Debug.Log($"Game is set to {playerSize}-player mode");

        GameObject[] existingPlayers;
        if ((existingPlayers = GameObject.FindGameObjectsWithTag("Player")) != null)
            foreach (var p in existingPlayers)
                p.GetComponent<PlayerMatchmakingInstance>().DisconnectPlayer();
        
        // Reset car display
        for (int i = 0; i < kartsDisplay.childCount; i++)
        {
            _kartsDisplayPivots[i] = kartsDisplay.GetChild(i).transform;
            kartsDisplay.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < kartsDisplayBase.childCount; i++)
        {
            _kartsDisplayBase[i] = kartsDisplayBase.GetChild(i).transform;
            kartsDisplayBase.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < playerSize; i++)
            kartsDisplayBase.GetChild(i).gameObject.SetActive(true);
        FocusCameraToKartBase();
    }
    
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
        // Instance.FocusCameraToKartDisplay();
    }
    
    public static void UnregisterPlayer(PlayerMatchmakingData data)
    {
        if (PageStateManager.Instance.GetCurrentPageState() != EPageStates.Matchmaking) return;
        
        Instance.matchmakingData.playersData[data.playerId] = null;
        
        // Hide kart by player index
        if (!Instance.kartsDisplay) return;
        Instance.kartsDisplay?.GetChild(data.playerId).gameObject?.SetActive(false);
        // Instance.FocusCameraToKartDisplay();
    }

    public static void ValidatePlayer(int id, bool isReady)
    {
        Instance.matchmakingData.playersData[id].isReady = isReady;

        // If all players ready
        var players = Instance.matchmakingData.playersData.Where((d) => d != null).ToArray();
        Instance._isMatchReady = players.All(p => p.isReady != false);
        
        if (Instance._isMatchReady)
        {
            Debug.Log("All players ready", Instance);

            // Submit data to GameManager
            GameManager.Instance.playerCount = players.Length;
            GameManager.Instance.ID = new int[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                GameManager.Instance.ID[i] = Instance.matchmakingData.playersData[i].playerId;
            }
        }
    }
    
#region Utilities

    public static void SetEventSystemFocus(bool isFocus)
    {
        if (!EventSystem.current) return;
        
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
        if (!EventSystem.current) return;
        
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
#region Level Utility Class
    [Serializable]
    public class LevelData
    {
        public string sceneName;
        public UnityEngine.UI.Button associatedSelectButton;
    }
#endregion
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides public method to be used in UI
/// </summary>
public class MatchMakingLobbyPageController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string _mainMenuName = "MainMenu";
    [SerializeField] private LevelData[] _levelsData;
    [SerializeField] private MenuPageCanvas[] _pages;

    [Header("References")] 
    [SerializeField] private MatchmakingLobby _matchmakingLobby;
    [SerializeField] private PlayerInputManager _inputManager;

    private void Start()
    {
        // Page setup/binding
        MenuPageCanvas p;
        p = System.Array.Find(_pages, (p) => p.Is(EPageStates.PlayerModeSelect));
        p.buttonPreviousPage.onClick.AddListener(() => LoadScene(_mainMenuName));
        p.buttonNextPage.onClick.AddListener(ConfirmPlayerMode);
        
        p = System.Array.Find(_pages, (p) => p.Is(EPageStates.Matchmaking));
        p.buttonPreviousPage.onClick.AddListener(ReturnToPlayerMode);
        p.buttonNextPage.onClick.AddListener(ConfirmMatchmakingData);
        
        p = System.Array.Find(_pages, (p) => p.Is(EPageStates.MapSelect));
        p.buttonPreviousPage.onClick.AddListener(ReturnToMatchmakingData);
        p.buttonNextPage.onClick.AddListener(StartMatch);
        
        // Centralize level data via this script once object references are setup
        Array.ForEach(_levelsData, (ld) =>
        {
            // Level select button
            ld.associatedSelectButton.onClick.AddListener(() =>
            {
                _matchmakingLobby.matchmakingData.gameplayScene = ld.sceneName;
                Debug.Log($"Map to load is set to: {_matchmakingLobby.matchmakingData.gameplayScene}");
            });            
        });
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ConfirmPlayerMode()
    {
        if (!_matchmakingLobby.isGameCreated)
        {
            Debug.Log("Game mode has not been set");
            return;
        }
        
        PageStateManager.Instance.ChangeState(EPageStates.Matchmaking);
        _inputManager.EnableJoining();
    }

    public void ReturnToPlayerMode()
    {
        PageStateManager.Instance.ChangeState(EPageStates.PlayerModeSelect);
        _matchmakingLobby.ResetGame();
    }

    public void ConfirmMatchmakingData()
    {
        if (!_matchmakingLobby.isMatchReady)
        {
            Debug.Log("Not all players have ready");
            return;
        }
        
        PageStateManager.Instance.ChangeState(EPageStates.MapSelect);
    }

    public void ReturnToMatchmakingData()
    {
        PageStateManager.Instance.ChangeState(EPageStates.Matchmaking);
    }

    public void StartMatch()
    {
        if (_inputManager.playerCount < _matchmakingLobby.matchmakingData.maxPlayers)
        {
            Debug.Log("Not enough player joined yet!\nJoin new player or pick other mode.");
            return;
        }

        if (_matchmakingLobby.matchmakingData.gameplayScene.ToLower() == "null") return;
            LoadScene(_matchmakingLobby.matchmakingData.gameplayScene);
    }
    
#region Utility Classes
    [System.Serializable]
    public class MenuPageCanvas
    {
        public EPageStates pageState;
        public GameObject pageRoot;
        public UnityEngine.UI.Button buttonPreviousPage;
        public UnityEngine.UI.Button buttonNextPage;
        
        public bool Is(EPageStates state) => pageState == state;   
    }

    [System.Serializable]
    public class LevelData
    {
        public string sceneName;
        public UnityEngine.UI.Button associatedSelectButton;
    }
#endregion
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
///  A class of player instance during matchmaking as a UI element. 
///  Matchmaking is the process of gathering player data of which later to be sent to the actual game session.
///  Data includes input device, kart id, etc.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerMatchmakingInstance : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private Color _defaultColor = Color.yellow;
    [SerializeField] private Color _readyColor = Color.green;
    public PlayerMatchmakingData blueprint;
    
    [Header("References")]
    [SerializeField] private RectTransform _playerIndicator;
    [SerializeField] private Transform _kartLabelsParent;
    private RectTransform[] _kartLabels;
    private PlayerInput _input;

    private int _selectionId = 0;
    
    private PlayerMatchmakingData _dataRef; // Only for initialization & reference
    private InputActionAsset _actionAsset;
    private InputActionMap _inputMap;

    [Header("UI References")] 
    [SerializeField] private TextMeshProUGUI _playerLabel;
    [SerializeField] private Image _playerPanel;

    private void Awake()
    {
        if (!MatchmakingLobby.Instance) return;
        _kartLabelsParent = MatchmakingLobby.Instance.kartsLabelParent;
        
        // Assign labels of which to be used for player instance position indicator
        _kartLabels = new RectTransform[_kartLabelsParent.childCount];
        for (int i = 0; i < _kartLabelsParent.childCount; i++) 
            _kartLabels[i] = _kartLabelsParent.GetChild(i) as RectTransform;
        
        // Input references setup
        _input = GetComponent<PlayerInput>();
        _actionAsset = _input.actions;
        _inputMap = _actionAsset.FindActionMap("UI");
        
        // Save data as PlayerMatchmakingData
        _dataRef = blueprint.CreateInstance(_input.playerIndex,_selectionId, _input.actions, _input.currentControlScheme, _input.devices[0]);
        MatchmakingLobby.RegisterPlayer(_dataRef);
        
        // Indicator label by player ID which given from InputSystem
        if (_playerLabel) _playerLabel.text = $"P{_dataRef.playerId + 1}";
    }

    private void OnEnable()
    {
        // Input action event subscription
        _inputMap.FindAction("Navigate").performed += (ctx) => CycleThroughSelection((int)ctx.ReadValue<Vector2>().x);
        _inputMap.FindAction("Submit").performed += (ctx) => ToggleValidatePlayer();
        _inputMap.FindAction("Join").performed += (ctx) => { DisconnectPlayer(); }; // Disconnect
        
        if (_dataRef.playerId == 0) MatchmakingLobby.SetEventSystemFocus(false);
    }

    private void OnDestroy()
    {
        if (!MatchmakingLobby.Instance) return;
        
        // Called when player left match/destroyed
        var data = MatchmakingLobby.GetPlayerMatchmakingData(_dataRef); // Get actual data from list
        MatchmakingLobby.UnregisterPlayer(data);
        // if (data.playerId == 0) MatchmakingLobby.SetEventSystemFocus(true);
    }

    private void Start()
    {
        _playerIndicator.transform.SetParent(_kartLabels[_selectionId], false);
        MatchmakingLobby.Instance.SetActiveSelectedKart(_dataRef.playerId, _selectionId);
    }

    public void DisconnectPlayer()
    {
        Destroy(_playerIndicator.gameObject); 
        Destroy(gameObject);
    }

    private void CycleThroughSelection(int direction)
    {
        if (PageStateManager.Instance.GetCurrentPageState() != EPageStates.Matchmaking) return;
        
        // Disallow changes when player is ready
        if (MatchmakingLobby.GetPlayerMatchmakingData(_dataRef).isReady) return;
        
        int step = _selectionId + direction;
        _selectionId = step < 0 ? _kartLabelsParent.childCount - 1 : step >= _kartLabelsParent.childCount ? 0 : step; // wrap step within range of 0 to label indices.
        
        _playerIndicator.transform.SetParent(_kartLabels[_selectionId], false);

        // Save to PlayerMatchmakingData
        if (!MatchmakingLobby.Instance) return;
        MatchmakingLobby.GetPlayerMatchmakingData(_dataRef).kartId = _selectionId;
        MatchmakingLobby.Instance.SetActiveSelectedKart(_dataRef.playerId, _selectionId);
    }

    [ContextMenu("Toggle Player Validation")]
    private void ToggleValidatePlayer()
    {
        if (PageStateManager.Instance.GetCurrentPageState() != EPageStates.Matchmaking) return;
        if (!MatchmakingLobby.Instance) return;
        
        var data = MatchmakingLobby.GetPlayerMatchmakingData(_dataRef);
        MatchmakingLobby.ValidatePlayer(data.playerId, !data.isReady);

        if (_playerPanel && data.isReady)
        {
            _playerPanel.color = _readyColor;
            // if (data.playerId == 0) MatchmakingLobby.SetEventSystemFocus(true, 1f);
        }
        else
        {
            _playerPanel.color = _defaultColor;
            // if (data.playerId == 0) MatchmakingLobby.SetEventSystemFocus(false);
        }
    }
}

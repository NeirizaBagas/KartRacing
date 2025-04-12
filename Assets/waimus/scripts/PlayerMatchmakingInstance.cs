using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
///  A class of player instance during matchmaking as a UI element. 
///  Matchmaking is the process of gathering player data of which later to be sent to the actual game session.
///  Data includes input device, kart id, etc.
/// </summary>
public class PlayerMatchmakingInstance : MonoBehaviour
{
    [SerializeField] private RectTransform _playerIndicator;
    [SerializeField] private Transform _kartLabelsParent;
    private RectTransform[] _kartLabels;
    private PlayerInput _input;

    private int _selectionId = 0;
    
    private PlayerMatchmakingData _dataRef; // Only for initialization & reference
    private InputActionAsset _actionAsset;
    private InputActionMap _inputMap;

    private void Awake()
    {
        if (!MatchmakingLobby.Instance) return;
        _kartLabelsParent = MatchmakingLobby.Instance.kartsLabelParent;
        
        // Assign labels of which to be used for player instance position indicator
        _kartLabels = new RectTransform[_kartLabelsParent.childCount];
        for (int i = 0; i < _kartLabelsParent.childCount; i++) 
            _kartLabels[i] = _kartLabelsParent.GetChild(i) as RectTransform;
        
        _input = GetComponent<PlayerInput>();
        _actionAsset = _input.actions;
        _inputMap = _actionAsset.FindActionMap("UI");
        
        // Save data as PlayerMatchmakingData
        _dataRef = ScriptableObject.CreateInstance($"PlayerMatchmakingData") as PlayerMatchmakingData;
        _dataRef.Initalize(_input.playerIndex,_selectionId, _input);
        MatchmakingLobby.RegisterPlayer(_dataRef);
        
        // Indicator label
        TextMeshProUGUI label;
        if ((label = GetComponentInChildren<TextMeshProUGUI>()) != null)
            label.text = $"P{_dataRef.playerId + 1}";
    }

    private void OnEnable()
    {
        // Input action event subscription
        _inputMap.FindAction("Navigate").performed += (ctx) => CycleThroughSelection((int)ctx.ReadValue<Vector2>().x);
        _inputMap.FindAction("Submit").performed += (ctx) => ToggleValidatePlayer();
        _inputMap.FindAction("Join").performed += (ctx) => { Destroy(_playerIndicator.gameObject); Destroy(gameObject); }; // Disconnect
    }

    private void OnDestroy()
    {
        // Called when player left match/destroyed
        var data = MatchmakingLobby.GetPlayerMatchmakingData(_dataRef); // Get actual data from list
        MatchmakingLobby.UnregisterPlayer(data);
    }

    private void Start()
    {
        _playerIndicator.transform.SetParent(_kartLabels[_selectionId], false);
    }

    private void CycleThroughSelection(int direction)
    {
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
        if (!MatchmakingLobby.Instance) return;
        
        var data = MatchmakingLobby.GetPlayerMatchmakingData(_dataRef);
        MatchmakingLobby.ValidatePlayer(data.playerId, !data.isReady);
    }
}

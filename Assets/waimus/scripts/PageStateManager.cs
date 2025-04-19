using UnityEngine;

[System.Serializable]
public enum EPageStates
{
    PlayerModeSelect = 0, Matchmaking = 1, MapSelect = 2
};

public class PageStateManager : MonoBehaviour
{
    public GameObject pagePlayerSizeSelect;
    public GameObject pageMatchmaking;
    public GameObject pageMapSelect;
    
    [SerializeField] private EPageStates _currentPageState = EPageStates.Matchmaking;

    public static PageStateManager Instance { get; private set; }

    private void Awake() => Instance ??= this;

    private void Start() => ChangeState(EPageStates.PlayerModeSelect);

    public void ChangeState(EPageStates state)
    {
        _currentPageState = state;

        pagePlayerSizeSelect.SetActive(_currentPageState == EPageStates.PlayerModeSelect);
        pageMatchmaking.SetActive(_currentPageState == EPageStates.Matchmaking);
        pageMapSelect.SetActive(_currentPageState == EPageStates.MapSelect);
    }

    public void ChangeState(int state)
    {
        ChangeState((EPageStates)state);
    }
    
    public EPageStates GetCurrentPageState() => _currentPageState;
}

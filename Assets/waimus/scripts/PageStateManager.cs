using UnityEngine;

[System.Serializable]
public enum EPageStates
{
    Matchmaking = 0, MapSelect = 1
};

public class PageStateManager : MonoBehaviour
{
    public GameObject pageMatchmaking;
    public GameObject pageMapSelect;
    
    [SerializeField] private EPageStates currentPageState = EPageStates.Matchmaking;

    public static PageStateManager Instance { get; private set; }

    private void Awake() => Instance ??= this;

    private void Start() => ChangeState(EPageStates.Matchmaking);

    public void ChangeState(EPageStates state)
    {
        currentPageState = state;

        pageMatchmaking.SetActive(currentPageState == EPageStates.Matchmaking);
        pageMapSelect.SetActive(currentPageState == EPageStates.MapSelect);
    }

    public void ChangeState(int state)
    {
        currentPageState = (EPageStates) state;
        
        pageMatchmaking.SetActive(currentPageState == EPageStates.Matchmaking);
        pageMapSelect.SetActive(currentPageState == EPageStates.MapSelect);
    }
    
    public EPageStates GetCurrentPageState() => currentPageState;
}
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MatchmakingDataRef : MonoBehaviour
{
    [Header("Data")] 
    public MatchmakingData data;
    
    [Header("Spawn Data References")]
    public GameObject[] spawnPoints;

    private PlayerInputManager _inputManager;

    private void Awake()
    {
        spawnPoints ??= GameObject.FindGameObjectsWithTag("SpawnPoints");
        
        _inputManager ??= GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        
    }
}
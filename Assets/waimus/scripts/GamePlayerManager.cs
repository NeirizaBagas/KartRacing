using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

/// <summary>
/// Manages player spawning given matchmaking data
/// </summary>
[RequireComponent(typeof(PlayerInputManager))]
public class GamePlayerManager : MonoBehaviour
{
    [Header("Match Data")] 
    public MatchmakingData data;

    [Header("Spawn Data References")] 
    public GameObject playerPrefab;
    public Transform[] spawnPoints;

    private PlayerInputManager _inputManager;

    private void Awake()
    {
        // Retrieve spawn points from collections;
        var collection = GameObject.FindGameObjectWithTag("SpawnPointCollection");
        spawnPoints = new Transform[collection.transform.childCount];
        for (int i = 0; i < collection.transform.childCount; i++)
            spawnPoints[i] = collection.transform.GetChild(i);
        
        _inputManager ??= GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        _inputManager.playerPrefab = playerPrefab;
        
        // Join player from matchmaking data
        var validData = data.playersData.Where((d) => d != null).ToArray();
        for (int i = 0; i < validData.Length; i++)
        {
            var p = _inputManager.JoinPlayer(
                validData[i].playerId,
                i,
                validData[i].controlScheme,
                validData[i].inputDevice);
            
            p.actions = validData[i].inputActions;
            p.actions.devices = new[] { validData[i].inputDevice };
            p.transform.position = spawnPoints[i].position;
            p.GetComponent<KartInputProcessor>().InitializeInput();
        }
    }
}
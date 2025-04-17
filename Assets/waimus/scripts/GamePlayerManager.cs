using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
        _inputManager.onPlayerJoined += OnPlayerJoined;
        
        // Join player from matchmaking data
        var validData = data.playersData.Where((d) => d != null).ToArray();
        for (int i = 0; i < validData.Length; i++)
        {
            var p = _inputManager.JoinPlayer(
                validData[i].playerId,
                i,
                validData[i].controlScheme,
                validData[i].inputDevice);
        }
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        // Additional configuration to player during joined
        player.actions = data.playersData[player.playerIndex].inputActions;
        player.actions.devices = new[] { data.playersData[player.playerIndex].inputDevice };
        player.transform.position = spawnPoints[player.playerIndex].position;
        player.GetComponent<KartInputProcessor>().InitializeInput();
        player.GetComponent<KartMeshesManager>().selectedId = data.playersData[player.playerIndex].kartId;

        // Update split screen layout
        CameraSplitscreenManager.Instance.CreateCamera(player.transform);
    }
}
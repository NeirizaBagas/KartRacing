using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera[] camPlayer;
    [SerializeField] private GameObject[] playerGameObjects;

    [Header("Settings")]
    [SerializeField] private bool isConfigured = false;
    private const int MIN_PLAYERS = 2;
    private const int MAX_PLAYERS = 4;

    private void LateUpdate()
    {
        if (!isConfigured && GameManager.Instance != null)
        {
            ConfigureSplitScreen(GameManager.Instance.playerCount);
            isConfigured = true;
        }
    }

    private void ConfigureSplitScreen(int playerCount)
    {
        // Validasi input
        playerCount = Mathf.Clamp(playerCount, MIN_PLAYERS, MAX_PLAYERS);

        // Validasi arrays
        if (camPlayer.Length < playerCount || playerGameObjects.Length < playerCount)
        {
            Debug.LogError($"Not enough cameras or player objects configured for {playerCount} players!");
            return;
        }

        // Nonaktifkan semua player terlebih dahulu
        DeactivateAllPlayers();

        // Konfigurasi berdasarkan jumlah player
        switch (playerCount)
        {
            case 1:
                SetupSinglePlayer();
                break;
            case 2:
                SetupTwoPlayers();
                break;
            case 3:
                SetupThreePlayers();
                break;
            case 4:
                SetupFourPlayers();
                break;
        }
    }

    private void DeactivateAllPlayers()
    {
        foreach (var player in playerGameObjects)
        {
            if (player != null)
                player.SetActive(false);
        }
    }

    private void SetupSinglePlayer()
    {
        playerGameObjects[0].SetActive(true);
        camPlayer[0].rect = new Rect(0, 0, 1, 1);
    }

    private void SetupTwoPlayers()
    {
        for (int i = 0; i < 2; i++)
        {
            playerGameObjects[i].SetActive(true);
        }

        camPlayer[0].rect = new Rect(0, 0, 0.5f, 1);
        camPlayer[1].rect = new Rect(0.5f, 0, 0.5f, 1);
    }

    private void SetupThreePlayers()
    {
        for (int i = 0; i < 3; i++)
        {
            playerGameObjects[i].SetActive(true);
        }

        camPlayer[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        camPlayer[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        camPlayer[2].rect = new Rect(0.25f, 0, 0.5f, 0.5f);
    }

    private void SetupFourPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            playerGameObjects[i].SetActive(true);
        }

        camPlayer[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        camPlayer[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        camPlayer[2].rect = new Rect(0, 0, 0.5f, 0.5f);
        camPlayer[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
    }

    // Method untuk reset konfigurasi jika diperlukan
    public void ReconfigureSplitScreen()
    {
        isConfigured = false;
    }

    // Method untuk validasi di editor
    private void OnValidate()
    {
        if (camPlayer != null && playerGameObjects != null)
        {
            if (camPlayer.Length != playerGameObjects.Length)
            {
                Debug.LogWarning("Camera array and player objects array should have the same length!");
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int playerCount;
    public int totalPlayers;
    public GameObject leaderboardPanel;
    public TextMeshProUGUI leaderboardText;

    private List<string> leaderboardEntries = new List<string>();

    private void Start()
    {
        leaderboardPanel.SetActive(false);
        totalPlayers = GameObject.FindGameObjectsWithTag("Player").Length; // Hitung semua pemain di scene
        Debug.Log("Total player: " + totalPlayers);
    }

    public void FinishCon(string playerName) // Dipanggil saat pemain mencapai garis finis
    {
        playerCount++;
        leaderboardEntries.Add($"{playerCount}. {playerName}"); // Tambahkan nama pemain ke leaderboard
        Debug.Log($"{playerName} selesai di posisi {playerCount}");

        if (playerCount >= totalPlayers) // Jika semua pemain selesai, tampilkan leaderboard
        {
            ShowLeaderboard();
        }
    }

    private void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        leaderboardText.text = "Leaderboard:\n" + string.Join("\n", leaderboardEntries);
    }

    public void FinishLevel()
    {
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("MainMenu");
    }
}

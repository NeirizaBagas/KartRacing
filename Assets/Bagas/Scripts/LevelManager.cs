using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public int playerEntry;
    public int totalPlayers;
    public GameObject leaderboardPanel;
    public AudioSource mainTheme;
    public AudioSource endTheme;

    // Array untuk menyimpan text posisi
    public TextMeshProUGUI[] positionTexts; // Assign di inspector untuk 4 posisi
    public float delayBeforeShowingLeaderboard = 2f;

    private List<string> leaderboardEntries = new List<string>();

    private void Start()
    {
        leaderboardPanel.SetActive(false);
        totalPlayers = GameManager.Instance.playerCount;

        // Reset semua text posisi
        foreach (var posText in positionTexts)
        {
            posText.text = "";
        }

        Debug.Log($"Total players in race: {totalPlayers}");
    }

    public void FinishCon(string playerName)
    {
        playerEntry++;

        // Simpan nama player di posisi yang sesuai (array dimulai dari 0)
        if (playerEntry <= positionTexts.Length)
        {
            positionTexts[playerEntry - 1].text = playerName;
            Debug.Log($"{playerName} finished in position {playerEntry}");
        }

        if (playerEntry >= totalPlayers)
        {
            mainTheme.Stop();
            endTheme.Play();
            StartCoroutine(ShowLeaderboardWithDelay());
        }
    }

    private IEnumerator ShowLeaderboardWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeShowingLeaderboard);
        leaderboardPanel.SetActive(true);
    }

    public void FinishLevel()
    {
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("MainMenu");
    }
}
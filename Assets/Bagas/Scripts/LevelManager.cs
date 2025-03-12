using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int playerCount;
    public int totalPlayers;
    public GameObject leaderboardPanel;

    private void Start()
    {
        leaderboardPanel.SetActive(false);
        totalPlayers = GameObject.FindGameObjectsWithTag("Player").Length; // Nyari semua player yang ada di scene
        Debug.Log("Total player" + totalPlayers);
    }

    //public void LeaderBoardShow()
    //{
    //    leaderboardPanel.SetActive(true);
    //}

    public void FinishCon() //Finish Condition
    {
        if (playerCount >= totalPlayers )
        {
            print("Leaderboard");
            leaderboardPanel.SetActive(true);
        }
    }

    public void FinishLevel()
    {
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }
}

using System.Xml.Schema;
using UnityEngine;


public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    public GameObject cp;
    public GameObject checkPointHolder;

    public GameObject[] players;
    public Transform[] checkpointPos;
    public GameObject[] checkPointEachPlayer;

    private int totalPlayers;
    private int totalCheckPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    private void Start()
    {
        totalPlayers = players.Length;
        totalCheckPoints = checkPointHolder.transform.childCount;
        SetCheckPoints();
        SetPlayerPosition();
    }

    void SetCheckPoints()
    {
        checkpointPos = new Transform[totalCheckPoints];

        for (int i = 0; i < totalCheckPoints; i++)
        {
            checkpointPos[i] = checkPointHolder.transform.GetChild(i).transform;
        }

        checkPointEachPlayer = new GameObject[totalPlayers];

        for (int i = 0;i < totalPlayers;i++)
        {
            checkPointEachPlayer[i] = Instantiate(cp, checkpointPos[0].position, checkpointPos[0].rotation);
            checkPointEachPlayer[i].name = "CP " + i;
            checkPointEachPlayer[i].layer = 10 + i;
        }
    }

    void SetPlayerPosition()
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            players[i].GetComponentInChildren<LapManager>().playerPosition = i + 1; // Tambah 1 agar tidak mulai dari 0
            players[i].GetComponentInChildren<LapManager>().playerNumber = i;
        }
    }


    public void PlayerCollected(int playerNumber, int cpNumber)
    {
        // Pastikan cpNumber valid
        if (cpNumber >= checkpointPos.Length || playerNumber >= checkPointEachPlayer.Length)
        {
            Debug.LogWarning($"[PlayerCollected] Invalid checkpoint or player index: player {playerNumber}, cp {cpNumber}");
            return;
        }

        // Debugging posisi checkpoint sebelum diupdate
        Debug.Log($"[PlayerCollected] Player {playerNumber} checkpoint pindah ke {cpNumber} ({checkpointPos[cpNumber].position})");

        // Pindahkan checkpoint ke posisi selanjutnya
        checkPointEachPlayer[playerNumber].transform.position = checkpointPos[cpNumber].position;
        checkPointEachPlayer[playerNumber].transform.rotation = checkpointPos[cpNumber].rotation;
        checkPointEachPlayer[playerNumber].transform.localScale = checkpointPos[cpNumber].localScale;

        ComparePosition(playerNumber);
    }

    void ComparePosition(int playerNumber)
    {
        GameObject currentPlayer = players[playerNumber];
        int currentPlayerPos = currentPlayer.GetComponentInChildren<LapManager>().playerPosition;
        int currentPlayerCP = currentPlayer.GetComponentInChildren<LapManager>().cpCrossed;

        GameObject playerInFront = null;
        int playerInFrontCP = 0;
        int playerInFrontPos = 0;

        for (int i = 0; i < totalPlayers; i++)
        {
            if (players[i].GetComponentInChildren<LapManager>().playerPosition == currentPlayerPos - 1)
            {
                playerInFront = players[i];
                playerInFrontCP = playerInFront.GetComponentInChildren<LapManager>().cpCrossed;
                playerInFrontPos = playerInFront.GetComponentInChildren<LapManager>().playerPosition;
                break;
            }
        }

        if (playerInFront != null && currentPlayerCP > playerInFrontCP)
        {
            currentPlayer.GetComponentInChildren<LapManager>().playerPosition = currentPlayerPos - 1;
            playerInFront.GetComponentInChildren<LapManager>().playerPosition = playerInFrontPos + 1;

            Debug.Log("Player " + playerNumber + " Has overtaken Player " + playerInFront.GetComponentInChildren<LapManager>().playerNumber);

            // Update UI setelah perubahan posisi
            currentPlayer.GetComponentInChildren<LapManager>().UpdatePositionUI();
            playerInFront.GetComponentInChildren<LapManager>().UpdatePositionUI();
        }
    }




}

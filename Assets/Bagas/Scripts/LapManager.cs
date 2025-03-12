using TMPro;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    [Header("Lap System")]
    public int lapCounter;
    public int maxLap;
    public TextMeshProUGUI _lapCounter;
    public bool raceStarted = false;
    public bool lapFinished = true;
    public TextMeshProUGUI winCon;
    public TextMeshProUGUI loseCon;
    public bool raceFinished;

    [Header("Checkpoint System")]
    public int playerNumber;
    public int cpCrossed = 0;
    public int playerPosition;
    public TextMeshProUGUI currentPosition;

    [Header("Reference Script")]
    public LevelManager gameManager;
    public RaceManager raceManager;
    public KartController kartController;


    public void Start()
    {
        winCon.gameObject.SetActive(false);
        loseCon.gameObject.SetActive(false);
        UpdateLapCounterUI();
        UpdatePositionUI();

        Debug.Log($"Max Lap: {maxLap}, Lap Counter (Start): {lapCounter}");
    }

    public void UpdatePositionUI()
    {
        currentPosition.text = "Posisi " + playerPosition + "/" + raceManager.players.Length;
    }


    public void IncrementLap()
    {
        lapCounter++;
        Debug.Log($"Lap bertambah: {lapCounter}/{maxLap}");

        UpdateLapCounterUI();

        if (lapCounter > maxLap && !raceFinished)
        {
            raceFinished = true;
            gameManager.playerCount++; // Menambah jumlah player yang udah finish
            gameManager.FinishCon(); // Mengecek apakah semua player sudah finish
            kartController.canMove = false;

            if (playerPosition == 1)
            {
                winCon.gameObject.SetActive(true);
            }
            else
            {
                loseCon.gameObject.SetActive(true);
            }
            
            Debug.Log(gameObject.name + " finished the race!");
        }
    }


    private void UpdateLapCounterUI()
    {
        _lapCounter.text = $"Lap: {lapCounter}/{maxLap}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CP"))
        {
            print("CP Terdeteksi!");
            cpCrossed++;

            // Jika sudah melewati semua checkpoint, mulai dari awal
            if (cpCrossed >= raceManager.checkpointPos.Length)
            {
                cpCrossed = 0; // Reset checkpoint agar mulai dari awal
                //IncrementLap(); // Tambah lap karena semua checkpoint telah dilewati
            }

            // Panggil RaceManager untuk update posisi checkpoint
            raceManager.PlayerCollected(playerNumber, cpCrossed);
        }
    }


}

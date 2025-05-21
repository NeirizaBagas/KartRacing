using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LapManager : MonoBehaviour
{
    [Header("All Reference")]
    public TextMeshProUGUI winCon;
    public TextMeshProUGUI loseCon;
    public TextMeshProUGUI _lapCounter;
    public TextMeshProUGUI currentPosition;
    public LevelManager levelManager;
    public RaceManager raceManager;
    public KartMover kartMover;
    public GameObject gUI;
    public TextMeshProUGUI countdownText;
    public RawImage miniMap;


    [Header("Lap System")]
    public bool raceStarted = false;
    public bool lapFinished = true;
    public bool raceFinished;
    public int lapCounter;
    public int maxLap;
    public int countdownTime = 3;

    [Header("Checkpoint System")]
    public int playerNumber;
    public int cpCrossed = 0;
    public int playerPosition;

    [Header("Reference Script")]
    public bool isKartFound = false;


    private void Start()
    {
        winCon.gameObject.SetActive(false);
        loseCon.gameObject.SetActive(false);
        miniMap.gameObject.SetActive(true);
        UpdateLapCounterUI();
        UpdatePositionUI();
    }

    private void Update()
    {
        // Cek jika belum menemukan KartMover yang aktif
        if (!isKartFound)
        {
            FindActiveKartMover();
            Debug.Log("Mencari KartMover yang aktif: " + this.gameObject.name);
        }
    }

    private void FindActiveKartMover()
    {
        // Cari KartMover yang aktif dari sibling objects
        KartMover[] siblingKartMovers = transform.parent.GetComponentsInChildren<KartMover>();

        foreach (var kart in siblingKartMovers)
        {
            if (kart.gameObject.activeInHierarchy)
            {
                kartMover = kart;
                kartMover.canMove = false;
                Debug.Log("KartMover ditemukan: " + kartMover.gameObject.name + this.gameObject.name);
                isKartFound = true;
                StartCoroutine(CountdownToStart());
                break;
            }
        }
    }


    IEnumerator CountdownToStart()
    {

        if (kartMover != null)
        {
            kartMover.canMove = false;
        }

        countdownText.gameObject.SetActive(true);

        for (int i = countdownTime; i > 0; i--)
        {
            AudioManager.Instance.PlaySFX(0);
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        AudioManager.Instance.PlaySFX(1);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        //raceStarted = true;
        if (kartMover != null)
        {
            kartMover.canMove = true;
        }

        countdownText.gameObject.SetActive(false);
    }

    public void UpdatePositionUI()
    {
        currentPosition.text = "Posisi " + playerPosition + "/" + raceManager.players.Length;
    }


    public void IncrementLap()
    {
        AudioManager.Instance.PlaySFX(12);
        lapCounter++;
        Debug.Log($"Lap bertambah: {lapCounter}/{maxLap}");

        UpdateLapCounterUI();
        if (lapCounter == maxLap)
        {
            print("Last Lap");
        }

        if (lapCounter > maxLap && !raceFinished)
        {
            raceFinished = true;
            levelManager.FinishCon(gameObject.name);


            if (kartMover != null)
            {
                miniMap.gameObject.SetActive(false);
                kartMover.canMove = false;
            }

            if (playerPosition == 1)
                winCon.gameObject.SetActive(true);
            else
                loseCon.gameObject.SetActive(true);
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

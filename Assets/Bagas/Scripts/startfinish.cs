using UnityEngine;
using TMPro;

public class startfinish : MonoBehaviour
{
    public GameObject startLine;
    public GameObject finishLine;
    public GameObject checkPointLine;
    public int lapCounter;
    public int maxLap;
    public TextMeshProUGUI _lapCounter;

    private void Start()
    {
        startLine.SetActive(true);
        finishLine.SetActive(false);
        checkPointLine.SetActive(false);

        UpdateLapCounterUI(); // Menampilkan lap awal di UI
    }

    public void StartLineTriggered()
    {
        Debug.Log("Start line triggered!");
        startLine.SetActive(false);
        checkPointLine.SetActive(true);
    }

    public void CheckpointLineTriggered()
    {
        Debug.Log("Checkpoint line triggered!");
        finishLine.SetActive(true);
        checkPointLine.SetActive(false);
    }

    public void FinishLineTriggered()
    {
        Debug.Log("Finish line triggered!");
        lapCounter++;
        UpdateLapCounterUI(); // Perbarui UI setelah lap bertambah

        finishLine.SetActive(false);
        checkPointLine.SetActive(true);

        if (lapCounter >= maxLap)
        {
            Debug.Log("Race Finished!");
        }
    }

    private void UpdateLapCounterUI()
    {
        _lapCounter.text = $"Lap: {lapCounter}/{maxLap}";
    }
}

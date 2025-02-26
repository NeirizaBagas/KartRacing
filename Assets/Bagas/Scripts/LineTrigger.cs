using UnityEngine;

public class LineTrigger : MonoBehaviour
{
    public string lineType; // Tipe garis: "Start", "Checkpoint", atau "Finish"

    private void OnTriggerEnter(Collider other)
    {
        // Cari LapManager di objek yang bertabrakan
        LapManager lapManager = other.transform.root.GetComponentInChildren<LapManager>();

        if (lapManager == null)
        {
            Debug.LogError("LapManager tidak ditemukan!");
            return;
        }

        // Pengecekan Start
        if (lineType == "StartFinish" && !lapManager.hasStarted)
        {
            Debug.Log("Start dipicu!");
            lapManager.hasStarted = true; // Set hasStarted ke true
        }
        else if (lineType == "StartFinish" && lapManager.hasStarted && !lapManager.hasFinished)
        {
            Debug.Log("Finish dipicu!");
            lapManager.hasFinished = true; // Set hasFinished ke true
            lapManager.lapCounter++; // Tambah lap counter

            // Update UI lap counter
            if (lapManager._lapCounter != null)
            {
                lapManager._lapCounter.text = "Lap: " + lapManager.lapCounter + " / " + lapManager.maxLap;
            }
        }

        // Pengecekan Checkpoint
        if (lineType == "Checkpoint")
        {
            Debug.Log("Checkpoint dipicu!");
            lapManager.hasFinished = false; // Set hasFinished ke false
        }
    }
}
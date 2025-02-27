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
            lapManager.hasStarted = true; // Set hasStarted ke true
        }
        else if (lineType == "StartFinish" && lapManager.hasStarted && !lapManager.hasFinished)
        {
            lapManager.hasFinished = true; // Set hasFinished ke true
            lapManager.IncrementLap(); // Tambah lap counter
        }

        // Pengecekan Checkpoint
        if (lineType == "Checkpoint")
        {
            lapManager.hasFinished = false; // Set hasFinished ke false
        }
    }
}
using UnityEngine;

public class LineTrigger : MonoBehaviour
{
    public string lineType; // Tipe garis: "Start", "Checkpoint", atau "Finish"

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name + " memasuki " + lineType);

            // Cari LapManager di objek yang bertabrakan
            LapManager lapManager = other.GetComponent<LapManager>();

            if (lapManager == null)
            {
                Debug.Log(other.name + " tidak memiliki LapManager! Mencari di parent...");
                Debug.LogError("LapManager tidak ditemukan!");
                return;
            }

            // Pengecekan Start
            if (lineType == "StartFinish" && !lapManager.raceStarted)
            {
                lapManager.raceStarted = true; // Set hasStarted ke true
            }
            else if (lineType == "StartFinish" && lapManager.raceStarted && !lapManager.lapFinished)
            {
                lapManager.lapFinished = true; // Set hasFinished ke true
                lapManager.IncrementLap(); // Tambah lap counter
            }

            // Pengecekan Checkpoint
            if (lineType == "Checkpoint")
            {
                lapManager.lapFinished = false; // Set hasFinished ke false
            }
        }
    }
}
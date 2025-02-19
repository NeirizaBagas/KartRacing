using UnityEngine;

public class LineTrigger : MonoBehaviour
{
    public GameManager gameManager; // Assign GameManager di Inspector
    public string lineType; // "Start", "Checkpoint", atau "Finish"

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Kirimkan event ke GameManager berdasarkan lineType
            if (lineType == "Start")
            {
                gameManager.StartLineTriggered();
            }
            else if (lineType == "Checkpoint")
            {
                gameManager.CheckpointLineTriggered();
            }
            else if (lineType == "Finish")
            {
                gameManager.FinishLineTriggered();

                if (other.CompareTag("Player")) // Pastikan Player punya tag "Player"
                {
                    KartController kart = other.transform.root.GetComponentInChildren<KartController>();
                    if (kart != null)
                    {
                        kart.IncrementLap();
                    }
                }
            }
        }
    }
}

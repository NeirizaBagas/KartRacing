using UnityEngine;

public class LapManager : MonoBehaviour
{
    public int totalLaps = 3;
    //private int currentLap = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<KartController>().IncrementLap();
        }
    }
}

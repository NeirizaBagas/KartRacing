using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn player = other.transform.root.GetComponentInChildren<PlayerRespawn>();
            if (player != null)
            {
                player.SetRespawnPoint(transform.position);
                Debug.Log("Checkpoint Updated: " + transform.position);
            }
        }
    }
}

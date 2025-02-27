using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn player = other.transform.root.GetComponentInChildren<PlayerRespawn>();
            if (player != null)
            {
                player.Respawn();
            }
        }
    }
}

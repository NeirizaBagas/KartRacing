using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("respawn");
            PlayerRespawn player = other.transform.parent.GetComponentInChildren<PlayerRespawn>();
            if (player != null)
            {
                player.Respawn();
            }
        }
    }
}

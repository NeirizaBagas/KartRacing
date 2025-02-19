using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 respawnPoint;
    public Rigidbody rb;

    private void Start()
    {
        // Awalnya, posisi respawn = posisi awal player
        respawnPoint = transform.position;
        Debug.Log("Initial Respawn Point: " + respawnPoint);
    }

    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        Debug.Log("SetRespawnPoint Dipanggil: " + newRespawnPoint);
        respawnPoint = newRespawnPoint;
    }


    public void Respawn()
    {
        Debug.Log("Respawning at: " + respawnPoint);

        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Hentikan momentum
            rb.angularVelocity = Vector3.zero;
            rb.MovePosition(respawnPoint); // Gunakan Rigidbody agar physics ikut berubah
        }
        else
        {
            transform.position = respawnPoint; // Jika tidak pakai Rigidbody
        }
    }
}

// --- StabiloDecalSpawner.cs ---
using UnityEngine;

public class StabiloDecalSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject decalPrefab;
    public float spawnInterval = 0.5f; // spawn every 0.5s
    public float decalLifetime = 5f; // decal stays for 5 seconds

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnDecal();
            timer = 0f;
        }
    }

    void SpawnDecal()
    {
        if (decalPrefab == null) return;

        // Instantiate decal at current position
        GameObject decal = Instantiate(decalPrefab, transform.position, Quaternion.identity);

        // Align decal flat on ground
        decal.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // Optionally, destroy decal after lifetime
        Destroy(decal, decalLifetime);
    }
}
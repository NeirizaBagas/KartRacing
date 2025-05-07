using UnityEngine;

public class StaplerTrapSpawner : MonoBehaviour
{
    public GameObject staplerPrefab;
    public Transform[] staplerSpawnPoints; 
    public int countPerSpawn = 6;          
    public float spawnRadius = 1f;

    private void Start()
    {
        SpawnRandomStaplerWave();
    }
    public void SpawnRandomStaplerWave()
    {
        if (staplerSpawnPoints.Length == 0 || staplerPrefab == null) return;

        Transform chosenPoint = staplerSpawnPoints[Random.Range(0, staplerSpawnPoints.Length)];

        for (int i = 0; i < countPerSpawn; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0;

            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Vector3 spawnPos = chosenPoint.position + randomOffset;

            Instantiate(staplerPrefab, spawnPos, randomRotation);
        }
    }
}

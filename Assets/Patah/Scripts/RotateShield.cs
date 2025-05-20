using UnityEngine;

public class MultiOrbitAndFloatLocal : MonoBehaviour
{
    [Header("Targets")]
    public Transform[] orbitingObjects;
    public Transform centerPoint;

    [Header("Orbit Settings")]
    public float orbitRadius = 1f;
    public float orbitSpeed = 50f;

    [Header("Float Settings")]
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 2f;

    private float[] angles;
    private float[] floatOffsets;

    void Start()
    {
        angles = new float[orbitingObjects.Length];
        floatOffsets = new float[orbitingObjects.Length];

        float angleStep = 360f / orbitingObjects.Length;

        for (int i = 0; i < orbitingObjects.Length; i++)
        {
            angles[i] = angleStep * i;
            floatOffsets[i] = Random.Range(0f, 2f * Mathf.PI); 
        }
    }

    void Update()
    {
        for (int i = 0; i < orbitingObjects.Length; i++)
        {
            if (orbitingObjects[i] == null || centerPoint == null) continue;


            angles[i] += orbitSpeed * Time.deltaTime;
            float radians = angles[i] * Mathf.Deg2Rad;

            Vector3 localOffset = new Vector3(
                Mathf.Cos(radians) * orbitRadius,
                Mathf.Sin(Time.time * floatFrequency + floatOffsets[i]) * floatAmplitude,
                Mathf.Sin(radians) * orbitRadius
            );

        
            orbitingObjects[i].localPosition = localOffset;
        }
    }
}

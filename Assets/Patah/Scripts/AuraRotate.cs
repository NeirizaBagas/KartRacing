// --- AuraRotate.cs ---
using UnityEngine;

public class AuraRotate : MonoBehaviour
{
    public float angleRange = 45f;     // Maksimum sudut osilasi Z lokal
    public float speed = 1f;           // Kecepatan osilasi

    private float initialZ;

    void Start()
    {
        initialZ = transform.localEulerAngles.z;
    }

    void Update()
    {
        float zRotation = initialZ + Mathf.PingPong(Time.time * speed * 2f, angleRange * 2f) - angleRange;
        Vector3 localEuler = transform.localEulerAngles;
        localEuler.z = zRotation;
        transform.localEulerAngles = localEuler;
    }
}

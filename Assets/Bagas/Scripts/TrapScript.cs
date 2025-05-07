using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public int dieDuration;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KartMover kart = other.transform.root.GetComponentInChildren<KartMover>();
            print("Die");
            kart.Die(dieDuration);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPlace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player");

            KartMover kart = other.transform.parent.GetComponentInChildren<KartMover>();

            if (kart != null)
            {
                kart.BoostExternal();
            }
        }
    }
}

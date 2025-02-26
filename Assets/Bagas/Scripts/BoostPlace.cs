using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPlace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KartController kart = other.transform.root.GetComponentInChildren<KartController>();

            if (kart != null)
            {
                kart.BoostExternal();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPaperStormScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioEffect != null)
            {
                audioEffect.Play(); // Play the audio effect when the glue spill is triggered
            }
            PlayerItemHandler item = other.transform.parent.GetComponentInChildren<PlayerItemHandler>();
            Debug.Log("player item: " + item.name);


            Destroy(gameObject); // Destroy the glue spill item after applying the effect
        }
    }
}

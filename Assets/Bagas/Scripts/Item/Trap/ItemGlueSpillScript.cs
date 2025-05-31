using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGlueSpillScript : MonoBehaviour
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
            KartMover kart = other.transform.parent.GetComponentInChildren<KartMover>();

            StartCoroutine(GlueSpillEffect(kart));
            Destroy(gameObject); // Destroy the glue spill item after applying the effect
        }
    }

    private IEnumerator GlueSpillEffect(KartMover kart)
    {
        kart.maxSpeed = kart.maxSpeed / 2; // Reduce speed by half

        yield return new WaitForSeconds(2f);

        kart.maxSpeed = kart.maxSpeed * 2; // Restore speed to normal
    }
}

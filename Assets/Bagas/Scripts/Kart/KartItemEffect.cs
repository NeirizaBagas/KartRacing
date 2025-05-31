using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartItemEffect : MonoBehaviour
{
    public KartMover _kart;

    public Image _paperEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            TrapScript trapScript = other.GetComponent<TrapScript>();

            if (_kart.shieldActive || _kart.isDead)
            {
                Destroy(other.gameObject); // Destroy the trap object if the kart has a shield or is dead
                return;
            }

            switch (trapScript.trapType)
            {
                case _TrapType.PaperStorm:
                    AudioManager.Instance.PlaySFX(11);
                    StartCoroutine(PaperStormEffect());
                    break;
                case _TrapType.GlueSpill:
                    AudioManager.Instance.PlaySFX(12);
                    StartCoroutine(GlueSpillEffect());
                    break;
                case _TrapType.StraplerTrap:
                    AudioManager.Instance.PlaySFX(13);
                    StrapplerEffect();
                    break;
                case _TrapType.SlipperyWater:
                    AudioManager.Instance.PlaySFX(14);
                    StartCoroutine(SlipperyWaterEffect());
                    break;
                case _TrapType.PinTrap:
                    AudioManager.Instance.PlaySFX(15);
                    StartPinEffect();
                    break;
            }

            Destroy(other.gameObject); // Destroy the trap object after applying the effect

        }
    }

    public void StartPaperStormEffect()
    {
        StartCoroutine(PaperStormEffect());
    }

    private IEnumerator PaperStormEffect()
    {
        _paperEffect.gameObject.SetActive(true); // Enable the paper storm effect UI

        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        _paperEffect.gameObject.SetActive(false); // Disable the paper storm effect UI
    }

    public void StartGlueSpillEffect()
    {
        StartCoroutine(GlueSpillEffect());
    }

    private IEnumerator GlueSpillEffect()
    {
        _kart.maxSpeed = _kart.maxSpeed / 2; // Reduce speed by half

        yield return new WaitForSeconds(2f);

        _kart.maxSpeed = _kart.maxSpeed * 2; // Restore speed to normal
    }

    public void StrapplerEffect()
    {
        _kart.Die(3); // Call the Die method on the KartMover
    }

    public void StartSlipperyWaterEffect()
    {
        StartCoroutine(SlipperyWaterEffect()); // Call the SlipperyWater method on the KartMover
    }

    private IEnumerator SlipperyWaterEffect()
    {
        _kart.canSteer = false; // Disable steering

        yield return new WaitForSeconds(2f);

        _kart.canSteer = true; // Re-enable steering
    }

    public void StartPinEffect()
    {
        _kart.Die(3); // Call the Die method on the KartMover
    }
}

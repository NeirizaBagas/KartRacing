using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _TrapType
{
    PaperStorm,
    GlueSpill,
    StraplerTrap,
    SlipperyWater,
    PinTrap,
}

public class TrapScript : MonoBehaviour
{
    public _TrapType trapType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KartItemEffect itemEffect = other.GetComponent<KartItemEffect>();

            switch (trapType)
            {
                case _TrapType.PaperStorm:
                    itemEffect.StartPaperStormEffect();
                    break;
                case _TrapType.GlueSpill:
                    itemEffect.StartGlueSpillEffect();
                    break;
                case _TrapType.StraplerTrap:
                    itemEffect.StrapplerEffect();
                    break;
                case _TrapType.SlipperyWater:
                    itemEffect.StartSlipperyWaterEffect();
                    break;
                case _TrapType.PinTrap:
                    itemEffect.StartPinEffect();
                    break;
            }

            Destroy(gameObject);
        }
    }

    
}

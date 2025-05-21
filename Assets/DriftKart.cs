using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftKart : MonoBehaviour
{
    public void driftSound()
    {
        AudioManager.Instance.PlaySFX(2);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeItem : MonoBehaviour
{
    [SerializeField] private AudioSource gachaSound;
    [SerializeField] private AudioSource getSound;

    void Start()
    {
        gachaSound = GetComponent<AudioSource>();
        getSound = GetComponentInChildren<AudioSource>();
    }

    public void Gacha()
    {
        Debug.Log("gacha");
        gachaSound.Play();
    }

    public void GetItem()
    {
        Debug.Log("Dapet Item");
        gachaSound.Stop();
        getSound.Play();
    }
}

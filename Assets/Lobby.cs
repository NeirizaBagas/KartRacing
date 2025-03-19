using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(PlayerInputManager))]
public class Lobby : MonoBehaviour
{
    public List<CarSelector> Car = new List<CarSelector>();
    private void Awake()
    {
        GameObject[] selectorObjects = GameObject.FindGameObjectsWithTag("Selector");

        foreach (GameObject obj in selectorObjects)
        {
            CarSelector carSelector = obj.GetComponent<CarSelector>();

            if (carSelector != null)
            {
                Car.Add(carSelector);
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.carSelector = Car;
        }
    }
}

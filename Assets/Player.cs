using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Player : MonoBehaviour
{
    public int id;
    public GameObject[] Character;

    private void Awake()
    {
        // Activate character based on GameManager's selected ID
        Character[GameManager.Instance.ID[id]].SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(PlayerInputManager))]
public class Lobby : MonoBehaviour
{
    public static Lobby Instance { get; private set; }
    public List<CarSelector> Car = new List<CarSelector>();
    public PlayerInput playerInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(gameObject);
            //return;
        }

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

        playerInput = GetComponent<PlayerInput>();
    }

    public void CheckAndReturnToMainMenu()
    {
        // Cek apakah masih ada pemain yang sudah memilih mobil
        foreach (CarSelector car in Car)
        {
            if (car.isSelected) // Jika masih ada yang memilih, tidak kembali ke MainMenu
            {
                return;
            }
        }

        // Jika semua pemain sudah batal memilih, kembali ke Main Menu
        SceneManager.LoadScene("MainMenu");
    }


}

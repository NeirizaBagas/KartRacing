using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    public GameObject _mainMenuCanvas;
    public GameObject _selectMenuCanvas;

    public GameObject _menu;
    public GameObject _select;

    public bool isSelect;

    public void GameStart()
    {
        _mainMenuCanvas.SetActive(false);
        _selectMenuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.Instance.SubmitInput)
        {
            if (!isSelect)
            {
                Select();
            }
            //else
            //{
            //    Unselect();
            //}
        }

        if (isSelect && InputManager.Instance.CancelInput)
        {
            Unselect();
        }
    }

    public void Select()
    {
        isSelect = true;

        OpenSelectMenu();
    }

    public void Unselect()
    {
        isSelect = false;

        CloseSelectMenu();
    }

    public void OpenSelectMenu()
    {
        _selectMenuCanvas.SetActive(true);
        _mainMenuCanvas.SetActive(false);

        // Pastikan controller bisa langsung navigasi
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_select);
    }

    public void CloseSelectMenu()
    {
        _mainMenuCanvas.SetActive(true);
        _selectMenuCanvas.SetActive(false);

        // Pastikan kembali ke tombol menu utama
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_menu);
    }



    public void Dua()
    {
        GameManager.Instance.playerCount = 2;
        SceneManager.LoadSceneAsync("Multiplayer2");
    }
    public void Tiga()
    {
        GameManager.Instance.playerCount = 3;
        SceneManager.LoadSceneAsync("Multiplayer3");
    }
    public void Empat()
    {
        GameManager.Instance.playerCount = 4;
        SceneManager.LoadSceneAsync("Multiplayer4");
    }
    public void Track(string track)
    {
        SceneManager.LoadSceneAsync(track);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(ExitArena);

    }

    public void ExitArena()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

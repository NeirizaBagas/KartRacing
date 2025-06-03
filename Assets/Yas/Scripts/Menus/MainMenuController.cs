using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip moveBtnSound;
    [SerializeField] private AudioClip cancelBtnSound;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionPanel;

    private GameObject lastSelected;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else if (Instance != null)
        {
            Destroy(this);
        }

    }

    private void Start()
    {

        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);

        //Set default keybind kalau belom ada keybind sama sekali
        if (KeybindSaveSystem.LoadKeybind() == null)
        {
            ControlKeybind defaultKeybind = new ControlKeybind();
            KeybindSaveSystem.SaveKeybind(defaultKeybind);
            KeybindSaveSystem.SetCurrentKeybind(KeybindSaveSystem.LoadKeybind());
            Debug.Log("Keybind set to default");
        }

        //AudioManager.Instance.PlayAudio(menuMusic, 0);
        AudioManager.Instance.PlayLoopingAudio(menuMusic,0);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if(lastSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
            }
        }
        else
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
    }


    public void OpenMenu()
    {
        AudioManager.Instance.PlayAudio(buttonSound, 1);
        StartCoroutine(DelayOpenMenu());
    }

    public IEnumerator DelayOpenMenu()
    {
        startPanel.GetComponent<Animator>().SetBool("dissapear", true);
        yield return new WaitForSeconds(0.5f);
        startPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OpenOption()
    {
        AudioManager.Instance.PlayAudio(buttonSound, 1);
        StartCoroutine(DelayOpenOption());
    }

    public void MovingBtn()
    {
        AudioManager.Instance.PlayAudio (moveBtnSound, 1);
    }

    public void DummyConfirm()
    {
        AudioManager.Instance.PlayAudio(buttonSound, 1);
    }

    public IEnumerator DelayOpenOption()
    {
        menuPanel.GetComponent<Animator>().SetBool("SlideOut",true);
        yield return new WaitForSeconds(0.5f);
        menuPanel.SetActive(false);
        optionPanel.SetActive(true);

        FindObjectOfType<OptionMenu>().SetupOption();
        //FindObjectOfType<KeybindOption>().SetupControlOption();
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
    }

    public void CancelCloseOption()
    {
        AudioManager.Instance.PlayAudio(cancelBtnSound, 1);
        menuPanel.SetActive(true);
        optionPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
    }

    public void ConfirmCloseOption()
    {
        AudioManager.Instance.PlayAudio(buttonSound, 1);
        menuPanel.SetActive(true);
        optionPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
    }


    public void QuitGame()
    {
        AudioManager.Instance.PlayAudio(cancelBtnSound, 1);
        Application.Quit();
    }
}
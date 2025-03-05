using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapDisplay : MonoBehaviour
{
    public TextMeshProUGUI mapName;
    public Image mapImage;
    public Button playButton;
    public GameObject lockIcon;

    public void DisplayMap(Map _map)
    {
        mapName.text = _map.mapName;
        //mapName.color = _map.nameColor;
        mapImage.sprite = _map.mapImage;

        bool mapUnlocked = PlayerPrefs.GetInt("CurrentScene", 0) >= _map.mapIndex;
        lockIcon.SetActive(!mapUnlocked);
        playButton.interactable = mapUnlocked;
        if (mapUnlocked) mapImage.color = Color.white;
        else mapImage.color = Color.gray;

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => SceneManager.LoadScene(_map.sceneToLoad.name));
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapDisplay : MonoBehaviour
{
    public TextMeshProUGUI mapName;  // Nama Map
    public Image mapImage;           // Gambar Map
    public Button playButton;        // Tombol Play
    public GameObject lockIcon;      // Ikon Kunci (jika terkunci)

    public void DisplayMap(Map _map)
    {
        // Tampilkan nama map
        mapName.text = _map.mapName;

        // Ganti gambar map
        mapImage.sprite = _map.mapImage;

        // Cek apakah map terkunci berdasarkan indeks
        bool mapUnlocked = PlayerPrefs.GetInt("CurrentScene", 0) >= _map.mapIndex;
        lockIcon.SetActive(!mapUnlocked);
        playButton.interactable = mapUnlocked;

        // Ubah warna jika terkunci
        mapImage.color = mapUnlocked ? Color.white : Color.gray;

        // Bersihkan listener sebelum menambah yang baru
        playButton.onClick.RemoveAllListeners();

        // Jika map terbuka, tambahkan event untuk pindah scene
        if (mapUnlocked)
        {
            playButton.onClick.AddListener(() => SceneManager.LoadScene(_map.sceneToLoad.name));
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    public GameObject[] carList; // Daftar mobil
    public bool[] playerConfirmed; // Status konfirmasi pemain
    public int[] selectedCars; // Mobil yang dipilih setiap pemain

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        int playerCount = PlayerPrefs.GetInt("PlayerCount", 4); // Ambil jumlah pemain
        playerConfirmed = new bool[playerCount];
        selectedCars = new int[playerCount];
    }

    public void ConfirmSelection(int playerIndex)
    {
        playerConfirmed[playerIndex] = true;

        // Cek apakah semua pemain sudah mengkonfirmasi
        if (CheckAllConfirmed())
        {
            StartGame();
        }
    }

    bool CheckAllConfirmed()
    {
        foreach (bool confirmed in playerConfirmed)
        {
            if (!confirmed) return false;
        }
        return true;
    }

    void StartGame()
    {
        // Simpan pilihan mobil ke PlayerPrefs atau ScriptableObject
        for (int i = 0; i < selectedCars.Length; i++)
        {
            PlayerPrefs.SetInt("Player" + i + "Car", selectedCars[i]);
        }

        SceneManager.LoadScene("Gameplay"); // Pindah ke scene Gameplay
    }
}
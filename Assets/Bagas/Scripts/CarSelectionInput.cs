using UnityEngine;
using UnityEngine.InputSystem;

public class CarSelectionInput : MonoBehaviour
{
    public PlayerIndicator[] playerIndicators; // Indikator pemain
    public int[] selectedCars; // Mobil yang dipilih setiap pemain
    public int totalCars; // Jumlah mobil yang tersedia
    [SerializeField] private int playerIndex; // Index pemain

    void Start()
    {
        // Debugging: Cek isi array playerIndicators
        for (int i = 0; i < playerIndicators.Length; i++)
        {
            if (playerIndicators[i] == null)
            {
                Debug.LogError("PlayerIndicator at index " + i + " is null!");
            }
            else
            {
                Debug.Log("PlayerIndicator at index " + i + " is assigned to: " + playerIndicators[i].name);
            }
        }

        // Set playerIndex berdasarkan nama GameObject
        string playerName = gameObject.name.Replace("Player", "");
        if (int.TryParse(playerName, out int index))
        {
            playerIndex = index - 1; // Konversi ke indeks berbasis 0
        }
        else
        {
            Debug.LogError("Nama GameObject tidak mengandung angka yang valid: " + gameObject.name);
            playerIndex = 0; // Default value jika parsing gagal
        }

        // Pastikan playerIndex dalam batas array
        if (playerIndex < 0 || playerIndex >= playerIndicators.Length || playerIndex >= selectedCars.Length)
        {
            Debug.LogError("PlayerIndex out of range: " + playerIndex);
            playerIndex = 0; // Default value jika indeks tidak valid
        }
    }

    // Method untuk mengelola input Move
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();

            // Pastikan playerIndex dalam batas array
            if (playerIndex >= 0 && playerIndex < playerIndicators.Length)
            {
                if (input.x > 0) // Pindah ke mobil berikutnya
                {
                    selectedCars[playerIndex] = (selectedCars[playerIndex] + 1) % totalCars;
                }
                else if (input.x < 0) // Pindah ke mobil sebelumnya
                {
                    selectedCars[playerIndex] = (selectedCars[playerIndex] - 1 + totalCars) % totalCars;
                }

                // Pindahkan indikator pemain ke mobil yang dipilih
                if (playerIndicators[playerIndex] != null)
                {
                    playerIndicators[playerIndex].MoveToCar(selectedCars[playerIndex]);
                }
                else
                {
                    Debug.LogError("PlayerIndicator is null for playerIndex: " + playerIndex);
                }
            }
            else
            {
                Debug.LogError("PlayerIndex out of range: " + playerIndex);
            }
        }
    }

    // Method untuk mengelola input Confirm
    public void OnConfirm()
    {
        // Pastikan playerIndex dalam batas array
        if (playerIndex >= 0 && playerIndex < selectedCars.Length)
        {
            PlayerInputManager.Instance.ConfirmSelection(playerIndex);
        }
        else
        {
            Debug.LogError("PlayerIndex out of range: " + playerIndex);
        }
    }
}
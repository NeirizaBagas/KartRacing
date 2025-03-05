using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    void Start()
    {
        // Cek jumlah perangkat yang tersedia
        var gamepads = Gamepad.all;

        if (gamepads.Count < 2)
        {
            Debug.LogError("Kurang dari 2 gamepad terhubung!");
            return;
        }

        // Pastikan setiap player punya device sendiri
        var players = FindObjectsOfType<PlayerInput>();
        if (players.Length >= 2)
        {
            players[0].SwitchCurrentControlScheme(gamepads[0]);
            players[1].SwitchCurrentControlScheme(gamepads[1]);
        }
    }
}

using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{
    public MatchmakingData data;
    private PlayerInputManager _inputManager;

    // Tambahkan array untuk menyimpan referensi ke KartMeshesManager yang sudah ada di scene
    public KartMeshesManager[] preSpawnedKarts;

    private void Start()
    {
        if (data == null)
        {
            Debug.LogError("MatchmakingData tidak ditemukan!");
            return;
        }

        // Langsung assign data ke KartMeshesManager yang sesuai
        var validData = data.playersData.Where((d) => d != null).ToArray();
        Debug.Log($"Jumlah pemain yang valid: {validData.Length}");

        foreach (var player in validData)
        {
            Debug.Log($"Player ID: {player.playerId}, Kart ID: {player.kartId}");
        }


        foreach (var kart in preSpawnedKarts)
        {
            if (kart == null) continue;

            var playerData = validData.FirstOrDefault(d => d.playerId == kart.id);
            if (playerData != null)
            {
                kart.selectedId = playerData.kartId;
                kart.InitializeKart(); // Panggil method baru untuk setup kart
            }
            else
            {
                Debug.LogWarning($"Kart ID {kart.id} tidak ditemukan di data pemain yang valid.");
            }
        }

        //for (int i = 0; i < validData.Length; i++)
        //{
        //    // Cari KartMeshesManager dengan ID yang sesuai
        //    var kartMesh = preSpawnedKarts.FirstOrDefault(k => k.id == validData[i].playerId);
        //    if (kartMesh != null)
        //    {
        //        kartMesh.selectedId = validData[i].kartId;
        //        kartMesh.InitializeKart(); // Panggil method baru untuk setup kart
        //    }
        //}
    }

    // Method untuk mengambil data kart ID berdasarkan player ID
    public int GetKartIdForPlayer(int playerId)
    {
        var playerData = data.playersData.FirstOrDefault(d => d != null && d.playerId == playerId);
        if (playerData == null)
        {
            Debug.LogWarning($"Player ID {playerId} tidak ditemukan di data pemain.");
            return 0; // Atau nilai default lainnya
        }

        Debug.Log($"Mengambil kart ID {playerData.kartId} untuk player ID {playerId}");
        return playerData.kartId;
    }
}
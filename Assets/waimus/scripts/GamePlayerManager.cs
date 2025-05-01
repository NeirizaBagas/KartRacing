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

        // Langsung assign data ke KartMeshesManager yang sesuai
        var validData = data.playersData.Where((d) => d != null).ToArray();
        for (int i = 0; i < validData.Length; i++)
        {
            // Cari KartMeshesManager dengan ID yang sesuai
            var kartMesh = preSpawnedKarts.FirstOrDefault(k => k.id == validData[i].playerId);
            if (kartMesh != null)
            {
                kartMesh.selectedId = validData[i].kartId;
                kartMesh.InitializeKart(); // Panggil method baru untuk setup kart
            }
        }
    }

    // Method untuk mengambil data kart ID berdasarkan player ID
    public int GetKartIdForPlayer(int playerId)
    {
        var playerData = data.playersData.FirstOrDefault(d => d != null && d.playerId == playerId);
        return playerData?.kartId ?? 0;
    }
}
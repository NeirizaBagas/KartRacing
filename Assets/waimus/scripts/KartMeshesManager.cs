using UnityEngine;

public class KartMeshesManager : MonoBehaviour
{
    public int id; // ID untuk identifikasi player
    public int selectedId;
    public GameObject[] kartMeshes;

    private GamePlayerManager gamePlayerManager;

    private void Awake()
    {
        // Cari GamePlayerManager di scene
        gamePlayerManager = FindObjectOfType<GamePlayerManager>();
        if (gamePlayerManager == null)
        {
            Debug.LogError("GamePlayerManager tidak ditemukan di scene!");
        }
    }

    private void Start()
    {
        if (gamePlayerManager != null)
        {
            // Ambil data kart langsung dari GamePlayerManager
            selectedId = gamePlayerManager.GetKartIdForPlayer(id);
            Debug.Log($"Kart ID untuk player {id} adalah {selectedId}");
            InitializeKart();
        }
    }

    public void InitializeKart()
    {
        Debug.Log($"Menginisialisasi kart untuk player {id} dengan kart ID {selectedId}");

        if (kartMeshes == null || kartMeshes.Length < 1 )
        {
            Debug.LogError("Kart meshes tidak ditemukan atau tidak ada yang diassign!");
            return;
        }

        //if (kartMeshes.Length < 1) return;

        // Nonaktifkan semua mesh dulu
        foreach (var mesh in kartMeshes)
        {
            if (mesh != null)
            {
                mesh.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Mesh kart tidak ditemukan!");
            }
        }

        // Aktifkan mesh yang sesuai
        if (selectedId >= 0 && selectedId < kartMeshes.Length)
        {
            if (kartMeshes[selectedId] != null)
            {
                kartMeshes[selectedId].SetActive(true);
                Debug.Log($"Mesh kart dengan ID {selectedId} diaktifkan untuk {id}.");
            }
            else
            {
                Debug.LogError($"Mesh kart dengan ID {selectedId} tidak ditemukan untuk {id} !");
            }
        }
        else
        {
            Debug.LogError($"ID kart {selectedId} tidak valid untuk player {id}!");
        }
    }
}
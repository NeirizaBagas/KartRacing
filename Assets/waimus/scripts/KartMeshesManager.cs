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
    }

    private void Start()
    {
        if (gamePlayerManager != null)
        {
            // Ambil data kart langsung dari GamePlayerManager
            selectedId = gamePlayerManager.GetKartIdForPlayer(id);
            InitializeKart();
        }
    }

    public void InitializeKart()
    {
        if (kartMeshes.Length < 1) return;

        // Nonaktifkan semua mesh dulu
        foreach (var mesh in kartMeshes)
        {
            mesh.SetActive(false);
        }

        // Aktifkan mesh yang sesuai
        if (selectedId >= 0 && selectedId < kartMeshes.Length)
        {
            kartMeshes[selectedId].SetActive(true);
        }
    }
}
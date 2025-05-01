using UnityEngine;

public class KartMeshesManager : MonoBehaviour
{
    public int id;

    public int selectedId;

    public GameObject[] kartMeshes;

    //private void Awake()
    //{
    //    // Activate character based on GameManager's selected ID
    //    kartMeshes[GameManager.Instance.ID[id]].SetActive(true);
    //}

    private void Start()
    {
        if (kartMeshes.Length < 1) return;
        for (int i = 0; i < kartMeshes.Length; i++)
        {
            kartMeshes[i].SetActive(i == selectedId);
        }
    }
}
using UnityEngine;

public class KartMeshesManager : MonoBehaviour
{
    public int selectedId;

    [SerializeField] private GameObject[] kartMeshes; 

    private void Start()
    {
        if (kartMeshes.Length < 1) return;
        for (int i = 0; i < kartMeshes.Length; i++)
        {
            kartMeshes[i].SetActive(i == selectedId);
        }
    }
}
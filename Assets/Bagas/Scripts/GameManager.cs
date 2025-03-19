using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton reference

    public int playerCount;
    public List<CarSelector> carSelector = new List<CarSelector>(); // Stores references to selectors
    public List<int> carIds = new List<int>(); // Stores selected character IDs
    public int[] ID; // Stores final selected character IDs

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene load event
    }

    private void Update()
    {
        // Ensure all players have selected characters before switching scenes
        if (carSelector != null && carSelector.Count > 0 && carSelector.All(c => c.isSelected))
        {
            StoreCarIDs(); // Save selected character IDs
            SceneManager.LoadScene("MapSelect");
        }
    }

    public void StoreCarIDs()
    {
        carIds.Clear(); // Clear previous selection data

        foreach (CarSelector selector in carSelector)
        {
            carIds.Add(selector.carID);
        }

        ID = carIds.ToArray(); // Store IDs in an array
    }

    public List<int> GetSelectedCarIDs()
    {
        return new List<int>(ID); // Return a copy of the stored IDs
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MapSelect")
        {
            carSelector.Clear();
            carIds.Clear();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe when destroyed
    }
}

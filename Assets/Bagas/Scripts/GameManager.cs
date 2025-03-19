using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton reference

    public int playerCount;
    public List<CharacterSelector> characterSelector = new List<CharacterSelector>(); // Stores references to selectors
    public List<int> characterIds = new List<int>(); // Stores selected character IDs
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
        if (characterSelector != null && characterSelector.Count > 0 && characterSelector.All(c => c.isSelected))
        {
            StoreCharacterIDs(); // Save selected character IDs
            SceneManager.LoadScene("CharacterSelector");
        }
    }

    public void StoreCharacterIDs()
    {
        characterIds.Clear(); // Clear previous selection data

        foreach (CharacterSelector selector in characterSelector)
        {
            characterIds.Add(selector.CharacterID);
        }

        ID = characterIds.ToArray(); // Store IDs in an array
    }

    public List<int> GetSelectedCharacterIDs()
    {
        return new List<int>(ID); // Return a copy of the stored IDs
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MapSelectionScreen")
        {
            characterSelector.Clear();
            characterIds.Clear();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe when destroyed
    }
}

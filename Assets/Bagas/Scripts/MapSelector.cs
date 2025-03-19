using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour
{
    public MapDisplay mapDisplay;
    public Map[] maps;
    public Button[] mapButtons; // Semua button map

    private int currentIndex = 0;
    private float moveCooldown = 0.2f;
    private float lastMoveTime = 0f;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        // Setup tombol button untuk memilih map
        for (int i = 0; i < mapButtons.Length; i++)
        {
            int index = i; // Simpan index agar tidak berubah dalam loop
            mapButtons[i].onClick.AddListener(() => SelectMap(index));
        }

        UpdateMapDisplay();
    }

    void Update()
    {
        Vector2 moveInput = playerInput.actions["Navigate"].ReadValue<Vector2>();

        if (Time.time - lastMoveTime > moveCooldown)
        {
            if (moveInput.x > 0.5f)
            {
                MoveSelection(1);
                lastMoveTime = Time.time;
            }
            else if (moveInput.x < -0.5f)
            {
                MoveSelection(-1);
                lastMoveTime = Time.time;
            }
        }

        if (playerInput.actions["Submit"].WasPressedThisFrame())
        {
            SelectMap(currentIndex);
        }
        if (playerInput.actions["Cancel"].WasPressedThisFrame())
        {
            SceneManager.LoadScene("Multiplayer2");
        }
    }

    public void MoveSelection(int direction)
    {
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = maps.Length - 1;
        else if (currentIndex >= maps.Length) currentIndex = 0;

        UpdateMapDisplay();
    }

    void UpdateMapDisplay()
    {
        mapDisplay.DisplayMap(maps[currentIndex]);
    }

    public void SelectMap(int index)
    {
        if (index >= 0 && index < maps.Length)
        {
            SceneManager.LoadScene(maps[index].sceneToLoad.name);
        }
    }
}

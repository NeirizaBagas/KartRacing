using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelector : MonoBehaviour
{
    public int playerID;
    public RectTransform cursor;  // UI cursor to move around
    public Button[] carButtons; // Assign buttons in the inspector
    private int currentIndex = 0;
    private float moveCooldown = 0.2f; // Time before another move is allowed
    private float lastMoveTime = 0f;
    public GameObject[] carDisplay;
    public int carID;
    public bool isSelected = false;


    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        //currentIndex = 2;
        UpdateCursorPosition();
    }

    void Update()
    {
        Vector2 moveInput = playerInput.actions["Navigate"].ReadValue<Vector2>();

        if (!isSelected)
        {
            if (Time.time - lastMoveTime > moveCooldown) // Only move if cooldown has passed
            {
                if (moveInput.x > 0.5f) // Move Right
                {
                    MoveCursor(1);
                    lastMoveTime = Time.time;
                }
                else if (moveInput.x < -0.5f) // Move Left
                {
                    MoveCursor(-1);
                    lastMoveTime = Time.time;
                }
            }
        }

        if (playerInput.actions["Submit"].WasPressedThisFrame())
        {
            SelectCar();
        }
        if (playerInput.actions["Cancel"].WasPressedThisFrame())
        {
            if (isSelected) // Jika player sudah memilih, batalkan saja, jangan keluar ke Main Menu
            {
                CancelCar();
            }
            else // Jika player belum memilih, dia keluar ke Main Menu
            {
                SceneManager.LoadScene("MainMenu");
            }
        }



    }
    void UpdateCursorPosition()
    {
        cursor.position = carButtons[currentIndex].transform.position;
    }
    void UpdateCharacterDisplay()
    {
        // Deactivate all character displays first
        for (int i = 0; i < carDisplay.Length; i++)
        {
            carDisplay[i].SetActive(false);
        }

        // Activate the selected character's display
        if (cursor.position == carButtons[currentIndex].transform.position)
        {
            carDisplay[currentIndex].SetActive(true);
        }
    }
    void MoveCursor(int direction)
    {
        currentIndex += direction;
        currentIndex = Mathf.Clamp(currentIndex, 0, carButtons.Length - 1);

        cursor.position = carButtons[currentIndex].transform.position;

        UpdateCharacterDisplay(); // Update the character preview
    }

    void SelectCar()
    {
        Debug.Log($"Player {playerID} selected {carButtons[currentIndex].name}");
        isSelected = true;
        carID = currentIndex;
        // Store selection and move to game scene
    }

    void CancelCar()
    {
        isSelected = false;
        carID = 0;
    }
}

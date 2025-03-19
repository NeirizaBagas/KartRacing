using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public int playerID;
    public RectTransform cursor;  // UI cursor to move around
    public Button[] characterButtons; // Assign buttons in the inspector
    private int currentIndex = 0;
    private float moveCooldown = 0.2f; // Time before another move is allowed
    private float lastMoveTime = 0f;
    public GameObject[] characterDisplay;
    public int CharacterID;
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
            SelectCharacter();
        }
        if (playerInput.actions["Cancel"].WasPressedThisFrame())
        {
            CancelCharacter();
        }
    }
    void UpdateCursorPosition()
    {
        cursor.position = characterButtons[currentIndex].transform.position;
    }
    void UpdateCharacterDisplay()
    {
        // Deactivate all character displays first
        for (int i = 0; i < characterDisplay.Length; i++)
        {
            characterDisplay[i].SetActive(false);
        }

        // Activate the selected character's display
        if (cursor.position == characterButtons[currentIndex].transform.position)
        {
            characterDisplay[currentIndex].SetActive(true);
        }
    }
    void MoveCursor(int direction)
    {
        currentIndex += direction;
        currentIndex = Mathf.Clamp(currentIndex, 0, characterButtons.Length - 1);

        cursor.position = characterButtons[currentIndex].transform.position;

        UpdateCharacterDisplay(); // Update the character preview
    }

    void SelectCharacter()
    {
        Debug.Log($"Player {playerID} selected {characterButtons[currentIndex].name}");
        isSelected = true;
        CharacterID = currentIndex;
        // Store selection and move to game scene
    }

    void CancelCharacter()
    {
        isSelected = false;
        CharacterID = 0;
    }
}

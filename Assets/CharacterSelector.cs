using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public int playerID;
    public GameObject selectionCursor; // Assign UI cursor object
    public Button[] characterButtons; // Assign all selectable character buttons

    private int selectedIndex = 0;
    [SerializeField] private PlayerInput playerInput;
    private GamepadActions controller;

    private void Awake()
    {
        controller = new GamepadActions();
    }

    private void Start()
    {
        UpdateCursorPosition();
    }

    private void OnEnable()
    {
        //if (playerInput == null) return;
        //playerInput.actions.Enable();
        //playerInput.actions.FindAction("Navigate").started += OnNavigate;

        // Enable the controller so it's accessible
        controller.Enable();
        controller.UI.Navigate.started += OnNavigate;
    }

    private void OnDisable()
    {
        //if (playerInput == null) return;
        //playerInput.actions.Disable();
        //playerInput.actions.FindAction("Navigate").started -= OnNavigate;
        controller.UI.Disable();
        controller.UI.Navigate.started -= OnNavigate;
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<Vector2>();
        OnNavigateValue(inputValue);
    }

    public void OnNavigateValue(Vector2 input)
    {
        //Vector2 input = value.Get<Vector2>();

        if (input.x > 0) NextCharacter();
        else if (input.x < 0) PreviousCharacter();
    }

    public void OnSelect()
    {
        Debug.Log($"Player {playerID} selected character: {characterButtons[selectedIndex].name}");
    }

    private void NextCharacter()
    {
        selectedIndex = (selectedIndex + 1) % characterButtons.Length;
        UpdateCursorPosition();
    }

    private void PreviousCharacter()
    {
        selectedIndex = (selectedIndex - 1 + characterButtons.Length) % characterButtons.Length;
        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        selectionCursor.transform.position = characterButtons[selectedIndex].transform.position;
    }
}

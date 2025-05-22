using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Component that processes input received from InputSystem and to be
/// passed into other consuming component such as KartMover
/// </summary>
public class KartInputProcessor : MonoBehaviour
{
    private PlayerInput _input;
    private InputActionAsset _actionAsset;
    private InputActionMap _inputMap;

    private void Awake()
    {
        InitializeInput();
    }

    public void InitializeInput()
    {
        _input = GetComponent<PlayerInput>();
        if (_input == null)
        {
            Debug.LogError("PlayerInput component is missing!");
            return;
        }

        _actionAsset = _input.actions;
        if (_actionAsset == null)
        {
            Debug.LogError("InputActionAsset is missing!");
            return;
        }

        _inputMap = _actionAsset.FindActionMap("Player");
        if (_inputMap == null)
        {
            Debug.LogError("Action Map 'Player' not found in InputActionAsset!");
        }
    }

    public InputAction GetAction(string name) => _inputMap.FindAction(name);
    public Vector2 GetMovementInput() => GetVectorInput(GetAction("Move"));

#region static helpers
    public static Vector2 GetVectorInput(InputAction action) => action.ReadValue<Vector2>();
    public static float GetFloatInput(InputAction action) => action.ReadValue<float>();
    public static bool GetActionPressed(InputAction action) => action.WasPressedThisFrame();
    public static bool GetActionReleased(InputAction action) => action.WasReleasedThisFrame();
    public static bool GetActionHeld(InputAction action) => action.IsPressed();
#endregion
}
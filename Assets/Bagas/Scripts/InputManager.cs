using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public bool SubmitInput { get; private set; }
    public bool CancelInput { get; private set; }

    private PlayerInput _playerInput;
    private InputAction submit;
    private InputAction cancel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        submit = _playerInput.actions["Submit"];
        cancel = _playerInput.actions["Cancel"];
    }

    private void Update()
    {
        SubmitInput = submit.WasPressedThisFrame();
        CancelInput = cancel.WasPressedThisFrame();
    }
}
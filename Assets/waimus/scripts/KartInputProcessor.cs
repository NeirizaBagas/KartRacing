using System;
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
    
    public DebugData debugData;
    
    public void InitializeInput()
    {
        _input = GetComponent<PlayerInput>();
        _actionAsset = _input.actions;
        _inputMap = _actionAsset.FindActionMap("Player");
        
        foreach (var action in _inputMap)
        {
            action.performed += (ctx) =>
            {
                debugData.counter += 1;
                debugData.lastActionPerformed = $"{debugData.counter}: {ctx.action.name}";
            };
        }
    }

    [System.Serializable]
    public class DebugData
    {
        public int counter;
        public string lastActionPerformed = "";
    }
}
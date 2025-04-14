using UnityEngine;

/// <summary>
/// Component that processes movement implementation given the input from an InputProcessor
/// </summary>
public class KartMover : MonoBehaviour
{
    private KartInputProcessor _inputProcessor;

    private void Awake()
    {
        _inputProcessor = GetComponent<KartInputProcessor>();
    }
}
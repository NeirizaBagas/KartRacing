using UnityEngine;

/// <summary>
/// Component that processes movement implementation given the input from an InputProcessor
/// </summary>
public class KartMover : MonoBehaviour
{
    private KartInputProcessor _inputProcessor;

    private Vector3 _velocity;

    private void Awake()
    {
        _inputProcessor = GetComponent<KartInputProcessor>();
    }

    private void Update() => _velocity = new Vector3(_inputProcessor.GetMovementInput().x, 0, _inputProcessor.GetMovementInput().y);
    private void FixedUpdate() => transform.position += _velocity * (10.0f * Time.fixedDeltaTime);
}
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages camera split screen functionality because the PlayerInputManager wont do it so I have to do it myself.
/// Cameras are created in a separate object group and then added to a List of which used to dynamically manage
/// camera viewport's rect. llm tech bros can suck my nut. This does not manage camera's follow target functionality.
/// </summary>
public class CameraSplitscreenManager : MonoBehaviour
{
    [Header("Data")] 
    [SerializeField] private List<Camera> _cameras = new();

    [Header("Camera Follow Configurations")]
    public Vector3 followOffset = new Vector3(0.0f, 2.5f, -5.0f);
    public float pitchAngle = 15.0f;
    public float followSpeed = 0.3f;

    private float _viewWidth, _viewHeight;
    
    public static CameraSplitscreenManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    public Camera CreateCamera(Transform target)
    {
        var cam = new GameObject($"Camera{target.name}").AddComponent<Camera>();
        var follow = cam.gameObject.AddComponent<KartCameraFollow>();
        
        // Object transform configuraiton
        cam.transform.parent = transform;
        follow.SetTarget(target, followOffset, pitchAngle, followSpeed);
        
        // Internal configuration
        _cameras.Add(cam);
        ConfigureSplitscreen();
        
        return cam;
    }

    private void ConfigureSplitscreen()
    {
        switch (_cameras.Count)
        {
        case 4: // Can also check by divisible by 2 numbers instead of fixed 4
            _viewWidth = 0.5f;
            _viewHeight = 0.5f;
        
            for (int x = 0; x < _cameras.Count / 2; x++)
            {
                for (int y = 0; y < _cameras.Count / 2; y++)
                {
                    Vector2 position = new Vector2(_viewWidth * y, _viewHeight * Mathf.Abs(x - 1));
                    Vector2 size = new Vector2(_viewWidth, _viewHeight);
                    Rect rect = new(position, size);

                    _cameras[(x * _cameras.Count / 2) + y].rect = rect;                    
                }
            }
            break;
        case 3:
            _viewWidth = 0.5f;
            _viewHeight = 0.5f;

            for (int i = 0; i < _cameras.Count; i++)
            {
                Vector2 position = i == _cameras.Count - 1 
                    ? new Vector2(_viewWidth - (_viewWidth * 0.5f), 0) 
                    : new Vector2(_viewWidth * i, 0.5f);
                Vector2 size = new Vector2(_viewWidth, _viewHeight);
                Rect rect = new(position, size);
                _cameras[i].rect = rect;          
            }
            break;
        default:
            // Horizontal column split
            _viewWidth = 1f/_cameras.Count;
            _viewHeight = 1f;

            for (int i = 0; i < _cameras.Count; i++)
            {
                Vector2 position = new Vector2(_viewWidth * i, 0);
                Vector2 size = new Vector2(_viewWidth, _viewHeight);
                Rect rect = new(position, size);

                _cameras[i].rect = rect;
            }
            break;
        }
    }
}
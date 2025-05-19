using System.Collections.Generic;
using System.Linq;
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

    [Header("Referensi")]

    [Header("Camera Follow Configurations")]
    public Vector3 followOffset = new Vector3(0.0f, 2f, -5.0f);
    public float pitchAngle = 18.0f;
    public float followSpeed = 0.2f;

    private float _viewWidth, _viewHeight;
    private Canvas _canvas;
    private RectTransform[] _voids;

    public static CameraSplitscreenManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _canvas = transform.GetChild(0).GetComponent<Canvas>();
        _voids = new RectTransform[2]; 
        _voids[0] = _canvas.transform.GetChild(0).GetComponent<RectTransform>();
        _voids[1] = _canvas.transform.GetChild(1).GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        ConfigureVoidSize(_cameras.Count == 3);
    }

    [ContextMenu("AddTestCamera")]
    public void AddTestCamera()
    {
        var c = new GameObject($"TestCamera_").AddComponent<Camera>();
        c.transform.position = Vector3.zero;
        c.transform.parent = transform;
        c.clearFlags = CameraClearFlags.SolidColor;
        _cameras.Add(c);  
        
        ConfigureSplitscreen();
    }
    
    [ContextMenu("DeleteTestCamera")]
    public void DeleteTestCamera()
    {
        var c = _cameras.Last();
        _cameras.Remove(c);
        Destroy(c.gameObject);
        ConfigureSplitscreen();
    }

    public Camera CreateCamera(Transform target)
    {
        var pivot = new GameObject("ChaseCameraRig");
        var cam = new GameObject($"Camera{target.name}").AddComponent<Camera>();
        var follow = pivot.AddComponent<KartCameraFollow>(); 
        
        // Object transform configuraiton
        pivot.transform.parent = transform;
        cam.transform.parent = pivot.transform;
        cam.transform.localPosition = followOffset;
        //follow.SetTarget(target.GetChild(0), pitchAngle, followSpeed);
        
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
            // ConfigureVoidSize(false);
        
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
            // ConfigureVoidSize(true);

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
            // ConfigureVoidSize(false);

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

    private void ConfigureVoidSize(bool isActive)
    {
        if (!isActive)
        {
            _canvas.gameObject.SetActive(false);
            return;
        }
        
        _canvas.gameObject.SetActive(true);
        
        // Black screen void of unoccupied screen
        _voids[0].anchorMin = new Vector3(0, 0);
        _voids[0].anchorMax = new Vector3(0, 0);
        _voids[1].anchorMin = new Vector3(1, 0);
        _voids[1].anchorMax = new Vector3(1, 0);
        _voids[0].pivot = new Vector2(0, 0);
        _voids[1].pivot = new Vector2(1, 0);
        _voids[0].sizeDelta = new Vector2(Screen.width / 4f, Screen.height / 2f);
        _voids[1].sizeDelta = new Vector2(Screen.width / 4f, Screen.height / 2f);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Burst.CompilerServices;
using Cinemachine;
using UnityEngine.VFX;

/// <summary>
/// Component that processes movement implementation given the input from an InputProcessor modified from KartController
/// </summary>
public class KartMover : MonoBehaviour
{
    [SerializeField] private float speed, currentSpeed;
    private float rotate, currentRotate;
    private int driftDirection;
    private float driftPower;
    private bool first, second, third, fourth;
    private Color c;

    [Header("Input data")]
    public KartInputProcessor _inputProcessor;
    private Vector2 directionInput;

    [Header("Model")]
    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;

    [Header("Bools")]
    public bool drifting;
    public bool boosting;
    public bool canMove;
    public bool shieldActive;
    public bool isMoving;
    public bool isGround;
    public bool isAccelerating = false;
    public bool isMove = false;
    public bool isPeak = false;

    [Header("Parameters")]
    public int driftMode = 0;
    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;
    public int dieDuration;
    public float follow = 0.4f;

    [Header("Particles")]
    public List<ParticleSystem> primaryParticles = new List<ParticleSystem>();
    public List<ParticleSystem> secondaryParticles = new List<ParticleSystem>();
    public Transform wheelParticles;
    public Transform flashParticles;
    public Color[] turboColors;
    private VisualEffect[] booster1Effects;
    private VisualEffect[] booster2Effects;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera virtualCam;
    private float defaultFOV;


    public PlayerItemHandler playerItemHandler;
    public LapManager lap;
    public Animator anim;

    private void Awake()
    {
        // Ambil referensi KartInputProcessor dari parent
        _inputProcessor = GetComponentInParent<KartInputProcessor>();
        anim = kartModel.GetComponentInChildren<Animator>();

        if (_inputProcessor == null)
        {
            Debug.LogError("KartInputProcessor not found in parent!");
        }

        playerItemHandler ??= GetComponent<PlayerItemHandler>();

        if (virtualCam != null)
        {
            defaultFOV = virtualCam.m_Lens.FieldOfView;
            virtualCam.Follow = this.transform;
            virtualCam.LookAt = this.transform;
        }

        if (this.gameObject.activeSelf == true)
        {
            virtualCam.Follow = this.transform;
            virtualCam.LookAt = this.transform;
        }

        //postVolume = Camera.main.GetComponent<PostProcessVolume>(); // Poss Process
        //postProfile = postVolume.profile;
    }

    void Start()
    {

        for (int i = 0; i < wheelParticles.GetChild(0).childCount; i++)
        {
            primaryParticles.Add(wheelParticles.GetChild(0).GetChild(i).GetComponent<ParticleSystem>());
        }

        for (int i = 0; i < wheelParticles.GetChild(1).childCount; i++)
        {
            primaryParticles.Add(wheelParticles.GetChild(1).GetChild(i).GetComponent<ParticleSystem>());
        }

        foreach (ParticleSystem p in flashParticles.GetComponentsInChildren<ParticleSystem>())
        {
            secondaryParticles.Add(p);
        }

        booster1Effects = kartModel.Find("Booster1").GetComponentsInChildren<VisualEffect>();
        booster2Effects = kartModel.Find("Booster2").GetComponentsInChildren<VisualEffect>();

        foreach (VisualEffect v in booster1Effects)
        {
            v.Stop();
        }

        foreach (VisualEffect v in booster2Effects)
        {
            v.Stop();
        }
    }

    void Update()
    {
        // Follow Collider
        transform.position = sphere.transform.position - new Vector3(0, follow, 0);

        _Input();
        UpdateSpeedAudio(currentSpeed);

        if (drifting)
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(directionInput.x, -1, 1, 0, 2)
                                                  : ExtensionMethods.Remap(directionInput.x, -1, 1, 2, 0);
            float powerControl = (driftDirection == 1) ? ExtensionMethods.Remap(directionInput.x, -1, 1, .2f, 1)
                                                       : ExtensionMethods.Remap(directionInput.x, -1, 1, 1, .2f);
            Steer(driftDirection, control);
            driftPower += powerControl;

            ColorDrift();
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f);
        speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        rotate = 0f;

        //if (currentSpeed > 0 && currentSpeed < 10 && !isPeak)
        //{
        //    print("Accelerate");
        //    AudioManager.Instance.StopSFX();
        //    AudioManager.Instance.PlaySFX(6);
        //}
        //else if (currentSpeed > 15 && currentSpeed < 40)
        //{
        //    print("Move");
        //    AudioManager.Instance.StopSFX();
        //    AudioManager.Instance.PlaySFX(7);
        //}
        //else if (currentSpeed > 30 && !isPeak)
        //{
        //    isPeak = true;
        //}
        //else if (currentSpeed < 38 && isPeak)
        //{
        //    print("Slow down");
        //    AudioManager.Instance.StopSFX();
        //    AudioManager.Instance.PlaySFX(8);
        //    isPeak = false;
        //}

        //a) KartType
        if (!drifting)
        {
            anim.SetBool("DriftKanan", false);
            anim.SetBool("DriftKiri", false);

            kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles, new Vector3(0, 90 + (directionInput.x * 15), kartModel.localEulerAngles.z), .2f);
        }
        else
        {
            if (driftDirection == 1)
                anim.SetBool("DriftKanan", true);
            else
                anim.SetBool("DriftKiri", true);
            AudioManager.Instance.PlaySFX(2);

            float control = (driftDirection == 1) ? ExtensionMethods.Remap(directionInput.x, -1, 1, .5f, 2) : ExtensionMethods.Remap(directionInput.x, -1, 1, 2, .5f);
            kartModel.parent.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(kartModel.parent.localEulerAngles.y, (control * 15) * driftDirection, .2f), 0);
        }
    }

    private void FixedUpdate()
    {
        if (!drifting)
            sphere.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);
        else
        {
            currentSpeed *= 0.8f;
            ApplyDriftAssist();
            sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);
        }

        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
                                             new Vector3(0, transform.eulerAngles.y + currentRotate, 0),
        Time.deltaTime * 5f);

        RaycastHit hitOn, hitNear;

        bool hitGround1 = Physics.Raycast(transform.position + (transform.up * 0.1f), Vector3.down, out hitOn, 1.1f, layerMask);
        bool hitGround2 = Physics.Raycast(transform.position + (transform.up * 0.1f), Vector3.down, out hitNear, 2.0f, layerMask);

        if (!hitGround1 || !hitGround2)
        {
            Debug.LogWarning("Raycast tidak mendeteksi tanah! Pastikan ada collider dan layerMask benar.");
        }
        else
        {
            isGround = true;
            
            Debug.DrawRay(hitNear.point, hitNear.normal * 2f, Color.green, 0.1f); // Visualisasi normal

            kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.fixedDeltaTime * 8f);
            kartNormal.Rotate(0, transform.eulerAngles.y, 0);
        }
    }

    void _Input()
    {
        if (canMove)
        {
            // Input arah gerak
            directionInput = _inputProcessor.GetMovementInput();

            // Input untuk penggerak (akselerasi/rem)
            //anim.SetTrigger("Move");
            float throttleInput = KartInputProcessor.GetFloatInput(_inputProcessor.GetAction("Throttle"));
            //AudioManager.Instance.PlaySFX(0);

            // Mengatur arah gerak
            if (directionInput.x != 0)
            {
                // Mengubah arah objek berdasarkan input arah
                int dir = directionInput.x > 0 ? 1 : -1;
                float amount = Mathf.Abs(directionInput.x);
                Steer(dir, amount);
            }

            // Mengatur kecepatan berdasarkan input gerak
            speed = acceleration * throttleInput;
            anim.SetInteger("Speed", (int)speed);

            // Drift
            if (KartInputProcessor.GetActionPressed(_inputProcessor.GetAction("Drift")) && !drifting && directionInput.x != 0)
            {
                drifting = true;
                driftDirection = directionInput.x > 0 ? 1 : -1;

                foreach (ParticleSystem p in primaryParticles)
                {
                    var mainModule = p.main;
                    mainModule.startColor = Color.clear;
                    p.Play();
                }

                kartModel.parent.DOComplete(); // DOTween untuk animasi
            }

            if (KartInputProcessor.GetActionReleased(_inputProcessor.GetAction("Drift")) && drifting)
            {


                Boost();
            }

            if (KartInputProcessor.GetActionPressed(_inputProcessor.GetAction("UseItem")) && playerItemHandler.currentItem.HasValue)
            {
                print("Pake Item");
                playerItemHandler.ApplyItem();
            }       
        }
    }

    // Method untuk handle audio state
    void UpdateSpeedAudio(float currentSpeed)
    {
        // State Accelerating (0-10 speed)
        if (currentSpeed > 0 && currentSpeed <= 10)
        {
            if (!isAccelerating)
            {
                AudioManager.Instance.StopSFX();
                AudioManager.Instance.PlaySFX(6); // Accelerate sound
                isAccelerating = true;
                isMove = false;
            }
        }
        // State Moving (>10 speed)
        else if (currentSpeed > 10 && currentSpeed <= 40)
        {
            if (!isMove)
            {
                AudioManager.Instance.StopSFX();
                AudioManager.Instance.PlaySFX(7); // Moving sound
                isAccelerating = false;
                isMove = true;
            }
        }
        // State Peak (>30 speed)
        else if (currentSpeed > 30)
        {
            isPeak = true;
            isAccelerating = false;
            isMove = false;
        }
        // State Decelerate (when speed drops after peak)
        else if (currentSpeed < 30 && isPeak)
        {
            AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlaySFX(8); // Decelerate sound
            isPeak = false;
            isAccelerating = false;
            isMove = false;
        }
    }

    private void ApplyDriftAssist()
    {
        if (drifting)
        {
            float driftAngle = Vector3.Angle(transform.forward, sphere.velocity);
            if (driftAngle > 15f) //Adjust the angle threshold as needed
            {
                float assistForce = Mathf.Clamp(0.5f * (driftAngle / 90f), 0f, 1f);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
                    new Vector3(0, transform.eulerAngles.y + assistForce * steering, 0),
                    Time.fixedDeltaTime * 4f);
            }
        }
    }

    public void Boost()
    {
        //AudioManager.Instance.PlaySFX(1);
        drifting = false;

        if (driftMode > 0)
        {
            DOVirtual.Float(currentSpeed * 3, currentSpeed, .3f * driftMode, Speed);
            //DOVirtual.Float(0, 1, .5f, ChromaticAmount).OnComplete(() => DOVirtual.Float(1, 0, .5f, ChromaticAmount));

            AudioManager.Instance.PlaySFX(3);
            print("Boost");
            kartModel.Find("Booster1").GetComponentInChildren<ParticleSystem>().Play();
            kartModel.Find("Booster2").GetComponentInChildren<ParticleSystem>().Play();
            PlayBoosterEffect(.5f * driftMode);
        }

        driftPower = 0;
        driftMode = 0;
        first = false;
        second = false;
        third = false;
        AudioManager.Instance.PlaySFX(4);

        foreach (ParticleSystem p in primaryParticles)
        {
            var mainModule = p.main;
            mainModule.startColor = Color.clear;
            p.Stop();
        }

        kartModel.parent.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutBack);
    }

    public void PlayBoosterEffect(float duration)
    {
        //Play the booster effect
        foreach (VisualEffect v in booster1Effects)
        {
            v.Play();
        }
        foreach (VisualEffect v in booster2Effects)
        {
            v.Play();
        }

        //Stop the effect after the duration
        DOVirtual.DelayedCall(duration, () =>
        {
            foreach (VisualEffect v in booster1Effects)
            {
                v.Stop();
            }
            foreach (VisualEffect v in booster2Effects)
            {
                v.Stop();
            }
        });
    }

    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    public void ColorDrift()
    {
        if (!first) c = Color.clear;

        if (driftPower > 50 && driftPower < 99 && !first)
        {
            first = true;
            c = turboColors[0];
            driftMode = 1;
            PlayFlashParticle(c);
        }

        if (driftPower > 100 && driftPower < 149 && !second)
        {
            second = true;
            c = turboColors[1];
            driftMode = 2;
            PlayFlashParticle(c);
        }

        if (driftPower > 150 && driftPower < 199 && !third)
        {
            third = true;
            c = turboColors[2];
            driftMode = 3;
            PlayFlashParticle(c);
        }

        if (driftPower > 200 && !fourth)
        {
            fourth = true;
            c = turboColors[3];
            driftMode = 4;
            PlayFlashParticle(c);
        }

        foreach (ParticleSystem p in primaryParticles)
        {
            var mainModule = p.main;
            mainModule.startColor = c;
        }

        foreach (ParticleSystem p in secondaryParticles)
        {
            var mainModule = p.main;
            mainModule.startColor = c;
        }
    }

    void PlayFlashParticle(Color c)
    {
        //GameObject.Find("CM vcam1").GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        foreach (ParticleSystem p in secondaryParticles)
        {
            var mainModule = p.main;
            mainModule.startColor = c;
            p.Play();
        }
    }

    private void Speed(float x)
    {
        currentSpeed = x;
    }

    //void ChromaticAmount(float x)
    //{
    //    postProfile.GetSetting<ChromaticAberration>().intensity.value = x;
    //}

    public void BoostExternal()
    {
        driftMode |= 1;
        Boost();
    }

    public void Die(int dieDuration)
    {
        if (!shieldActive)
        {
            canMove = false;
            StartCoroutine(BackToNormal(dieDuration));
        }
    }

    private IEnumerator BackToNormal(int dieDuration)
    {
        yield return new WaitForSeconds(dieDuration);

        canMove = true;
    }

    private void OnDrawGizmos()
    {
        // Raycast 1 visualization
        Gizmos.color = Color.red; // Color for hitGround1
        Vector3 start1 = transform.position + (transform.up * 0.1f);
        Vector3 end1 = start1 + Vector3.down * 1.1f;
        Gizmos.DrawLine(start1, end1);
        Gizmos.DrawSphere(end1, 0.05f); // Small sphere at the end of the ray

        // Raycast 2 visualization
        Gizmos.color = Color.blue; // Color for hitGround2
        Vector3 start2 = transform.position + (transform.up * 0.1f);
        Vector3 end2 = start2 + Vector3.down * 2.0f;
        Gizmos.DrawLine(start2, end2);
        Gizmos.DrawSphere(end2, 0.05f); // Small sphere at the
    }
}
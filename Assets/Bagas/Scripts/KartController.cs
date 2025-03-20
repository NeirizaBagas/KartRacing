using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityEngine.Rendering.PostProcessing;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class KartController : MonoBehaviour
{
    //private PostProcessVolume postVolume;
    //private PostProcessProfile postProfile;

    float speed, currentSpeed;
    float rotate, currentRotate;
    int driftDirection;
    float driftPower;
    bool first, second, third, fourth;
    Color c;

    [Header("Player")]
    [SerializeField] private PlayerInput playerInput;
    private Vector2 moveInput; // Nyimpen input gerakan
    Vector2 directionInput;

    [Header("Model")]
    public Transform kartModel;
    public Transform kartNormal;
    public Transform frontWheels;
    public Transform backWheels;
    public Transform steeringWheel;
    public Rigidbody sphere;

    [Header("Bools")]
    public bool drifting;
    public bool boosting;
    public bool canMove;
    public bool shieldActive;

    [Header("Parameters")]
    public int driftMode = 0;
    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;
    public int dieDuration;

    [Header("Particles")]
    public List<ParticleSystem> primaryParticles = new List<ParticleSystem>();
    public List<ParticleSystem> secondaryParticles = new List<ParticleSystem>();
    public Transform wheelParticles;
    public Transform flashParticles;
    public Color[] turboColors;

    public PlayerItemHandler playerItemHandler;
    //public Animator anim;

    void Start()
    {
        //postVolume = Camera.main.GetComponent<PostProcessVolume>(); // Poss Process
        //postProfile = postVolume.profile;
        canMove = true;

        playerInput = GetComponent<PlayerInput>();
        playerItemHandler = GetComponent<PlayerItemHandler>();

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
    }

    void Update()
    {
        // Follow Collider
        transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);

        _Input();

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

        //a) Kart
        if (!drifting)
        {
            kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles, new Vector3(0, 90 + (directionInput.x * 15), kartModel.localEulerAngles.z), .2f);
        }
        else
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(directionInput.x, -1, 1, .5f, 2) : ExtensionMethods.Remap(directionInput.x, -1, 1, 2, .5f);
            kartModel.parent.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(kartModel.parent.localEulerAngles.y, (control * 15) * driftDirection, .2f), 0);
        }

        //b) Wheels
        frontWheels.localEulerAngles = new Vector3(0, (directionInput.x * 15), frontWheels.localEulerAngles.z);
        frontWheels.localEulerAngles += new Vector3(sphere.velocity.magnitude / 2, 0, 0);
        backWheels.localEulerAngles += new Vector3(sphere.velocity.magnitude / 2, 0, 0);

        //c) Steering Wheel
        steeringWheel.localEulerAngles = new Vector3(-25, 90, (directionInput.x * 45));
        //boostBar.value = driftMode;
    }

    private void FixedUpdate()
    {
        if (!drifting)
            sphere.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);
        else
            sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
                                             new Vector3(0, transform.eulerAngles.y + currentRotate, 0),
                                             Time.deltaTime * 5f);

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f, layerMask);

        //Normal Rotation
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);
    }

    void _Input()
    {
        if (canMove)
        {
            // Input arah gerak
            directionInput = this.playerInput.actions["Move"].ReadValue<Vector2>(); // Mengambil input arah

            // Input untuk penggerak (akselerasi/rem)
            //anim.SetTrigger("Move");
            float throttleInput = playerInput.actions["Throttle"].ReadValue<float>(); // Mengambil input akselerasi/mundur
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

            // Drift
            if (playerInput.actions["Drift"].WasPressedThisFrame() && !drifting && directionInput.x != 0)
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

            if (playerInput.actions["Drift"].WasReleasedThisFrame() && drifting)
            {
                Boost();
            }

            if (playerInput.actions["UseItem"].WasPressedThisFrame() && playerItemHandler.currentItem.HasValue)
                playerItemHandler.ApplyItem();
        }
        

    }

    public void Boost()
    {
        //AudioManager.Instance.PlaySFX(1);
        drifting = false;

        if (driftMode > 1)
        {
            DOVirtual.Float(currentSpeed * 3, currentSpeed, .3f * driftMode, Speed);
            //DOVirtual.Float(0, 1, .5f, ChromaticAmount).OnComplete(() => DOVirtual.Float(1, 0, .5f, ChromaticAmount));

            kartModel.Find("Tube001").GetComponentInChildren<ParticleSystem>().Play();
            kartModel.Find("Tube002").GetComponentInChildren<ParticleSystem>().Play();
        }

        driftPower = 0;
        driftMode = 0;
        first = false;
        second = false;
        third = false;

        foreach (ParticleSystem p in primaryParticles)
        {
            var mainModule = p.main;
            mainModule.startColor = Color.clear;
            p.Stop();
        }

        kartModel.parent.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutBack);
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
        driftMode |= 2;
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
}

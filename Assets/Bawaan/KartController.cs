using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityEngine.Rendering.PostProcessing;
using Cinemachine;
using UnityEngine.UI;

public class KartController : MonoBehaviour
{
    //private PostProcessVolume postVolume;
    //private PostProcessProfile postProfile;

    float speed, currentSpeed;
    float rotate, currentRotate;
    int driftDirection;
    float driftPower;
    int driftMode = 0;
    bool first, second, third, fourth;
    Color c;

    [Header("Model")]
    public Transform kartModel;
    public Transform kartNormal;
    public Transform frontWheels;
    public Transform backWheels;
    public Transform steeringWheel;
    public Rigidbody sphere;

    [Header("Item")]
    public GameObject trapItem;
    public Transform dropLocation;
    private ItemEffect? currenItem = null; // Menyimpan efek item yang diambil
    public int itemDuration = 3;

    [Header("UI")]
    //public Slider boostBar;
    public Image effectUi; // UI Image di canvas
    public Sprite shieldIcon, trapIcon, boostIcon; // Ikon untuk UI

    [Header("Bools")]
    public bool drifting;
    public bool boosting;

    [Header("Parameters")]
    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;


    [Header("Particles")]
    public List<ParticleSystem> primaryParticles = new List<ParticleSystem>();
    public List<ParticleSystem> secondaryParticles = new List<ParticleSystem>();
    public Transform wheelParticles;
    public Transform flashParticles;
    public Color[] turboColors;


    void Start()
    {
        //postVolume = Camera.main.GetComponent<PostProcessVolume>(); // Poss Process
        //postProfile = postVolume.profile;

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
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 0, 2)
                                                  : ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 2, 0);
            float powerControl = (driftDirection == 1) ? ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, .2f, 1)
                                                       : ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 1, .2f);
            Steer(driftDirection, control);
            driftPower += powerControl;

            ColorDrift();
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f);
        speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        rotate = 0f;
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
    }

    void _Input()
    {
        float input = Input.GetAxis("Vertical"); // Menentukan arah gerak

        // Accelerate
        if (Input.GetButton("Vertical"))
            speed = acceleration * input;

        // Steer
        if (Input.GetAxis("Horizontal") != 0)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs(Input.GetAxis("Horizontal"));
            Steer(dir, amount);
        }

        // Drift
        if (Input.GetKeyDown(KeyCode.LeftShift) && !drifting && Input.GetAxis("Horizontal") != 0)
        {
            drifting = true;
            driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;

            foreach (ParticleSystem p in primaryParticles)
            {
                var mainModule = p.main;
                mainModule.startColor = Color.clear;
                p.Play();
            }

            kartModel.parent.DOComplete(); //DoTween
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && drifting)
        {
            Boost();
        }

        if (Input.GetKeyDown(KeyCode.Space) && currenItem != null)
        {
            ApplyItem();
        }
    }

    public void Boost()
    {
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

        if (driftPower > 0 && driftPower < 49 && !first)
        {
            first = true;
            c = turboColors[0];
            driftMode = 1;
            PlayFlashParticle(c);
        }

        if (driftPower > 50 && driftPower < 99 && !second)
        {
            second = true;
            c = turboColors[1];
            driftMode = 2;
            PlayFlashParticle(c);
        }

        if (driftPower > 100 && driftPower < 149 && !third)
        {
            third = true;
            c = turboColors[2];
            driftMode = 3;
            PlayFlashParticle(c);
        }

        if (driftPower > 150 && !fourth)
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
        GameObject.Find("CM vcam1").GetComponent<CinemachineImpulseSource>().GenerateImpulse();

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

    public void PickItem(ItemEffect item)
    {
        if (currenItem == null)
        {
            currenItem = item; // Simpan efek item
            UpdateUi(item);
        }
    }

    public void ApplyItem()
    {
        if (currenItem == null) return;

        switch (currenItem)
        {
            case ItemEffect.Shield:
                print("Menggunakan Shield");
                effectUi.color = Color.blue;
                ItemShield();
                break;
            case ItemEffect.Trap:
                print("Menggunakan Trap");
                effectUi.color = Color.red;
                ItemTrap();
                break;
            case ItemEffect.Boost:
                print("Menggunakan Boost");
                effectUi.color = Color.green;
                ItemBoost();
                break;
        }

        effectUi.gameObject.SetActive(true);
        StartCoroutine(HideEffectAfterDelay());
        currenItem = null; // Hapus item setelah digunakan
    }

    void UpdateUi(ItemEffect item)
    {
        switch (item)
        {
            case ItemEffect.Shield:
                effectUi.sprite = shieldIcon;
                effectUi.color = Color.blue;
                break;
            case ItemEffect.Trap:
                effectUi.sprite = trapIcon;
                effectUi.color = Color.red;
                break;
            case ItemEffect.Boost:
                effectUi.sprite = boostIcon;
                effectUi.color = Color.green;
                break;
        }

        effectUi.gameObject.SetActive(true);
    }

    IEnumerator HideEffectAfterDelay()
    {
        yield return new WaitForSeconds(itemDuration);
        effectUi.gameObject.SetActive(false);
    }

    void ItemTrap()
    {
        Instantiate(trapItem, dropLocation.transform.position, Quaternion.identity);
    }

    void ItemBoost()
    {
        driftMode |= 2;
        Boost();
    }

    void ItemShield()
    {
        //SHIELD
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartControl : MonoBehaviour
{
    [Header("Objek")]
    public Rigidbody sphere; // Collider utama kart berbentuk bola
    public Transform kartModel; // Model kart yang bisa berputar/miring

    [Header("Gerak Lurus")]
    public float acceleration = 30f; // Kecepatan akselerasi
    public float gravity = 10f; // Gaya gravitasi
    private float currentSpeed; // Kecepatan saat ini

    [Header("Gerak Kanan Kiri")]
    public float steering = 80f; // Kekuatan kemudi
    private float rotate; // Nilai rotasi saat ini

    [Header("Drift")]
    private bool drifting = false; // Apakah kart sedang drifting?
    private int driftDirection; // Arah drift (-1 = kiri, 1 = kanan)
    private float driftPower; // Kekuatan drift yang terakumulasi

    [Header("Skill")]
    public GameObject trapItem; // Prefab jebakan
    public Transform dropLocation; // Lokasi tempat item jatuh

    public List<ParticleSystem> driftParticles = new List<ParticleSystem>();

    private void Start()
    {
        if (driftParticles != null)
        {
            // Mengambil semua partikel dari objek anak
            foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
            {
                driftParticles.Add(p);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Pastikan model mengikuti posisi collider utam (sphere)
        transform.position = sphere.transform.position;

        Move();
        Drift();
    }

    private void FixedUpdate()
    {
        // Gerakkan sphere ke depan
        sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration); // Menjaga kart tetap di tanah

        // Menerapkan rotasi secara bertahap agar tidak langsung berbelok tajam
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + rotate, 0), Time.deltaTime * 5f);
    }

    void Move()
    {
        // Pergerakan maju ketika tombol panah atas ditekan
        if (Input.GetButton("Vertical"))
        {
            currentSpeed = acceleration;
        }
        else
        {
            currentSpeed = 0;
        }

        // Mengubah arah sesuai input horizontal
        if (Input.GetAxis("Horizontal") != 0)
        {
            int direction = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs(Input.GetAxis("Horizontal"));
            Steer(direction, amount);
        }
    }

    void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    void Drift()
    {
        // Memeriksa apakah tombol drift ditekan
        if (Input.GetKeyDown(KeyCode.LeftShift) && !drifting && Input.GetAxis("Horizontal") != 0)
        {
            drifting = true;
            driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
        }

        if (drifting)
        {
            // Meningkatkan kekuatan drift jika masih menekan tombol
            driftPower += Time.deltaTime * 50;
            foreach (ParticleSystem p in driftParticles)
            {
                p.Play(); // Menyalakan partikel saat drifting
            }
        }
        else
        {
            foreach (ParticleSystem p in driftParticles)
            {
                p.Stop(); // Mematikan partikel jika tidak drifting
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && drifting)
        {
            Boost();
        }
    }

    void Boost()
    {
        drifting = false;

        if (driftPower > 50)
        {
            currentSpeed *= 2; // Boost dua kali kecepatan normal
        }

        driftPower = 0; // Reset drift power
    }

    void Trap()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DropTrap();
        }
    }

    void DropTrap()
    {
        Instantiate(trapItem, dropLocation.position, Quaternion.identity);
    }
}

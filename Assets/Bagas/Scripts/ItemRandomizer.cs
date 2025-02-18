using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ItemEffect { Shield, Trap, Boost } // Enum efek item

public class ItemRandomizer : MonoBehaviour
{
    public Sprite shieldIcon, trapIcon, boostIcon; // Ikon efek di UI
    public KartController kartController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyRandomEffect();
            Destroy(gameObject); // Hancurkan item setelah diambil
        }
    }

    void ApplyRandomEffect()
    {
        ItemEffect randomEffect = (ItemEffect)Random.Range(0, 3);
        Debug.Log("Item yang diperoleh: " + randomEffect); // Debug log untuk melihat item yang didapat
        kartController.PickItem(randomEffect); // Simpan item di kartcontroller
    }
}
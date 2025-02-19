using System.Collections;
using UnityEngine;

public enum ItemEffect { Shield, Trap, Boost }

public class ItemRandomizer : MonoBehaviour
{
    public Sprite shieldIcon, trapIcon, boostIcon;
    public int itemCooldown;

    private void OnTriggerEnter(Collider other)
    {
        KartController playerKart = other.transform.root.GetComponentInChildren<KartController>();
        if (playerKart != null)
        {
            ApplyRandomEffect(playerKart);
            gameObject.SetActive(false);
            GameManager.Instance.StartCoroutine(RespawnItem()); // Panggil dari GameManager
        }
    }

    void ApplyRandomEffect(KartController kartController)
    {
        ItemEffect randomEffect = (ItemEffect)Random.Range(0, 3);
        Debug.Log(kartController.name + " mendapatkan item: " + randomEffect);
        kartController.PickItem(randomEffect);
    }

    IEnumerator RespawnItem()
    {
        yield return new WaitForSeconds(itemCooldown);
        gameObject.SetActive(true);
    }
}

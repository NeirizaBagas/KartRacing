using System.Collections;
using UnityEngine;

public enum ItemEffect { Shield, Boost, StickyNote, Stabilo, Sharpener, TipxMekanik, Pita, PaperStorm, GlueSpill, Strapler, SlipperyWater, Pin }

public class ItemRandomizer : MonoBehaviour
{
    public Sprite shieldIcon, trapIcon, boostIcon;
    public int itemCooldown;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Yang nabrak: " + other.gameObject.name);
        PlayerItemHandler playerItem = other.transform.parent.GetComponentInChildren<PlayerItemHandler>();
        playerItem.hasItem = true;
        if (playerItem != null && !playerItem.HasItem()) // Memastikan tidak menimpa item yang ada
        {
            playerItem.Gacha();
            ApplyRandomEffect(playerItem);
            gameObject.SetActive(false);
            RaceManager.Instance.StartCoroutine(RespawnItem()); 
        }
    }

    void ApplyRandomEffect(PlayerItemHandler playerItem)
    {
        //AudioManager.Instance.PlaySFX(10);
        ItemEffect randomEffect = (ItemEffect)Random.Range(0, System.Enum.GetValues(typeof(ItemEffect)).Length);
        AudioManager.Instance.StopSFX();
        //AudioManager.Instance.PlaySFX(11);
        Debug.Log(playerItem.name + " mendapatkan item: " + randomEffect);
        playerItem.PickItem(randomEffect);
    }

    IEnumerator RespawnItem()
    {
        yield return new WaitForSeconds(itemCooldown);
        gameObject.SetActive(true);
    }
}

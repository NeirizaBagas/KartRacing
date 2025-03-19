using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemHandler : MonoBehaviour
{
    public Image effectUi;
    public Sprite shieldIcon, trapIcon, boostIcon;
    public GameObject trapItem;
    public Transform dropLocation;
    public int itemDuration = 3;
    public int shieldDuration;
    public bool hasItem;

    public ItemEffect? currentItem = null;

    private KartController kartController;

    private void Start()
    {
        kartController = GetComponent<KartController>();
    }

    public bool HasItem()
    {
        return currentItem.HasValue;
    }

    public void PickItem(ItemEffect item)
    {
        if (!currentItem.HasValue)
        {
            currentItem = item;
            UpdateItemUI(item);
        }
    }

    public void ApplyItem()
    {
        if (!currentItem.HasValue) return;

        switch (currentItem.Value)
        {
            case ItemEffect.Shield:
                ActivateShield();
                break;
            case ItemEffect.Trap:
                DropTrap();
                break;
            case ItemEffect.Boost:
                ActivateBoost();
                break;
        }

        effectUi.gameObject.SetActive(true);
        StartCoroutine(HideEffectAfterDelay());
        currentItem = null;
    }

    private void UpdateItemUI(ItemEffect item)
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

    private IEnumerator HideEffectAfterDelay()
    {
        yield return new WaitForSeconds(itemDuration);
        effectUi.gameObject.SetActive(false);
    }


    private void DropTrap() { Instantiate(trapItem, dropLocation.position, Quaternion.identity); }
    private void ActivateBoost()
    {
        kartController.driftMode = 2;
        kartController.Boost();
    }

    private void ActivateShield()
    {
        kartController.shieldActive = true;
    }

    private IEnumerator UnactivateShield()
    {
        yield return new WaitForSeconds(shieldDuration);
        kartController.shieldActive = false;
    }
}

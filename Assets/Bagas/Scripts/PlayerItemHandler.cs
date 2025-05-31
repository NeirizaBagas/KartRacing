using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemHandler : MonoBehaviour
{
    public Image itemUi;
    [SerializeField] private Animator itemAnim;
    public Sprite shieldIcon, trapIcon, boostIcon;
    public Transform dropLocation;
    public int itemDuration = 3;
    public bool hasItem;
    public ItemEffect? currentItem = null;
    public KartMover kartMover;
    private KartItemEffect kartItemEffect;

    [Header("Shield properties")]

    [Header("Trap properties")]
    public int shieldDuration;
    public GameObject[] trapItem;

    //[Header("Boost properties")]

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void Gacha()
    {
        print("Gacha");
        itemAnim.SetTrigger("Gacha");
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
            case ItemEffect.Boost:
                ActivateBoost();
                break;
            case ItemEffect.StickyNote:
                break;
            case ItemEffect.TipxMekanik:
                // Implement TipxMekanik effect
                break;
            case ItemEffect.Stabilo:
                // Implement Stabilo effect
                break;
            case ItemEffect.Sharpener:
                // Implement Sharpener effect
                break;
            case ItemEffect.Pita:
                // Implement Pita effect
                break;
            case ItemEffect.PaperStorm:
                DropPaperStorm();
                break;
            case ItemEffect.GlueSpill:
                DropGlueSpill();
                break;
            case ItemEffect.Strapler:
                DropStrapler();
                break;
            case ItemEffect.SlipperyWater:
                DropSlipperyWater();
                break;
            case ItemEffect.Pin:
                DropPin();
                break;

        }

        //itemUi.gameObject.SetActive(true);
        StartCoroutine(HideEffectAfterDelay());
        currentItem = null;
    }

    private void UpdateItemUI(ItemEffect item)
    {
        switch (item)
        {
            case ItemEffect.Shield:
                //itemUi.sprite = shieldIcon;
                itemUi.color = Color.blue;
                break;
            case ItemEffect.Boost:
                //itemUi.sprite = boostIcon;
                itemUi.color = Color.green;
                break;
            case ItemEffect.StickyNote:
                //itemUi.sprite = stickyNoteIcon;
                itemUi.color = Color.blue;
                break;
            case ItemEffect.Stabilo:
                //itemUi.sprite = stabiloIcon;
                itemUi.color = Color.green;
                break;
            case ItemEffect.Sharpener:
                //itemUi.sprite = sharpenerIcon;
                itemUi.color = Color.green;
                break;
            case ItemEffect.TipxMekanik:
                //itemUi.sprite = tipxMekanikIcon;
                itemUi.color = Color.blue;
                break;
            case ItemEffect.Pita:
                //itemUi.sprite = pitaIcon;
                itemUi.color = Color.green;
                break;
            case ItemEffect.PaperStorm:
                //itemUi.sprite = paperStormIcon;
                itemUi.color = Color.red;
                break;
            case ItemEffect.GlueSpill:
                //itemUi.sprite = glueSpillIcon;
                itemUi.color = Color.red;
                break;
            case ItemEffect.Strapler:
                //itemUi.sprite = straplerIcon;
                itemUi.color = Color.red;
                break;
            case ItemEffect.SlipperyWater:
                //itemUi.sprite = slipperyWaterIcon;
                itemUi.color = Color.red;
                break;
            case ItemEffect.Pin:
                //itemUi.sprite = pinIcon;
                itemUi.color = Color.red;
                break;
        }

        itemUi.gameObject.SetActive(true);
    }

    private IEnumerator HideEffectAfterDelay()
    {
        yield return new WaitForSeconds(itemDuration);
        itemUi.gameObject.SetActive(false);
    }


    private void DropPaperStorm()
    {
        Instantiate(trapItem[0], dropLocation.position, Quaternion.identity);
    }

    private void DropGlueSpill()
    {
        Instantiate(trapItem[1], dropLocation.position, Quaternion.identity);
    }

    private void DropStrapler()
    {
        Instantiate(trapItem[2], dropLocation.position, Quaternion.identity);
    }

    private void DropSlipperyWater()
    {
        Instantiate(trapItem[3], dropLocation.position, Quaternion.identity);
    }

    private void DropPin()
    {
        Instantiate(trapItem[4], dropLocation.position, Quaternion.identity);
    }

    private void ActivateBoost()
    {
        kartMover.driftMode = 2;
        kartMover.Boost();
    }

    private void ActivateShield()
    {
        kartMover.shieldActive = true;
        StartCoroutine(UnactivateShield());
    }

    private IEnumerator UnactivateShield()
    {
        yield return new WaitForSeconds(shieldDuration);
        kartMover.shieldActive = false;
    }
}

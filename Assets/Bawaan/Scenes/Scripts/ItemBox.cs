using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public ItemEffect[] possibleItems;

    private void OnTriggerEnter(Collider other)
    {
        PlayerItemHandler playerItem = other.GetComponent<PlayerItemHandler>();

        if (playerItem != null && !playerItem.HasItem())
        {
            ItemEffect randomItem = possibleItems[Random.Range(0, possibleItems.Length)];
            playerItem.PickItem(randomItem);
            Destroy(gameObject);
        }
    }
}

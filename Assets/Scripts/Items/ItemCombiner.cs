using UnityEngine;

public class ItemCombiner : MonoBehaviour
{
    [Header("Combination Settings")]
    [Tooltip("Unique identifier for this item.")]
    public string itemId;
    [Tooltip("Unique identifier for the item to combine with.")]
    public string combineWithItemId;
    [Tooltip("Prefab of the new item created when combining.")]
    public GameObject combinedItemPrefab;

    private bool hasCombined = false; // Boolean flag to track combination
    private bool playerInTrigger = false; // Boolean flag to track if player is in trigger
    private ItemCombiner otherItemCombiner; // Reference to the other ItemCombiner

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCombined) return; // Exit if already combined

        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
        else
        {
            ItemCombiner itemCombiner = other.GetComponent<ItemCombiner>();
            if (itemCombiner != null)
            {
                otherItemCombiner = itemCombiner;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
        else if (other.GetComponent<ItemCombiner>() != null)
        {
            otherItemCombiner = null;
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && otherItemCombiner != null && !hasCombined)
        {
            // Simplified condition to check if the other object has the correct identifier for combination
            if ((itemId == otherItemCombiner.combineWithItemId && combineWithItemId == otherItemCombiner.itemId) ||
                (itemId == otherItemCombiner.itemId && combineWithItemId == otherItemCombiner.combineWithItemId))
            {
               

                // Instantiate the combined item at the midpoint between the two items
                Vector3 combinedPosition = (transform.position + otherItemCombiner.transform.position) / 2;
                Instantiate(combinedItemPrefab, combinedPosition, Quaternion.identity);

                // Set the flag to true to prevent further combinations
                hasCombined = true;
                otherItemCombiner.hasCombined = true;

                // Destroy both original items
                Destroy(otherItemCombiner.gameObject);
                Destroy(gameObject);
            }
            else
            {
            }
        }
    }
}

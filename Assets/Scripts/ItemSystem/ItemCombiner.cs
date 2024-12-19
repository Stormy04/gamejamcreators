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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCombined) return; // Exit if already combined

        Debug.Log("Collision detected with: " + other.name);

        ItemCombiner otherItemCombiner = other.GetComponent<ItemCombiner>();
        if (otherItemCombiner != null)
        {
            Debug.Log("Other object has ItemCombiner component.");
            Debug.Log("This itemId: " + itemId + ", This combineWithItemId: " + combineWithItemId);
            Debug.Log("Other itemId: " + otherItemCombiner.itemId + ", Other combineWithItemId: " + otherItemCombiner.combineWithItemId);

            // Simplified condition to check if the other object has the correct identifier for combination
            if ((itemId == otherItemCombiner.combineWithItemId && combineWithItemId == otherItemCombiner.itemId) ||
                (itemId == otherItemCombiner.itemId && combineWithItemId == otherItemCombiner.combineWithItemId))
            {
                Debug.Log("Combination criteria met.");

                // Instantiate the combined item at the midpoint between the two items
                Vector3 combinedPosition = (transform.position + other.transform.position) / 2;
                Instantiate(combinedItemPrefab, combinedPosition, Quaternion.identity);

                // Set the flag to true to prevent further combinations
                hasCombined = true;
                otherItemCombiner.hasCombined = true;

                // Destroy both original items
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Combination criteria not met.");
            }
        }
    }
}

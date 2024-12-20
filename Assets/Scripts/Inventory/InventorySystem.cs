using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryObject> InventoryObjects = new List<InventoryObject>();
    public Transform inventoryParent;
    public InventoryButton buttonPrefab;

    private void OnEnable()
    {
        UpdateInventory();
    }
    public void UpdateInventory()
    {
        if (inventoryParent == null) return;
        for (int i = 0; i < inventoryParent.childCount; i++)
        {
            Destroy(inventoryParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < InventoryObjects.Count; i++)
        {
            InventoryButton button = Instantiate(buttonPrefab, inventoryParent);
            button.Initialize(InventoryObjects[i], this, i);
        }
    }

    public void UseInventoryItem(int itemIndex)
    {
        InventoryObject item = InventoryObjects[itemIndex];
        InventoryObjects.RemoveAt(itemIndex);
        UpdateInventory();
        // place object in world
    }
}

[System.Serializable]
public class InventoryObject
{
    public bool inPossession;
    public GameObject worldItem;
    public Sprite displayImage;

    public InventoryObject(GameObject worldItem, Sprite displayImage)
    {
        this.worldItem = worldItem;
        this.displayImage = displayImage;
    }
}
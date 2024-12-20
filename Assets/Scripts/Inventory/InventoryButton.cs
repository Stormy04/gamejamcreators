using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private InventorySystem inventorySystem;
    private int inventoryIndex;

    public void Initialize(InventoryObject invObject, InventorySystem invSystem, int invIndex)
    {
        inventorySystem = invSystem;
        inventoryIndex = invIndex;
        if (iconImage != null )
            iconImage.sprite = invObject.displayImage;
    }

    public void OnSelectItem()
    {
        inventorySystem.UseInventoryItem(inventoryIndex);
    }
}

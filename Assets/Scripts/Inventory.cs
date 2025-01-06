using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inventory class to manage the inventory UI
// Made with help from this tutorial: https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/#inventory_systems
public class Inventory : MonoBehaviour
{
    // List of key items and lore items
    public List<ItemInfo> KeyItems = new List<ItemInfo>();
    public List<ItemInfo> LoreItems = new List<ItemInfo>();

    public GameObject InventoryUI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Awake()
    {
        checkKeyItems();
        checkLoreItems();
    }

    public void AddItem(ItemInfo item)
    {
        // Check if the item is a key item or a lore item and add it to the appropriate list
        if (item.itemType == ItemInfo.ItemType.Key)
        {
            KeyItems.Add(item);
            print("Key Item Added");
        }
        else if (item.itemType == ItemInfo.ItemType.Lore)
        {
            LoreItems.Add(item);
        }
    }

    public void RemoveItem(ItemInfo item)
    {
        // Check if the item is a key item or a lore item and remove it from the appropriate list
        if (item.itemType == ItemInfo.ItemType.Key)
        {
            KeyItems.Remove(item);
        }
        else if (item.itemType == ItemInfo.ItemType.Lore)
        {
            LoreItems.Remove(item);
        }
    }

    // Debugging function to check key items
    void checkKeyItems()
    {
        foreach (ItemInfo item in KeyItems)
        {
            Debug.Log(item.itemID);
        }
    }

    // Debugging function to check lore items
    void checkLoreItems()
    {
        foreach (ItemInfo item in LoreItems)
        {
            Debug.Log(item.itemID);
        }
    }

    // Function to check if the player has a certain key item
    public bool hasKeyItem(string itemID){
        foreach (ItemInfo item in KeyItems)
        {
            if (item.itemID == itemID)
            {
                return true;
            }
        }
        return false;
    }
}

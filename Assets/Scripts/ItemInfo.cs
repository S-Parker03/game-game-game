using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this scriptable object is used to create items that can be stored in the inventory
//made with help from this tutorial: https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/#inventory_systems

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class ItemInfo : ScriptableObject
{
    // Start is called before the first frame update
    public string itemName;

    [TextArea]
    public string itemDescription;
    public Sprite itemImage;

    public enum ItemType{Key, Lore};
    public ItemType itemType;
    public string itemID;

    public GameObject modelPrefab;

}

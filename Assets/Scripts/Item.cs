using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this scriptable object is used to create items that can be stored in the inventory
//made with help from this tutorial: https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/#inventory_systems

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class ItemInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public bool hasItem;
    public string itemName;

    [TextArea]
    public string itemDescription;
    public Sprite itemImage;

    public enum ItemType{Key, Lore};
    public ItemType itemType;
    public string itemID;

    public GameObject pickupObject;


    public void collect(){
        if (pickupObject.GetComponent<Collider>() != null){
            pickupObject.GetComponent<Collider>().enabled = false;
        }
        if (pickupObject != null){
            pickupObject.SetActive(false);
        }
        hasItem = true;
    }

}

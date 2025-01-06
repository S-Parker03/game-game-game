using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

//this scriptable object is used to create items that can be stored in the inventory
//made with help from this tutorial: https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/#inventory_systems


[System.Serializable]
public class ItemInfo : MonoBehaviour
{

    //define variables for each item
    public bool hasItem;
    public string itemName;

    [TextArea]
    public string itemDescription;
    public Sprite itemImage;

    public enum ItemType{Key, Lore};
    public ItemType itemType;
    public string itemID;

    public GameObject pickupObject;

    
    // collection function to handle physical items
    public void collect(){
        //disable the collider and set the object to inactive
        if (pickupObject.GetComponent<Collider>() != null){
            pickupObject.GetComponent<Collider>().enabled = false;
        }
        if (pickupObject != null){
            pickupObject.SetActive(false);
        }
        //set the hasItem variable to true
        hasItem = true;
    }

}

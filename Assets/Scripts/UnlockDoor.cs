using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject door;
    public List<ItemInfo> requiredItems;

    private bool doorUnlocked = false;
    private Transform playerTransform;

    public GameObject player;

    public GameObject lobbyDoor;

    // Start is called before the first frame update
    void Start()
    {
      //Finding the player and the lobby door, as it behaves differently from other doors
      lobbyDoor = GameObject.Find("Lobby Door");
      player = GameObject.Find("Player");  
    }

    // Update is called once per frame
    void Update()
    {
        //get the player's transform
        playerTransform = player.transform;
    }

    //function to unlock the door
    public void Unlock()
    {
        //if the door is already unlocked, return
        if (doorUnlocked)
        {
            return;
        }

        //check if the player has all the required items
        bool hasAllItems = true;
        foreach (ItemInfo item in requiredItems)
        {
            if (!player.GetComponent<Inventory>().hasKeyItem(item.itemID))
            {
                //if the player is missing an item required to unlock the door, set hasAllItems to false and break the loop
                hasAllItems = false;
                break;
            }
            Debug.Log("Item " + item.itemName + " found");
        }
        //if the player has all the required items, unlock the door
        if (hasAllItems)
        {
            //if the door is the lobby door, set it to inactive
            if( door == lobbyDoor){
                door.SetActive(false);
            }
            //otherwise set the door to unlocked
            doorUnlocked = true;
            door.tag = "Door";
            door.GetComponent<Door>().Open(playerTransform.position);
        }
    }
}

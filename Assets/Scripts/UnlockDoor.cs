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

    public GameObject basement;

    public GameObject roof;


    void Start()
    {
      lobbyDoor = GameObject.Find("Lobby Door");
      player = GameObject.Find("Player");  
    }

    void Update()
    {
        playerTransform = player.transform;
        Unlock();
    }

    // unlock door function
    public void Unlock()
    {
        if (doorUnlocked)
        {
            return;
        }

        bool hasAllItems = true;
        foreach (ItemInfo item in requiredItems)
        {
            if (!player.GetComponent<Inventory>().hasKeyItem(item.itemID))
            {
                hasAllItems = false;
                // Debug.Log("Player does NOT have item: " + item.itemName);

                break;
            }
            Debug.Log("Item " + item.itemName + " found");
        }

        // if all glass pieces are collected hide/unlock door
        if (hasAllItems)
        {
            if( door == lobbyDoor){
                door.SetActive(false);
                basement.SetActive(false);
            }
            basement.SetActive(false);
            doorUnlocked = true;
            door.tag = "Door";
            roof.SetActive(false);
            door.GetComponent<Door>().Open(playerTransform.position);
        }
    }
}

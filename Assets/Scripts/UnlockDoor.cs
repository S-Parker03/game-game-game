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
      lobbyDoor = GameObject.Find("Lobby Door");
      player = GameObject.Find("Player");  
    }

    // Update is called once per frame
    void Update()
    {
        playerTransform = player.transform;
        Unlock();
    }

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

        if (hasAllItems)
        {
            if( door == lobbyDoor){
                door.SetActive(false);
            }
            doorUnlocked = true;
            door.tag = "Door";
            door.GetComponent<Door>().Open(playerTransform.position);
        }
    }
}

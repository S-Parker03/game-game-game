using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject door;
    public List<ItemInfo> requiredItems;

    private bool doorUnlocked = false;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
            if (!item.hasItem)
            {
                hasAllItems = false;
                break;
            }
        }

        if (hasAllItems)
        {
            doorUnlocked = true;
            door.tag = "Door";
            door.GetComponent<Door>().Open(playerTransform.position);
        }
    }
}

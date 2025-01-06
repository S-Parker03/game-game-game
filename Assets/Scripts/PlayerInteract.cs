using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;



// class for checking if the NPC is in range of the player and if key E is pressed
public class PlayerInteract : MonoBehaviour
{

    public Transform cam4ray;
        //Variables to do with Door Interactions\\
    public float MaxUseDistance = 20f;
    public LayerMask UseLayers;
    RaycastHit hit;
    private GameObject playerObj;

    private GameObject lastObject = null;

    [SerializeField]private AudioClip SanityPickUpSound;

    [SerializeField]private AudioClip ItemPickUpSound;



    void Start()
    {
        // Get the player object
        playerObj = GameObject.FindGameObjectWithTag("Player");

    }

    void FixedUpdate()
    {
        //draws a line from the camera to the max use distance
        Debug.DrawLine(cam4ray.position, cam4ray.position + cam4ray.forward * MaxUseDistance, Color.red);
        // code to highlight an interactable object (item, npc or door) when the player is in range
        if (Physics.Raycast(cam4ray.position, cam4ray.forward, out hit, MaxUseDistance))
        {
            if (lastObject != null && lastObject != hit.collider.gameObject)
            {
                // if the last object is not the same as the current object, unhighlight the last object
                highlight(lastObject, false);
            }    
            if (hit.collider.CompareTag("Door") || hit.collider.CompareTag("Item")
            || hit.collider.CompareTag("SanityPickUp") || hit.collider.CompareTag("NPC") || hit.collider.CompareTag("CanBeUnlocked")) 
            {
                // if the object is a door, item, sanity pickup, npc or can be unlocked, highlight the object
                highlight(hit.collider.gameObject, true);
                lastObject = hit.collider.gameObject;
            }
        }
    }

    // Method to use the binding set up in the "Use" action in the input system
    public void OnUse()
    {
        // Use Raycast to detect how far away the player's front is from an object
        if (Physics.Raycast(cam4ray.position, cam4ray.forward, out hit, MaxUseDistance))
        {
            // Draw a ray to show where the player is looking
            Debug.DrawRay(cam4ray.position, cam4ray.forward, Color.green);
            // Get Door collider component and see if it's been hit
            if (hit.collider.TryGetComponent<Door>(out Door door) && hit.collider.CompareTag("Door"))
            {
                // If door is open, then run close method. Otherwise open door with open method
                if (door.isOpen)
                {
                    door.Close();
                }
                else
                {
                    door.Open(playerObj.transform.position);
                }
            }
            // Get ItemInfo component and see if it's been hit
            else if (hit.collider.TryGetComponent<ItemInfo>(out ItemInfo itemInfo))
            {
                Debug.Log("Item hit: " + itemInfo.itemName);
                // Get the ItemInfo component from the hit object and interact with it
                if (itemInfo != null)
                {
                    //sound for item pickup
                    SoundManager.instance.PlayItemPickUpClip(ItemPickUpSound, transform, 1f);
                    // get the player object
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        // get the inventory component of the player
                        Inventory inventory = player.GetComponent<Inventory>();
                        
                        if (inventory != null)
                        {
                            // if the inventory was found add the item to the inventory
                            inventory.AddItem(itemInfo);
                            // collect the item
                            itemInfo.collect();
                            Debug.Log("Item attemepted to add to inventory: " + itemInfo.itemName);
                        }
                        else
                        {
                            Debug.LogError("Player does not have an Inventory component.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Player object not found.");
                    }
                    
                }
            }
            // Get SanityPickUp component and see if it's been hit  
            else if (hit.collider.CompareTag("SanityPickUp")){
                // check for the player object
                if (playerObj != null)
                {
                    // check if the player's sanity is less than 5
                    if (playerObj.GetComponent<PlayerController>().Sanity < 5)
                    {
                        // check if the player's dependency is less than 50
                        if (playerObj.GetComponent<Dependency>().DependencyPercent < 50)
                        {
                            // increase the player's sanity by 1
                            playerObj.GetComponent<PlayerController>().ChangeSanity(1);
                            // increase the player's dependency by 20
                            playerObj.GetComponent<Dependency>().changeDependency(20f);
                            // play the sound for the sanity pickup
                            SoundManager.instance.PlaySanityPickUpClip(SanityPickUpSound, transform, 1f);
                            // destroy the sanity pickup object
                            Destroy(hit.collider.gameObject);
                        }
                        // if the player's deoendency is greater than 50
                        else
                        {
                            // do not increase the player's sanity
                            Destroy(hit.collider.gameObject);
                            // increase the player's dependency by 10
                            playerObj.GetComponent<Dependency>().changeDependency(10f);
                        }
                    }
                    else
                    {
                        // if the player's sanity is full, do not increase the player's sanity
                        Debug.Log("Sanity is full");
                    }
                }
            // Get NPC component and see if it's been hit
            } else if (hit.collider.CompareTag("NPC"))
            {
            
            } else if (hit.collider.CompareTag("CanBeUnlocked"))
            // Get UnlockDoor component and see if it's been hit
            {
                Debug.Log("Unlocking door");
                //Attempt to unlock the door
                hit.collider.GetComponent<UnlockDoor>().Unlock();
            }
        }
    }

    // Method to highlight an object

    public void highlight(GameObject obj, bool toggle)
    {
        // create an outline object
        Outline outline;

        if(toggle == true)
        {
            // if the object does not have an outline, add an outline to the object
            if (obj.GetComponent<Outline>() == null )
            {
                outline = obj.AddComponent<Outline>();
            }
            // if the object already has an outline, get the outline component
            else
            {
                outline = obj.GetComponent<Outline>();
            }
            // set the outline properties
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = new Color(175f / 255f, 0f, 0f);
            outline.OutlineWidth = 20f;
            outline.enabled = true;
        }
        if(toggle == false)
        {
            // if the object has an outline, disable the outline
            outline = obj.GetComponent<Outline>();
            outline.enabled = false;

        }
    }


}

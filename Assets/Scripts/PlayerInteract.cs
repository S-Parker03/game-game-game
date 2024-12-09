using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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


    void Start()
    {

        playerObj = GameObject.FindGameObjectWithTag("Player");

    }

    void FixedUpdate()
    {
        Debug.DrawLine(cam4ray.position, cam4ray.position + cam4ray.forward * MaxUseDistance, Color.red);
        //code to highlight an interactable object (item, npc or door) when the player is in range
        if (Physics.Raycast(cam4ray.position, cam4ray.forward, out hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.CompareTag("Door") || hit.collider.CompareTag("Item"))
            {
                highlight(hit.collider.gameObject);
                // Debug.Log("highlighted"+hit.collider.gameObject.name);
            }
        }

    }

    // Method to use the binding set up in the "Use" action in the input system
    public void OnUse()
    {
        // Use Raycast to detect how far away the player's front is from an object
        if (Physics.Raycast(cam4ray.position, cam4ray.forward, out hit, MaxUseDistance, UseLayers))
        {
            Debug.DrawRay(cam4ray.position, cam4ray.forward, Color.green);
            // Get Door collider component and see if it's been hit
            if (hit.collider.TryGetComponent<Door>(out Door door))
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
            // else if (hit.collider.CompareTag("NPC"))
            // {
            //     // Get the NPC component from the hit object and interact with it
            //     NPC npc = hit.collider.GetComponent<NPC>();
            //     if (npc != null)
            //     {
            //         // npc.Interact();
            //     }
            // }
            else if (hit.collider.TryGetComponent<ItemInfo>(out ItemInfo itemInfo))
            {
                // Get the ItemInfo component from the hit object and interact with it
                if (itemInfo != null)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().AddItem(itemInfo);
                    itemInfo.collect();
                    
                }
            }
        } 
    }

    public void highlight(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.green;
    }




}

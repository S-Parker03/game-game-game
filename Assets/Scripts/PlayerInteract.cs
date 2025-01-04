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
    // public LayerMask UILayers;
    RaycastHit hit;
    private GameObject playerObj;

    private GameObject lastObject = null;
    private Pause pauseScript; // Reference to the Pause script





    void Start()
    {
        

        playerObj = GameObject.FindGameObjectWithTag("Player");

        pauseScript = GetComponent<Pause>(); // Get the Pause script attached to the same GameObject
        if (pauseScript == null)
        {
            Debug.LogError("Pause script not found in the scene.");
        }
        else
        {
            Debug.Log("Pause Script is found");
        }

    }

    void FixedUpdate()
    {
        Debug.DrawLine(cam4ray.position, cam4ray.position + cam4ray.forward * MaxUseDistance, Color.red);
        // code to highlight an interactable object (item, npc or door) when the player is in range
        if (Physics.Raycast(cam4ray.position, cam4ray.forward, out hit, MaxUseDistance))
        {
            if (lastObject != null && lastObject != hit.collider.gameObject)
            {
                highlight(lastObject, false);
            }    
            if (hit.collider.CompareTag("Door") || hit.collider.CompareTag("Item")
            || hit.collider.CompareTag("SanityPickUp") || hit.collider.CompareTag("NPC")) 
            {
                highlight(hit.collider.gameObject, true);
                lastObject = hit.collider.gameObject;
                // Debug.Log("highlighted"+hit.collider.gameObject.name);
                
                
            }

            
        }
        
        

    }

    // Method to use the binding set up in the "Use" action in the input system
    public void OnUse()
    {
        
        // Use Raycast to detect how far away the player's front is from an object
        if (Physics.Raycast(cam4ray.position, cam4ray.forward, out hit, MaxUseDistance))
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

            else if (hit.collider.TryGetComponent<UIDialogues>(out UIDialogues dialogue))
            {
                if (hit.collider != null)
                {
                    Debug.Log($"Hit object: {hit.collider.name}");
                    pauseScript.pauseGame();
                    // Debug.Log("The game is paused");
                    dialogue.StartDialogue();

                    // pauseScript.resumeGame();
                }
            }

            else if (hit.collider.TryGetComponent<ItemInfo>(out ItemInfo itemInfo))
            {
                // Get the ItemInfo component from the hit object and interact with it
                if (itemInfo != null)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    GameObject inventoryUI = GameObject.Find("Inventory");
                    if (player != null)
                    {
                        Inventory inventory = player.GetComponent<Inventory>();
                        
                        if (inventory != null)
                        {
                            inventory.AddItem(itemInfo);
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
            else if (hit.collider.CompareTag("SanityPickUp")){
                if (playerObj != null)
                {
                    if (playerObj.GetComponent<PlayerController>().Sanity < 5)
                    {
                        if (playerObj.GetComponent<Dependency>().DependencyPercent < 50)
                        {
                            playerObj.GetComponent<PlayerController>().ChangeSanity(1);
                            playerObj.GetComponent<Dependency>().changeDependency(10f);
                            Destroy(hit.collider.gameObject);
                        }
                        else
                        {
                            Destroy(hit.collider.gameObject);
                            playerObj.GetComponent<Dependency>().changeDependency(10f);
                        }
                    }
                    else
                    {
                        Debug.Log("Sanity is full");
                    }
                }
            }
        }
    }

    // public void OnInteract()
    // {
    // if (Input.GetMouseButtonDown(0)){

    //     if (Physics.Raycast(cam4ray.position, cam4ray.forward, out hit, MaxUseDistance, UILayers))
    //     {
    //         Debug.DrawRay(cam4ray.position, cam4ray.forward, Color.green);

    //         // Check for UIDialogues component
    //         if (hit.collider.TryGetComponent<UIDialogues>(out UIDialogues dialogue))
    //         {
    //             if (dialogue != null)
    //             {
    //                 Debug.Log($"Hit object: {hit.collider.name}");
    //                 pauseScript.pauseGame();
    //                 Debug.Log("The game is paused");
    //                 // dialogue.StartDialogue();
    //                 // Debug.Log("Started Dialogue via Mouse presss.");
    //             }
    //         }
    //     }
    // }

    // }


    public void highlight(GameObject obj, bool toggle)
    {
        Outline outline;
        // List<Material> materials = new List<Material>();
        // Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        
        // foreach (Renderer r in renderers)
        // {
        //     materials.AddRange(new List<Material>(r.materials));
        // }

        if(toggle == true)
        {
            
            // foreach (Material m in materials)
            // {
            //     m.EnableKeyword("_EMISSION");
            //     m.SetColor("_EmissionColor", Color.gray);
            //     m.SetFloat("_EmissionBrightness", 0.1f);
            // }
            // Debug.Log("highlighted" + obj.name);
            if (obj.GetComponent<Outline>() == null )
            {
                outline = obj.AddComponent<Outline>();
            }
            else
            {
                outline = obj.GetComponent<Outline>();
            }

            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = new Color(175f / 255f, 0f, 0f);
            outline.OutlineWidth = 20f;
            outline.enabled = true;
        }
        if(toggle == false)
        {
            outline = obj.GetComponent<Outline>();
            outline.enabled = false;
            // foreach (Material m in materials)
            // {
                // m.DisableKeyword("_EMISSION");
                
            // }
            // Debug.Log("unhighlighted" + obj.name);
        }
    }


}

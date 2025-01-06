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

    private Pause pauseScript; // Reference to the Pause script

    private DialogueManager dialogueManager; // Reference to the DialogueManager script

    [SerializeField] private TextAsset OldLadyInkJson;// The ink file to load
    [SerializeField] private TextAsset VaseGuyInkJson;// The ink file to load
    [SerializeField] private TextAsset CreepyGuyInkJson;// The ink file to load
    [SerializeField] private TextAsset YellowKingInkJson;// The ink file to load
    [SerializeField] private TextAsset AngelGhostInkJson;// The ink file to load
    [SerializeField] private TextAsset FountainInkJson;// The ink file to load

    void Start()
    {
        // Get the player object
        playerObj = GameObject.FindGameObjectWithTag("Player");

        pauseScript = UnityEngine.Object.FindFirstObjectByType<Pause>();

        if (pauseScript == null)
        {
            Debug.LogError("Pause script not found in the scene.");
        }



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
            || hit.collider.CompareTag("SanityPickUp") || hit.collider.CompareTag("NPC")) 
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
            Debug.Log("Raycast hit: " + hit.collider.name);
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

           else if (hit.collider.CompareTag("NPC"))
{
    Debug.Log("NPC detected: " + hit.collider.gameObject.name);

    if (pauseScript != null)
    {
        pauseScript.pauseGame();

        // Call StartDialogue from the singleton DialogueManager with the NPC's specific Ink JSON
        if (DialogueManager.instance != null)
        {
           string npcName = hit.collider.gameObject.name;
           Debug.Log("NPC name: " + npcName);

                        if (npcName == "old lady")
                        {
                            DialogueManager.instance.StartDialogue(OldLadyInkJson);
                        }
                        else if (npcName == "VaseGuy")
                        {
                            DialogueManager.instance.StartDialogue(VaseGuyInkJson);
                        }
                        else if (npcName == "CreepyGuy")
                        {
                            DialogueManager.instance.StartDialogue(CreepyGuyInkJson);
                        }
                        else if (npcName == "YellowKing")
                        {
                            DialogueManager.instance.StartDialogue(YellowKingInkJson);
                        }
                        else if (npcName == "AngelGhost")
                        {
                            DialogueManager.instance.StartDialogue(AngelGhostInkJson);
                        }
                        else if (npcName == "Fountain")
                        {
                            DialogueManager.instance.StartDialogue(FountainInkJson);
                        }
                        else if (npcName == "VaseGuy")
                        {
                            DialogueManager.instance.StartDialogue(VaseGuyInkJson);
                        }
                        // Add more else if statements for other NPCs
                        else
                        {
                            Debug.LogError("No Ink JSON assigned for this NPC.");
                        }
                    }
                    else
                    {
                        Debug.LogError("DialogueManager instance is not assigned.");
                    }
                }
                else
                {
                    Debug.LogError("Pause script is not assigned.");
                }
            }
            

           



            else if (hit.collider.TryGetComponent<ItemInfo>(out ItemInfo itemInfo))
            {
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
                            playerObj.GetComponent<Dependency>().changeDependency(10f);
                            SoundManager.instance.PlaySanityPickUpClip(SanityPickUpSound, transform, 1f); // sound for sanity pickup
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
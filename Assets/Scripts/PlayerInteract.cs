using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class for checking if the NPC is in range of the player and if key E is pressed
public class PlayerInteract : MonoBehaviour
{
    // private bool isInteracting = false;
    // [SerializeField] private GameObject interactUI;  // UI element for "Press E to interact"

    private void Update (){
        if (Input.GetKeyDown(KeyCode.E)){
            //radius for the imaginary sphere around the player to detect interactable objects with the player being the cnetre
            float interactRange =0.5f;  
            // function creates an imaginary sphere around a specified point in space (typically the player's position), with a defined radius.
            Collider [] colliderArray = Physics.OverlapSphere(transform.position,interactRange); 
            foreach (Collider collider in colliderArray){
                 var npcInteractable = collider.GetComponent<NPCInteractable>(); //getting the NPCInteractable component
                    npcInteractable.StartDialogue(); //calling StartDialogue method from npcInteractable class
            }
        }
    }
}

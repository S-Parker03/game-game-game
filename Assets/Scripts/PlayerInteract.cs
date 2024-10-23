using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    // private bool isInteracting = false;
    // [SerializeField] private GameObject interactUI;  // UI element for "Press E to interact"

    private void Update (){
        if (Input.GetKeyDown(KeyCode.E)){
            float interactRange =0.5f;
            Collider [] colliderArray = Physics.OverlapSphere(transform.position,interactRange);
            foreach (Collider collider in colliderArray){
                 var npcInteractable = collider.GetComponent<NPCInteractable>();
                    npcInteractable.StartDialogue(); 
            }
        }
    }
}

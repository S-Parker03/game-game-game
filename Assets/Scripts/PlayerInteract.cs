using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class for checking if the NPC is in range of the player and if key E is pressed
public class PlayerInteract : MonoBehaviour
{

    public Transform cam4ray;
        //Variables to do with Door Interactions\\
    public float MaxUseDistance = 1f;
    public LayerMask UseLayers;
    RaycastHit hit;
    private GameObject playerObj;


    void Start()
    {

        playerObj = GameObject.FindGameObjectWithTag("Player");

    }

    // Method to use the binding set up in the "Use" action in the input system
    public void OnUse()
    {
        // Use Raycast to detect how far away the player's front is from an object
        if (Physics.Raycast(cam4ray.position, cam4ray.forward, out hit, MaxUseDistance, UseLayers))
        {
            Debug.DrawRay(cam4ray.position, cam4ray.forward, Color.green);
            // Get Door collider component and see if it's been hit
            if(hit.collider.TryGetComponent<Door>(out Door door))
            {
                //if door is open, then run close method. Otherwise open door with open method
                if (door.isOpen)
                {
                    door.Close();
                }
                else
                {
                    door.Open(playerObj.transform.position);
                }
                
            }
        } 
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// interaction interface
public interface IInteraction
{


   
// this is where the normal press E interact will go 
 public void Interact()
 {
    Debug.Log("You are near the interactable object");

 }


// Now, all objects will implement the basic press E interact function but will have their own special functions
// Like if it's a door, it will open up (specific function for it) in their own scripts


}

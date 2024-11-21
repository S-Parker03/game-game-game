// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;


// This mainly detects if the player is in range of any interactable game object 

// STRUCTURE
// the detection is around player instead of every object having a distance detection. 
//1. creates an imaginary sphere around the player's feet (at the interaction point)
//2. If any interactable object is detected in that sphere, then it populates the array of colliders
//3. All the NPC, collectables need to be in the interactable layer 
public class InteractionSystem : MonoBehaviour
{
    //x,y,z position of the player
    [SerializeField] private Transform _interactionPoint;
    //radius for the imaginary sphere around the player to detect interactable objects with the player being the cnetre
    [SerializeField] private float _interactionPointRadius = 0.5f;
    // radius with the interaction point as the center for an imaginary sphere
    [SerializeField] private LayerMask _interactableMask;
    // to seperate background from interactable objects - which layer can be interacted with. All objects in 
    // interactable layer will be added in here.



    private readonly Collider[] _colliders = new Collider[5];
    //how many interactable objects are found in sphere 

    [SerializeField] private int _numFound; 
    //num of interactable objects found in the sphere
    private void Update()
    {
        // function creates an imaginary sphere around a specified point in space (typically the player's position), with a defined radius.
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders,_interactableMask);
        // returns an int - number of interactable object
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position,_interactionPointRadius);
    // this is the imaginary sphere that checks if any interactable object is within range
    // red wire one so i can see it while testing - keep it until the end 
        
    }

}
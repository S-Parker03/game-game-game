using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy sight cone script
public class SightCone : MonoBehaviour
{
    public bool canSeePlayer = false;

    // when the sight cone collides with the player, set canSeePlayer to true
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player has been seen");
            canSeePlayer = true;
        }
    }
}

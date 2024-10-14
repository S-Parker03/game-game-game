using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{

    //Variables for sanity system\\
    private int sanity;

    int maxSanity = 10;
    public int Sanity => sanity;
    //----------------------------\\

    //Variables for movement system\\
    float speed;
    float rotation;
    //------------------------------\\


    // Start is called before the first frame update
    void Start()
    {
        sanity = maxSanity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            ChangeSanity(1);
            print(sanity);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)){
            ChangeSanity(-1);
            print(sanity);
        }
    }

    // void fixedUpdate(){
    //     Vector3 moveDirection = Vector3.zero;

    //     moveDirection = new Vector3(rotation, 0, 0);
    // }

    public void ChangeSanity(int value){
        sanity += value;
        sanity = Math.Clamp(sanity, 0, maxSanity);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;


public class PlayerController : MonoBehaviour
{

    //Variables for sanity system\\
    private int sanity;
    int maxSanity = 10;
    //allowing readonly access to sanity
    public int Sanity => sanity;
    //----------------------------\\

    //Variables for movement system\\
    public float speed;
    public float rotate_speed;
    private Rigidbody playerbody;
    public Vector2 mouseRotate;        
    public float sensitivity = 0.01f;
    //------------------------------\\

    // Start is called before the first frame update
    void Start()
    {
        //setting sanity to max sanity on game start, not a permanent solution
        sanity = maxSanity;
        //getting the rigidbody component of the player
        playerbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //debug for sanity system
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            ChangeSanity(1);
            print(sanity);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)){
            ChangeSanity(-1);
            print(sanity);
        }

        //movement system
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        mouseRotate.x = Input.GetAxis("Mouse X") * sensitivity;
        mouseRotate.y = Input.GetAxis("Mouse Y") * sensitivity;

        playerbody.velocity = (transform.right * horizontalMove + transform.forward * verticalMove) * speed * Time.fixedDeltaTime;
        transform.Rotate(0, mouseRotate.x, 0);
    }

    //method to safely change sanity
    public void ChangeSanity(int value){
        sanity += value;
        sanity = Math.Clamp(sanity, 0, maxSanity);
    }

}

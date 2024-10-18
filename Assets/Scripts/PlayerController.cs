using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class PlayerController : MonoBehaviour
{

    //Variables for sanity system\\
    private int sanity;

    int maxSanity = 10;
    public int Sanity => sanity;
    //----------------------------\\

    //Variables for movement system\\
    public float speed;
    public float rotate_speed;
    private Rigidbody playerbody;
    //------------------------------\\


    // Start is called before the first frame update
    void Start()
    {
        sanity = maxSanity;

        playerbody = gameObject.GetComponent<Rigidbody>();
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

        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        // playerbody.velocity = new Vector3(horizontalMove * speed * Time.fixedDeltaTime, 0, verticalMove * speed * Time.fixedDeltaTime);
        playerbody.velocity = (transform.forward * verticalMove) * speed * Time.fixedDeltaTime;
        transform.Rotate((transform.up * horizontalMove) * rotate_speed * Time.fixedDeltaTime);
    }

    public void ChangeSanity(int value){
        sanity += value;
        sanity = Math.Clamp(sanity, 0, maxSanity);
    }

}

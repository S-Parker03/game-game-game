using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{

    public bool playerHasKey = false;
    //Variables for sanity system\\
    private int sanity;
    int maxSanity = 5;
    public int Sanity => sanity;
    //----------------------------\\

    // Variables for character movement \\
    private Rigidbody playerbody;
    public Vector2 moveValue;
    public float speed;
    public static Vector2 lookValue;
    [Range(0.0f, 1.0f)] public float sensitivity = 0.5f;
    //----------------------------\\

    // Variables for camera movement \\
    public Camera cam;
    // public Transform cam4ray;
    float horizRotate;
    float vertRotate;
    //----------------------------\\

    private GameObject playerCollider;
    public Vector2 mouseRotate;   
    // public Vector3 movement;// new     



    private PlayerActionControls playerActionControls;
    private InputAction sprintAction;
    private InputAction jumpAction;

    //Variables to do with jumping
    public float jumpForce = 5.0f;
    bool isGrounded = true;

    //------------------------------\\

    // //Variables to do with Door Interactions\\
    // public float MaxUseDistance = 5f;
    // public LayerMask UseLayers;
    // RaycastHit hit;

    //------------------------------\\

    // Pick up variables

    //run once at start
    void Awake()
    {
        playerActionControls = new PlayerActionControls();
        sprintAction = playerActionControls.Player.Sprint;
        jumpAction = playerActionControls.Player.Jump;
    }

    //Enable and disable input
    private void OnEnable()
    {
        playerActionControls.Enable();
    }

    private void OnDisable()
    {
        playerActionControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        sanity = 5;

        playerbody = gameObject.GetComponent<Rigidbody>();

        playerCollider = GameObject.FindGameObjectWithTag("GroundCollider");
    }

    // // Method to use the binding set up in the "Use" action in the input system
    // public void OnUse()
    // {
    //     // Use Raycast to detect how far away the player's front is from an object
    //     if (Physics.Raycast(cam4ray.position, cam4ray.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
    //     {
    //         Debug.DrawRay(cam4ray.position, cam4ray.forward, Color.green);
    //         // Get Door collider component and see if it's been hit
    //         if(hit.collider.TryGetComponent<Door>(out Door door))
    //         {
    //             //if door is open, then run close method. Otherwise open door with open method
    //             if (door.isOpen)
    //             {
    //                 door.Close();
    //             }
    //             else
    //             {
    //                 door.Open(transform.position);
    //             }
                
    //         }
    //     } 
    // }

    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    void OnLook(InputValue value) {
        lookValue = value.Get<Vector2>();
    }

    //function to check if sprinting
    public bool sprintCheck(){
        if(sprintAction.ReadValue<float>() > 0){
            return true;
        }
        else{return false;}
    }

    //function to check if jumping
    public bool jumpCheck(){
        if(jumpAction.ReadValue<float>() > 0){
            return true;
        }
        else{return false;}
    }

    private void FixedUpdate()
    {
        // Movement \\
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y).normalized;
        Vector3 mouse = new Vector2(lookValue.x, lookValue.y);

        //check if sprinting
        if(sprintCheck()){
            speed = 200;
        }
        else{
            speed = 150;
        }        
        
        vertRotate += mouse.y * sensitivity;
        vertRotate = Mathf.Clamp(vertRotate, -70, 90);
        cam.transform.eulerAngles = new Vector3(-vertRotate, horizRotate, 0);
        horizRotate = (horizRotate + mouse.x * sensitivity) % 360f;

        playerbody.MoveRotation(UnityEngine.Quaternion.Euler(0, horizRotate, 0));
        playerbody.velocity = (transform.right * movement.x + transform.forward * movement.z) * speed * Time.fixedDeltaTime  + transform.up * playerbody.velocity.y ;

        if (jumpCheck() && isGrounded)
        {
            playerbody.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
            isGrounded = false;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Grounded")
        {
            isGrounded = true;
        }
    }

    //method to handle collision with healing items and key items
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "SanityPickUp" && sanity < maxSanity){
            //find Dependecy value and calculate dependency as a value netween 0 and 1
            GameObject.Find("Player").GetComponent<Dependency>().changeDependency(10f);
            float dependency = GameObject.Find("Player").GetComponent<Dependency>().DependencyPercent/100;
            other.gameObject.SetActive(false);
            //heal by 2*(1-dependency)
            ChangeSanity((int)Math.Round(2*(1-dependency)));
        }
    }

    // change sanity value as enemies collide with player
    public void ChangeSanity(int value){
        sanity += value;
        sanity = Math.Clamp(sanity, 0, maxSanity);
    }

}

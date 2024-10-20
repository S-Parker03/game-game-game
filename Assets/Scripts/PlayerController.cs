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
    public int Sanity => sanity;
    //----------------------------\\

    //Variables for movement system\\
    public float speed;
    public float rotate_speed;
    private Rigidbody playerbody;
    public Vector2 mouseRotate;        
    public float sensitivity = 0.01f;

    // public float jumpHeight = 15.0f;
    //------------------------------\\

    //Variables for jump movement system\\
    public float jumpForce = 7.0f;
    public LayerMask floorLayer;
    public float raycastDist = 0.6f;
    // private bool isGrounded;

    private PlayerActionControls playerActionControls;

    // PlayerInput playerInput;
    // Action inventoryAction;
    // public InputAction jumpAction;
    //--------------------------------\\

    void Awake(){
        // playerInput = GetComponent<PlayerInput>();
        // inventoryAction = playerInput.actions("Inventory");
        playerActionControls = new PlayerActionControls();
    }

    private void OnEnable(){
        playerActionControls.Enable();
    }

    private void OnDisable(){
        playerActionControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        sanity = maxSanity;

        playerbody = gameObject.GetComponent<Rigidbody>();

        playerActionControls.Player.Jump.performed += _ => Jump();
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Jump(){
        if (IsGrounded()){
            playerbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.Impulse);
        }
    }

    private bool IsGrounded(){
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.down, out hit, raycastDist, floorLayer);
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
        mouseRotate.x = Input.GetAxis("Mouse X") * sensitivity;
        mouseRotate.y = Input.GetAxis("Mouse Y") * sensitivity;

        // playerbody.velocity = new Vector3(horizontalMove * speed * Time.fixedDeltaTime, 0, verticalMove * speed * Time.fixedDeltaTime);
        playerbody.velocity = (transform.right * horizontalMove + transform.forward * verticalMove) * speed * Time.fixedDeltaTime;
        // playerbody.velocity = (transform.forward * verticalMove) * speed * Time.fixedDeltaTime;
        // playerbody.velocity = (-transform.right * verticalMove) * speed * Time.fixedDeltaTime;


        // transform.Rotate((transform.up * horizontalMove) * rotate_speed * Time.fixedDeltaTime);
        transform.Rotate(0, mouseRotate.x, 0); //, Space.World);
        // transform.Rotate(Quaternion.Euler(-mouseRotate.y, mouseRotate.x, 0));
        // playerbody.AddForce(new Vector3(0.0f, jumpHeight, 0.0f), ForceMode.Impulse);

        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDist, floorLayer))
        // {
        //     isGrounded = true;
        //     Debug.Log(isGrounded);
        // } else
        // {
        //     isGrounded = false;
        //     Debug.Log(isGrounded);
        // }

        // if (playerInput.GetComponent<PlayerInput>().actions["Jump"].GetBinding)
        // float jumpInput = playerActionControls.Player.Jump.ReadValue<float>();
    }

    // void FixedUpdate()
    // {
    //     if (Input.GetKey("Space"))
    //     {
    //         playerbody.AddForce(new Vector3(0.0f, jumpHeight, 0.0f), ForceMode.Impulse);

    //     // Vector3 jump = new Vector3(moveValue.x, 0.0f, moveValue.y);

    //     // playerbody.AddForce(movement * speed * Time.fixedDeltaTime);
   
    //     }
    // }
    public void ChangeSanity(int value){
        sanity += value;
        sanity = Math.Clamp(sanity, 0, maxSanity);
    }

}

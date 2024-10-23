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
using TMPro;
using Unity.VisualScripting;



public class PlayerController : MonoBehaviour
{

    public bool playerHasKey = false;
    //Variables for sanity system\\
    private int sanity;
    int maxSanity = 5;
    public int Sanity => sanity;
    //----------------------------\\

    //Variables for movement system\\
    public float speed;
    public float rotate_speed;
    private Rigidbody playerbody;
    public Vector2 mouseRotate;        
    public float sensitivity = 0.01f;

     private PlayerActionControls playerActionControls;
    private InputAction sprintAction;


    //------------------------------\\

    //Variables to do with Door Interactions\\
    [SerializeField]
    private float MaxUseDistance = 5f;
    [SerializeField]
    private LayerMask UseLayers;
    RaycastHit hit;

    //------------------------------\\

    // Pick up variables


    void Awake()
    {
        playerActionControls = new PlayerActionControls();
        sprintAction = playerActionControls.Player.Sprint;
    }


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
    }
    public void OnUse()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxUseDistance, UseLayers))
        {
            if(hit.collider.TryGetComponent<Door>(out Door door))
            {
                
                if (door.isOpen)
                {
                    door.Close();
                }
                else
                {
                    door.Open(transform.position);
                }
                
            }
        } 
    }

    public bool sprintCheck(){
        if(sprintAction.ReadValue<float>() > 0){
            return true;
        }
        else{return false;}
    }


    // Update is called once per frame
    private void Update()
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

        if(sprintCheck()){
            speed = 600;
        }
        else{
            speed = 300;
        }               

        playerbody.velocity = (transform.right * horizontalMove + transform.forward * verticalMove) * speed * Time.fixedDeltaTime;
        transform.Rotate(0, mouseRotate.x, 0);
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "SanityPickUp" && sanity < maxSanity){
            other.gameObject.SetActive(false);
            ChangeSanity(2);
        } else if(other.gameObject.tag == "KeyItem"){
            other.gameObject.SetActive(false);
            playerHasKey = true;
        }
    }

    public void ChangeSanity(int value){
        sanity += value;
        sanity = Math.Clamp(sanity, 0, maxSanity);
    }

}

using System;
using System.Numerics;
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

    //Variables for movement system\\
    public float speed;
    private Rigidbody playerbody;
    private GameObject playerCollider;
    public Vector2 mouseRotate;   
    public Vector3 movement;// new     
    public float sensitivity = 0.01f;

    private PlayerActionControls playerActionControls;
    private InputAction sprintAction;

    //Variables to do with jumping
    public float jumpForce = 5.0f;
    bool isGrounded = true;

    //------------------------------\\

    //Variables to do with Door Interactions\\
    public float MaxUseDistance = 5f;
    public LayerMask UseLayers;
    RaycastHit hit;

    //------------------------------\\

    // Pick up variables

    //run once at start
    void Awake()
    {
        playerActionControls = new PlayerActionControls();
        sprintAction = playerActionControls.Player.Sprint;
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

    // Method to use the binding set up in the "Use" action in the input system
    public void OnUse()
    {
        // Use Raycast to detect how far away the player's front is from an object
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxUseDistance, UseLayers))
        {
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
                    door.Open(transform.position);
                }
                
            }
        } 
    }

    //function to check if sprinting
    public bool sprintCheck(){
        if(sprintAction.ReadValue<float>() > 0){
            return true;
        }
        else{return false;}
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // sanity debudding \\
        // if (Input.GetKeyDown(KeyCode.UpArrow)){
        //     ChangeSanity(1);
        //     print(sanity);
        // }

        // if (Input.GetKeyDown(KeyCode.DownArrow)){
        //     ChangeSanity(-1);
        //     print(sanity);
        // }
        // ----------------- \\

        //getting player inputs for movement and rotation using the input system
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        mouseRotate.x = Input.GetAxis("Mouse X") * sensitivity;
        mouseRotate.y = Input.GetAxis("Mouse Y") * sensitivity;

        //check if sprinting
        if(sprintCheck()){
            speed = 600;
        }
        else{
            speed = 300;
        }        
        
        // movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized;

        //move the player using actions set up in input system and rotate using the mouse
        // playerbody.velocity = (transform.right * movement.x + transform.forward * movement.z) * speed * Time.deltaTime;
        // transform.Rotate(0, mouseRotate.x, 0);

        // playerbody.velocity = movement * speed * Time.fixedDeltaTime;
        // float horizontalMove = Input.GetAxis("Horizontal");
        // float verticalMove = Input.GetAxis("Vertical");
        movement = new Vector3(horizontalMove, 0.0f, verticalMove).normalized;
        // movement.y = playerbody.velocity.y;
        // playerbody.velocity = movement * speed;

        playerbody.velocity = (transform.right * movement.x + transform.forward * movement.z) * speed * Time.fixedDeltaTime  + transform.up * playerbody.velocity.y ;
        transform.Rotate(0, mouseRotate.x, 0);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //playerbody.velocity = new Vector3(0.0f, jumpForce, 0.0f);
            // playerbody.AddForce(new Vector3(0.0f, 100, 0.0f));
             playerbody.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
            // playerbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f));

            // Debug.Log(isGrounded);


            isGrounded = false;
        }

    }
    // public void OnJump()
    // {
    //     if (isGrounded){
    //         // playerbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f));
    //         playerbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);



    //         isGrounded = false;
    //     }
    // }

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
            float dependency = GameObject.Find("Player").GetComponent<Dependency>().DependencyPercent/100;
            other.gameObject.SetActive(false);
            //heal by 2*(1-dependency)
            ChangeSanity((int)Math.Round(2*(1-dependency)));
        } else if(other.gameObject.tag == "KeyItem"){
            other.gameObject.SetActive(false);
            playerHasKey = true;
        }
    }

    // void FixedUpdate() {
    //     // playerbody.velocity = movement * speed * Time.fixedDeltaTime;
    //     float horizontalMove = Input.GetAxis("Horizontal");
    //     float verticalMove = Input.GetAxis("Vertical");
    //     movement = new Vector3(horizontalMove, 0.0f, verticalMove).normalized;
    //     // movement.y = playerbody.velocity.y;
    //     // playerbody.velocity = movement * speed;

    //     playerbody.velocity = (transform.right * movement.x + transform.up * playerbody.velocity.y + transform.forward * movement.z) * speed * Time.fixedDeltaTime;
    //     transform.Rotate(0, mouseRotate.x, 0);

    //     // if (Input.GetButtonDown("Jump") && isGrounded)
    //     // {
    //     //     // playerbody.velocity = new Vector3(0.0f, jumpForce, 0.0f);
    //     //     // playerbody.AddForce(new Vector3(0.0f, 100, 0.0f));
    //     //     // playerbody.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
    //     //     playerbody.AddForce(0.0f, jumpForce, 0.0f);

    //     //     Debug.Log(isGrounded);


    //     //     // isGrounded = false;
    //     // }
    //             // transform.Rotate(0, mouseRotate.x, 0);

    //     // if (Input.GetButtonDown("Jump") && !isGrounded)
    //     // {
    //     //     // playerbody.velocity = new Vector3(0.0f, -jumpForce, 0.0f);
    //     //     playerbody.AddForce(new Vector3(0.0f, -100, 0.0f));
    //     //     // playerbody.AddForce(Vector3.down * jumpForce , ForceMode.Impulse);


    //     //     isGrounded = true;
    //     // }
    // }
    // change sanity value as enemies collide with player
    public void ChangeSanity(int value){
        sanity += value;
        sanity = Math.Clamp(sanity, 0, maxSanity);
    }

}

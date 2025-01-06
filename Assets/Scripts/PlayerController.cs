using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using Unity.VisualScripting;


public class PlayerController : MonoBehaviour
{

    public bool playerHasKey = false;
    //Variables for sanity system\\
    private int sanity;
    int maxSanity = 5;
    public int Sanity => sanity;

    public Slider sanitySlider;

    PostProcessVolume volume;

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

    // private GameObject playerCollider;
    public Vector2 mouseRotate;   
    // public Vector3 movement;// new     



    private PlayerActionControls playerActionControls;
    private InputAction sprintAction;
    private InputAction jumpAction;

    //Variables to do with jumping
    public float jumpForce = 5.0f;
    public bool isGrounded = true;

    public AudioSource footsteps;

    //------------------------------\\

    // //Variables to do with Door Interactions\\
    // public float MaxUseDistance = 5f;
    // public LayerMask UseLayers;
    // RaycastHit hit;

    //------------------------------\\

    // Pick up variables

    //variables for sound 
    [SerializeField] private AudioClip FootstepsSound;
    [SerializeField] private AudioClip DamageSound;

    //run once at start
    void Awake()
    {
        //set up the player action controls
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
        //find the player's rigidbody
        playerbody = gameObject.GetComponent<Rigidbody>();
        volume = cam.GetComponent<PostProcessVolume>();
    }


    //Method to handle movement input
    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
        //play footsteps sound if moving
        if(isGrounded && moveValue.magnitude > 0.1f && !footsteps.isPlaying){
            footsteps.Play();
        } else {
            footsteps.Stop();
        }
    }

    //Method to handle look input
    void OnLook(InputValue value) {
        lookValue = value.Get<Vector2>();
    }

    //function to check if sprinting
    public bool SprintCheck(){
        if(sprintAction.ReadValue<float>() > 0){
            return true;
        }
        else{return false;}
    }

    //function to check if jumping
    public bool JumpCheck(){
        if(jumpAction.ReadValue<float>() > 0){
            return true;
        }
        else{return false;}
    }

    // FixedUpdate is called at fixed intervals
    private void FixedUpdate()
    {
        // Movement \\
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y).normalized;
        Vector3 mouse = new Vector2(lookValue.x, lookValue.y);

        //check if sprinting
        if(SprintCheck()){
            speed = 200;
        }
        else{
            speed = 150;
        }        

        //handle camera rotation
        vertRotate += mouse.y * sensitivity;
        vertRotate = Mathf.Clamp(vertRotate, -70, 70);
        cam.transform.eulerAngles = new Vector3(-vertRotate, horizRotate, 0);
        horizRotate = (horizRotate + mouse.x * sensitivity) % 360f;
        //rotate the player body
        playerbody.MoveRotation(UnityEngine.Quaternion.Euler(0, horizRotate, 0));
        //move the player
        playerbody.velocity = (transform.right * movement.x + transform.forward * movement.z) * speed * Time.fixedDeltaTime  + transform.up * playerbody.velocity.y ;
        //jump
        if (JumpCheck() && isGrounded)
        {
            playerbody.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
            isGrounded = false;
        }

        sanitySlider.value = Sanity;

        SanityEffects();

    }

    // Update is called once per frame
    void Update(){
        
        //find the torch and torch ambience lights
        Light torch = GameObject.Find("Torch").GetComponent<Light>();
        Light torchAmbience = GameObject.Find("Torch Ambience").GetComponent<Light>();
        SettingsManager settings = GameObject.Find("Settings").GetComponent<SettingsManager>();
        float spotAngleMult = settings.torchRadiusPercent/100.0f;
        //multiply torch radius and sensitivity by their percentages from settings controller
        torch.spotAngle = 160.0f * spotAngleMult;
        torchAmbience.spotAngle = 250.0f * spotAngleMult;
        sensitivity = settings.sensitivityPercent/200.0f + 0.05f;
        //adjust brightness
        AdjustBrightness(settings.brightnessPercent / 400.0f);
        
    }
    //function to adjust brightness of the game
    public void AdjustBrightness(float brightness){
        //set the ambient light to the brightness
        RenderSettings.ambientLight = new Color(brightness, brightness, brightness, 1.0f);
    }

    //function to handle collision with the ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Grounded")
        {
            isGrounded = true;
        }
        
    }


    //function to handle collision with death planes
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathPlane")
        {
            //kill the player
            ChangeSanity(-99999);
        }
    }

    //function to change sanity
    public void ChangeSanity(int value){
        sanity += value;
        sanity = Math.Clamp(sanity, 0, maxSanity);
        if(value < 0){
            SoundManager.instance.PlayDamageClip(DamageSound, transform, 1f);
        }
    }

    //debug function to kill the player
    void OnKillYourSelf(){
        ChangeSanity(-99999);
    }

    //debug function to lower sanity
    void OnLowerSanity(){
        ChangeSanity(-1);
    }
    
    //function to handle sanity effects
    // Sanity camera effects
    void SanityEffects(){
        if (volume.profile.TryGetSettings<Vignette>(out Vignette vignette))
        {
            float effects_value = Sanity;

            // (hue - min1)/(max1 - min1)
            effects_value = (effects_value - 0) / (5 - 0);
            // hue * (max2 - min 2) + min2
            effects_value = effects_value * (0 - 1) + 1;
            vignette.intensity.value = effects_value;
            vignette.smoothness.value = effects_value;
        }
    }

}

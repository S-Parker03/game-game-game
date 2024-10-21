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

    //Variables to do with Door Interactions\\
    [SerializeField]
    private TextMeshPro UseText;
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private float MaxUseDistance = 5f;
    [SerializeField]
    private LayerMask UseLayers;

    public TextMeshPro doorOpenText;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        //setting sanity to max sanity on game start, not a permanent solution
        sanity = maxSanity;
        //getting the rigidbody component of the player
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

    // Update is called once per frame
    private void Update()
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

        // if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxUseDistance, UseLayers) 
        // && hit.collider.TryGetComponent<Door>(out Door door))
        // {
        //     if (door.isOpen)
        //     {
                
        //         doorOpenText.SetText("Close 'E'");
        //     }
        //     else
        //     {
        //         doorOpenText.SetText("Open 'E'");
        //     }
        //     doorOpenText.gameObject.SetActive(true);
        //     // UseText.transform.position = hit.point - (hit.point - transform.position).normalized * 0.01f;
        //     // UseText.transform.rotation = Quaternion.LookRotation((hit.point - transform.position).normalized);
        // }
        // else
        // {
        //     doorOpenText.gameObject.SetActive(false);
        // }
    }




    //method to safely change sanity
    public void ChangeSanity(int value){
        sanity += value;
        sanity = Math.Clamp(sanity, 0, maxSanity);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    // public Transform mainCamera;

    public Vector3 offset;
        // float xRotation = 0f;

        //     public float mouseSensitivity = 100f;


    // Start is called before the first frame update
    // void Start()
    // {
    //     offset = transform.position;
    // }

    // Update is called once per frame

    // void Update(){
    //      float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

    // xRotation -= mouseY;
    // xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    // transform.rotation = Quaternion.Euler(0f, player.transform.rotation.eulerAngles.y, 0f);

    // }
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}


// --------------------------------------------------------------------------------------------------------------------- \\
// Adapted from LiamAcademy Rotating and Sliding Doors tutorial on youtube - https://www.youtube.com/watch?v=cPltQK5LlGE \\
// --------------------------------------------------------------------------------------------------------------------- \\

using System.Collections;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Door : MonoBehaviour
{
    // variables used to transform a door object's location to make it appear as if it's sliding\\
    public bool isOpen = false;
    public float speed = 1.0f;
    private float RotationAmount = 90f;
    public float ForwardDirection = 0;
    private Vector3 StartRotation;
    private Vector3 Forward;
    private Coroutine AnimationCoroutine;
    //-------------------------------------------------------------------------------------------\\

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        //rest the door to initial position
        Forward = transform.position;

    }

    //open the door using coroutine, first checking if it's not open
    public void Open(Vector3 UserPosition)
    {
        if (!isOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            float dot = Vector3.Dot(Forward, (UserPosition - transform.position).normalized);
            Debug.Log($"Dot: {dot.ToString("N3")}");
            AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;
            Debug.Log($"Dot: {ForwardAmount.ToString("N3")}");
            Debug.Log($"Dot: {ForwardDirection.ToString("N3")}");

        if (ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }
        isOpen = true;

        float time = 0;
        while(time <1)
        {
            // use Slerp instead of Lerp to create a slower movement effect when door is 
            // almost fully open or closed, and faster when it's in the middle of the animation
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    //close the door using coroutine, first checking if it's open
    public void Close()
    {
        if(isOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(DoRotationCLose());
        }
    }

    private IEnumerator DoRotationCLose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);

        isOpen = false;

        float time = 0;
        while(time <1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}

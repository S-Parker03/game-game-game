
// --------------------------------------------------------------------------------------------------------------------- \\
// Adapted from LiamAcademy Rotating and Sliding Doors tutorial on youtube - https://www.youtube.com/watch?v=cPltQK5LlGE \\
// --------------------------------------------------------------------------------------------------------------------- \\

using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    // variables used to transform a door object's location to make it appear as if it's sliding\\
    public bool isOpen = false;
    public float speed = 1.0f;
    public Vector3 slideDirection = Vector3.back;
    public float slideDistance = 0.0f;
    private Vector3 StartPosition;
    private Coroutine AnimationCoroutine;
    //-------------------------------------------------------------------------------------------\\

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        //rest the door to initial position
        StartPosition = transform.position;
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
            AnimationCoroutine = StartCoroutine(DoSlidingOpen());
        }
    }

    // Do sliding open method to move the door from its original position to a 
    // new position whose direction is determined by a vector and its transformation in that direction by slideDistance variable
    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = StartPosition + slideDistance * slideDirection;
        Vector3 startPosition = transform.position;

        float time = 0;
        isOpen = true;
        while(time < 1)
        {
            //move the door from start position to end position over time
            time += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
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

            AnimationCoroutine = StartCoroutine(DoSlidingCLose());
        }
    }

    // Do sliding close method to move the door from its new position to its origial position
    private IEnumerator DoSlidingCLose()
    {
        Vector3 endPosition = StartPosition;
        Vector3 startPosition = transform.position;

        float time = 0;
        isOpen = false;

        while (time < 1)
        {
            // move the door from end position to start position using transform over time
            time += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
        }
    }
}

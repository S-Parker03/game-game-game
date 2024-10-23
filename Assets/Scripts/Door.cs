using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private Vector3 slideDirection = Vector3.back;
    [SerializeField]
    private float slideDistance = 2.0f;

    private Vector3 StartPosition;

    private Coroutine AnimationCoroutine;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        //rest the door to initial position
        StartPosition = transform.position;
    }

    //Open the door
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

    //Coroutine to open the door
    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = StartPosition + slideDistance * slideDirection;
        Vector3 startPosition = transform.position;

        float time = 0;
        isOpen = true;
        while(time < 1)
        {
            //move the door from start position to end position over time
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    //close the door
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

    //Coroutine to close the door
    private IEnumerator DoSlidingCLose()
    {
        Vector3 endPosition = StartPosition;
        Vector3 startPosition = transform.position;

        float time = 0;
        isOpen = false;

        while (time < 1)
        {
            //move the door from end position to start position over time
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}

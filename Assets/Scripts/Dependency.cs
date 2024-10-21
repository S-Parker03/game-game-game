using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dependency : MonoBehaviour
{

    private float dependencyPercent;

    public float DependencyPercent => dependencyPercent;
    // Start is called before the first frame update
    void Start()
    {
        dependencyPercent = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8)){
            changeDependency(10f);
            Debug.Log(dependencyPercent);

        }

        if (Input.GetKeyDown(KeyCode.Keypad2)){
            changeDependency(-10f);
            Debug.Log(dependencyPercent);

        }

        dependencyPercent -=0.001f;
        dependencyPercent = Mathf.Clamp(dependencyPercent, 0, 100);

        
    }

    void changeDependency(float value){
        dependencyPercent += value;
    }
}

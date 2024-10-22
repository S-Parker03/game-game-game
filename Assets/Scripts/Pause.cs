using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;

    public TextMeshProUGUI pauseText;
    public bool paused;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Monster");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !paused){
            Debug.Log("Pausing game");

            Cursor.lockState = CursorLockMode.None;
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<Dependency>().enabled = false;
            enemy.GetComponent<EnemyController>().enabled = false;
            pauseText.text = "Game Paused";
            Time.timeScale = 0;
            paused = true;
        }else if (Input.GetKeyDown(KeyCode.Escape) && paused){
            Debug.Log("Unpausing game");
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<Dependency>().enabled = true;
            enemy.GetComponent<EnemyController>().enabled = true;
            pauseText.text = "";
            Time.timeScale = 1;
            paused = false;
        }          
    }
}

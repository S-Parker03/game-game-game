using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;

    public TextMeshProUGUI pauseText;
    public bool paused;

    public bool gameOver;
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
        //check if the player presses the escape key, if they do pause or unpause the game
        if(Input.GetKeyDown(KeyCode.Escape) && !paused){
            pauseGame();
            pauseText.text = "Game Paused";
            paused = true;
        }else if (Input.GetKeyDown(KeyCode.Escape) && paused){
            resumeGame();
            pauseText.text = "";
            paused = false;
        }          
    }
    //function to pause the game
    public void pauseGame(){
        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Dependency>().enabled = false;
        enemy.GetComponent<EnemyController>().enabled = false;
        
        Time.timeScale = 0;
        
    }
    //function to resume the game
    public void resumeGame(){
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Dependency>().enabled = true;
        enemy.GetComponent<EnemyController>().enabled = true;
        
        Time.timeScale = 1;
        
    }



}

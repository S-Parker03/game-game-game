using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Pause : MonoBehaviour
{

    public GameObject player;
    // public GameObject enemy;
    public GameObject UI;

    public GameObject HUD;

    public bool paused;

    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        player = GameObject.Find("Player");
        UI.SetActive(false);

        // enemy = GameObject.Find("Monster");
    
        
    }

    // Update is called once per frame
    public void OnPause()
    {
        //check if the player presses the escape key, if they do pause or unpause the game
        if(!paused){
            pauseGame();
            UI.GetComponent<InventoryController>().guiNeedsUpdating = true;
            UI.SetActive(true);
            
            paused = true;
        }else if (paused){
            resumeGame();
            UI.SetActive(false);
            
            paused = false;
        }          
    }
    //function to pause the game
    public void pauseGame(){
        foreach( var each in HUD.GetComponentsInChildren<TextMeshProUGUI>()){
            each.alpha = 0;
        }
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Dependency>().enabled = false;
        // enemy.GetComponent<EnemyController>().enabled = false;
        
        
        Time.timeScale = 0;
        
    }
    //function to resume the game
    public void resumeGame(){
        foreach( var each in HUD.GetComponentsInChildren<TextMeshProUGUI>()){
            each.alpha = 1;
        }
        
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Dependency>().enabled = true;
        // enemy.GetComponent<EnemyController>().enabled = true;
        
        Time.timeScale = 1;
        
    }



}

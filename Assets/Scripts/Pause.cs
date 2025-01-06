using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Pause : MonoBehaviour
{

    public GameObject player;
    // public GameObject enemy;
    public GameObject UI;

    public GameObject HUD;

    GameObject settings;

    GameObject mainMenu;

    GameObject endScreen;

    public GameObject dependencySlider;

    public GameObject sanityDial;

    public bool paused;

    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        //initialize the player, UIs and settings objects
        paused = false;
        player = GameObject.Find("Player");
        settings = GameObject.Find("Settings");
        mainMenu = GameObject.Find("MainMenu");
        endScreen = GameObject.Find("EndScreen");
        //hide the UI elements
        UI.SetActive(false);
        //hide the dependency slider and sanity dial
        dependencySlider.SetActive(false);
        sanityDial.SetActive(false);    
    }

    // Update is called once per frame
    public void OnPause()
    {
        //check if the player presses the escape key, if they do pause or unpause the game
        if(!paused){
            //if the main menu, settings and end screen are not displayed, pause the game
            if (mainMenu.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None) && settings.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None) && endScreen.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None)){
            pauseGame();
            //update the inventory GUI
            UI.GetComponent<InventoryController>().guiNeedsUpdating = true;
            //display the inventory GUI
            UI.SetActive(true);
            paused = true;
            }  
        }else if (paused){
            //if the main menu, settings and end screen are not displayed, resume the game
            if (mainMenu.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None) && settings.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None) && endScreen.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None)){
                resumeGame();
                //hide the inventory GUI
                UI.SetActive(false);
                paused = false;
                
            }
        }          
    }
    //function to pause the game
    public void pauseGame(){
        //hide the HUD elements
        foreach( Transform child in HUD.transform){
            child.gameObject.SetActive(false);
        }
        //unlock the cursor
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        //disable the player's movement, interaction, input and dependency scripts
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerInteract>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;
        player.GetComponent<Dependency>().enabled = false;
        //set the time scale to 0
        Time.timeScale = 0;
        
    }
    //function to resume the game
    public void resumeGame(){
        //show the HUD elements
        foreach( Transform child in HUD.transform){
            child.gameObject.SetActive(false);

        }
        //lock the cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        //enable the player's movement, interaction, input and dependency scripts
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<PlayerInteract>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<Dependency>().enabled = true;
        dependencySlider.SetActive(true);
        sanityDial.SetActive(true);
        //set the time scale to 1
        Time.timeScale = 1;
        
    }



}

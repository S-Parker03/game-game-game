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

    public GameObject dependencySlider;

    public GameObject sanityDial;

    public bool paused;

    public bool gameOver;
    public UIDialogues uiDialogues; // Reference to the UIDialogues script
    public @PlayerActionControls playerControls; 
    // Start is called before the first frame update
    void Start()
    {
        playerControls = new @PlayerActionControls();  // Initialize input controls
        Debug.Log("player controls is initialised properly");
        playerControls.Enable();  // Enable the input controls
        paused = false;
        player = GameObject.Find("Player");
        settings = GameObject.Find("Settings");
        mainMenu = GameObject.Find("MainMenu");
        UI.SetActive(false);
        dependencySlider.SetActive(false);
        sanityDial.SetActive(false);
        // enemy = GameObject.Find("Monster");

        if (uiDialogues == null)
        {
            uiDialogues = FindObjectOfType<UIDialogues>();
        }

        if (uiDialogues == null)
        {
            Debug.LogError("UIDialogues is not assigned or found in the scene!");
        }
    
        
    }
        void Update()
    {
        // Check for mouse press while the game is paused
        if (paused==true && Input.GetMouseButtonDown(0)) {

            if (uiDialogues != null)
        {
            uiDialogues.gameObject.SetActive(true);
            uiDialogues.StartDialogue();
            Debug.Log("Dialogue started");
        }
        else
        {
            Debug.LogError("uiDialogues is not assigned.");
        }

        }
    }

    // Update is called once per frame
    public void OnPause()
    {
        //check if the player presses the escape key, if they do pause or unpause the game
        if(!paused){
            if (mainMenu.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None) && settings.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None)){
            pauseGame();
            UI.GetComponent<InventoryController>().guiNeedsUpdating = true;
            UI.SetActive(true);
            paused = true;
            }  
        }else if (paused){
            if (mainMenu.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None) && settings.GetComponent<UIDocument>().rootVisualElement.style.display.Equals(DisplayStyle.None)){
                resumeGame();
                UI.SetActive(false);
                
                paused = false;
                
            }
        }          
    }
    //function to pause the game
    public void pauseGame(){
        foreach( var each in HUD.GetComponentsInChildren<TextMeshProUGUI>()){
            each.alpha = 0;
        }
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerInteract>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;
        player.GetComponent<Dependency>().enabled = false;
        UnityEngine.Cursor.visible = true; // so the cursor is visible when the game is paused

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
        player.GetComponent<PlayerInteract>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<Dependency>().enabled = true;
        dependencySlider.SetActive(true);
        sanityDial.SetActive(true);
        // enemy.GetComponent<EnemyController>().enabled = true;
        
        Time.timeScale = 1;
        
    }



}

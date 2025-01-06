using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    Pause pause;
    GameObject player;
    
    GameObject settings;

    UIDocument mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        //Finding the player
        player = GameObject.Find("Player");
        //setting the player's sanity to 5
        player.GetComponent<PlayerController>().ChangeSanity(5);
        //Finding the settings and pause objects
        settings = GameObject.Find("Settings");
        //Find the pause component
        pause = player.GetComponent<Pause>();
        //Find the main menu
        mainMenu = GetComponent<UIDocument>();
        //Pause the game
        pause.pauseGame();
        //Open the main menu
        OpenMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //called each frame when the gui is open
    void OnGUI()
    {
        //refreshes the menu each frame it is open
        if (mainMenu.rootVisualElement.style.display == DisplayStyle.Flex)
        {
            mainMenu.rootVisualElement.style.display = DisplayStyle.None;
            mainMenu.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }

    //Function to open the main menu
    public void OpenMenu(){
        Debug.Log("Menu Clicked");
        //Hide the settings menu
        settings.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        //Show the main menu
        mainMenu.rootVisualElement.style.display = DisplayStyle.Flex;
        //Find the buttons in the main menu
        Button startGameButton = mainMenu.rootVisualElement.Q<Button>("StartButton");
        Button settingsButton = mainMenu.rootVisualElement.Q<Button>("SettingsButton");
        Button quitGameButton = mainMenu.rootVisualElement.Q<Button>("QuitButton");
        //Add event listeners to the buttons
        startGameButton.RegisterCallback<ClickEvent>(StartGame);
        settingsButton.RegisterCallback<ClickEvent>(evt => {
            Debug.Log("Settings Button Clicked");
            settings.GetComponent<SettingsManager>().previousMenu =  "MainMenu";
            settings.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
            mainMenu.rootVisualElement.style.display = DisplayStyle.None;
        });
        quitGameButton.RegisterCallback<ClickEvent>(evt => {
            Debug.Log("Quit Button Clicked");
            Application.Quit();
        });

    }

    private void StartGame(ClickEvent evt)
    {
        Debug.Log("Start Button Clicked");
        //Hide the main menu
        mainMenu.rootVisualElement.style.display = DisplayStyle.None;
        //Find the player object
        player = GameObject.Find("Player");
        //Find the pause component
        pause = player.GetComponent<Pause>();
        mainMenu = GetComponent<UIDocument>();
        //Hide the settings menu
        settings.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        //Resume the game
        pause.resumeGame();
    }

}

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

    void Start()
    {
        player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().ChangeSanity(5);
        settings = GameObject.Find("Settings");
        pause = player.GetComponent<Pause>();
        mainMenu = GetComponent<UIDocument>();
        pause.pauseGame();
        OpenMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (mainMenu.rootVisualElement.style.display == DisplayStyle.Flex)
        {
            mainMenu.rootVisualElement.style.display = DisplayStyle.None;
            mainMenu.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }

    public void OpenMenu(){
        Debug.Log("Menu Clicked");
        settings.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        mainMenu.rootVisualElement.style.display = DisplayStyle.Flex;
        Button startGameButton = mainMenu.rootVisualElement.Q<Button>("StartButton");
        Button settingsButton = mainMenu.rootVisualElement.Q<Button>("SettingsButton");
        Button quitGameButton = mainMenu.rootVisualElement.Q<Button>("QuitButton");
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
        mainMenu.rootVisualElement.style.display = DisplayStyle.None;

        player = GameObject.Find("Player");
        pause = player.GetComponent<Pause>();
        mainMenu = GetComponent<UIDocument>();
        
        settings.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        pause.resumeGame();
    }

}

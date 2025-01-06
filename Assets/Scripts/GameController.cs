using Palmmedia.ReportGenerator.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    GameObject player;
    public GameObject GameOver;
    public GameObject RestartButton;
    public Pause pause;

    public enum Ending {
        death,
        bad,
        good,
        neutral
    }

    public GameObject endScreenobj;

    public VisualElement endUI;

    public Button menuButton;

    void Start()
    {
    // finds player object and the UI objects, sets UI to be visible on scene load 
    
        player = GameObject.FindGameObjectWithTag("Player");
        endScreenobj = GameObject.Find("EndScreen");
        endUI = endScreenobj.GetComponent<UIDocument>().rootVisualElement;
        menuButton = endUI.Q<Button>("MenuButton");
        endUI.style.display = DisplayStyle.None;

        // sets the cursor to be visible and unlocked
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        //and pauses the game
        pause.pauseGame();
        //sets the settings menu to be invisible
        GameObject.Find("Settings").GetComponent<UIDocument>().rootVisualElement.style.display= DisplayStyle.None;
    }
    void Update() {
        // finds player sanity value.
        int endGame = player.GetComponent<PlayerController>().Sanity;
        // checks if player sanity is 0 and ends game with the "death ending" if so
        if (endGame <= 0) {
            EndGame(Ending.death);
        }

        
    }


    // function RestartGame to be triggered when restart button is clicked
    public void RestartGame() {
        player = GameObject.FindGameObjectWithTag("Player");

        // resume function from pause script
        pause.resumeGame();
        //sets sanity to 5 to avoid being stuck in game over screen
        player.GetComponent<PlayerController>().ChangeSanity(5);
        //restarts the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    public void EndGame(Ending endingType) {
        // pauses the game
        pause.pauseGame();
        // sets the player to be unable to move or interact
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;
        
        string imagePath = "Assets/UI/Images/Neutral-Ending.png";
        // sets the menu button to restart the game
        menuButton.RegisterCallback<ClickEvent>(evt =>{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        // finds the correct image path for the ending type
        if(endingType == Ending.death) {
            Debug.Log("Death Ending");
            imagePath = "Images/Death-Ending";
        } else if(endingType == Ending.bad) {
            Debug.Log("Bad Ending");
            imagePath = "Images/Bad-Ending";
        } else if(endingType == Ending.good) {
            Debug.Log("Good Ending");
            imagePath = "Images/Good-Ending";
        } else if(endingType == Ending.neutral) {
            Debug.Log("Neutral Ending");
            imagePath = "Images/Neutral-Ending";
        }
        // loads the image from the path
        Texture2D endingImage = Resources.Load<Texture2D>(imagePath);
        if (endingImage == null) {
            Debug.LogError("Failed to load image at path: " + imagePath);
        } else {
            // sets the image to be the ending image
            endUI.Q<VisualElement>("EndingImage").style.backgroundImage = new StyleBackground(endingImage);
        }
        // sets the endUI to be visible
        endUI.style.display = DisplayStyle.Flex;
    }
}

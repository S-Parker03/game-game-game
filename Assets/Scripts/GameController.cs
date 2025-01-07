// using Palmmedia.ReportGenerator.Core;
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
    // finds player object and sets game over screen to false
        player = GameObject.FindGameObjectWithTag("Player");
        endScreenobj = GameObject.Find("EndScreen");
        endUI = endScreenobj.GetComponent<UIDocument>().rootVisualElement;
        menuButton = endUI.Q<Button>("MenuButton");
        endUI.style.display = DisplayStyle.None;
        GameOver.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        // player.SetActive(false);
        
        pause.pauseGame();
        GameObject.Find("Settings").GetComponent<UIDocument>().rootVisualElement.style.display= DisplayStyle.None;
    }
    void Update() {
        // finds player sanity value.
        int endGame = player.GetComponent<PlayerController>().Sanity;
        // checks if player sanity is 0 and ends game if so
        if (endGame <= 0) {
            EndGame(Ending.death);
        }
    }


    // function RestartGame to be triggered when restart button is clicked
    public void RestartGame() {
        player = GameObject.FindGameObjectWithTag("Player");
        GameOver.SetActive(false);
        // resume function from pause script
        pause.resumeGame();
        //sets sanity to 5 to avoid being stuck in game over screen
        player.GetComponent<PlayerController>().ChangeSanity(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    public void EndGame(Ending endingType) {
        // finds player object and sets game over screen to true
        // pause function from pause script
        pause.pauseGame();
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;
        string imagePath = "Assets/UI/Images/Neutral-Ending.png";
        menuButton.RegisterCallback<ClickEvent>(evt =>{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
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

        Texture2D endingImage = Resources.Load<Texture2D>(imagePath);
        if (endingImage == null) {
            Debug.LogError("Failed to load image at path: " + imagePath);
        } else {
            endUI.Q<VisualElement>("EndingImage").style.backgroundImage = new StyleBackground(endingImage);
        }

        endUI.style.display = DisplayStyle.Flex;
    }
}

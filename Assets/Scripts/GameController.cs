using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    GameObject player;
    public GameObject GameOver;
    public GameObject RestartButton;
    public Pause pause;

    void Start()
    {
    // finds player object and sets game over screen to false
        player = GameObject.FindGameObjectWithTag("Player");
        GameOver.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update() {
        // finds player sanity value.
        int endGame = player.GetComponent<PlayerController>().Sanity;
        // checks if player sanity is 0 and ends game if so
        if (endGame <= 0) {
            GameOver.SetActive(true);
            // pause function from pause script
            pause.pauseGame();
            
        }
    }


    // function RestartGame to be triggered when restart button is clicked
    public void RestartGame() {
        GameOver.SetActive(false);
        // resume function from pause script
        pause.resumeGame();
        //sets sanity to 5 to avoid being stuck in game over screen
        player.GetComponent<PlayerController>().ChangeSanity(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }
}

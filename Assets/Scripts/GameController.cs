using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour
{
    GameObject player;
    public GameObject GameOver;
    public GameObject RestartButton;
    // TextMeshProUGUI dependencyText;
    // TextMeshProUGUI sanityText;
    public Pause pause;

    void Start()
    {
      //Finding the player
      player = GameObject.FindGameObjectWithTag("Player");
    //   sanityText = GameObject.Find("SanityText").GetComponent<TextMeshProUGUI>();
    //   dependencyText = GameObject.Find("DependencyText").GetComponent<TextMeshProUGUI>();
    //   pause = player.GetComponent<Pause>();
      GameOver.SetActive(false);
    }
    void Update() {
        //
        int endGame = player.GetComponent<PlayerController>().Sanity;
        if (endGame <= 0) {
            GameOver.SetActive(true);
            pause.pauseGame();
        }
    }

    public void RestartGame() {
        Debug.Log("test");
        GameOver.SetActive(false);
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        // player.GetComponent<PlayerController>().Sanity = 7;
        // sanityText.text = "Sanity: " + player.GetComponent<PlayerController>().Sanity.ToString() + " / 10";
        // Slider dependencyRestart = GameObject.Find("DependencyBar").GetComponent<Slider>();
        // dependencyRestart.value = 0;
        // dependencyText.text = "Dependency: " + dependencyRestart.ToString() + "%";
        // GameOver.SetActive(false);
    }
}

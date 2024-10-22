using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour
{
    GameObject player;
    public GameObject GameOver;
    TextMeshProUGUI dependencyText;
    TextMeshProUGUI sanityText;

    void Start()
    {
      //Finding the player
      player = GameObject.FindGameObjectWithTag("Player");
      sanityText = GameObject.Find("SanityText").GetComponent<TextMeshProUGUI>();
      dependencyText = GameObject.Find("DependencyText").GetComponent<TextMeshProUGUI>();
      GameOver.SetActive(false);
    }
    void Update() {
        //
        int endGame = player.GetComponent<PlayerController>().Sanity;
        //if sanity <= 0, game over screen, button, and text display
        if (endGame <= 0) {
            GameOver.SetActive(true);
        }
    }

    public void RestartGame() {
        Debug.Log("test");
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

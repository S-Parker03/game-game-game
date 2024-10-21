using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameController : MonoBehaviour
{
    GameObject player;
    public GameObject GameOver;
    void Start()
    {
      //Finding the player
      player = GameObject.FindGameObjectWithTag("Player");
      GameOver.SetActive(false);
    }
    void Update() {
        //
        int endGame = player.GetComponent<PlayerController>().Sanity;
        Debug.Log("testA");
        //if sanity <= 0, game over screen, button, and text display
        if (endGame <= 0) {
            Debug.Log("test");
            GameOver.SetActive(true);
        }
    }
}

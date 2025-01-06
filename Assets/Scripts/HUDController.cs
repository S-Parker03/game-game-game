using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    GameObject player;

    public TextMeshProUGUI sanityText ;
    public TextMeshProUGUI dependencyText;

    void Start()
    {
      //Finding the player
      player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
      //updating HUD elements
        int sanity = player.GetComponent<PlayerController>().Sanity;
        int dependency = (int)Math.Round(player.GetComponent<Dependency>().DependencyPercent);

    }

}

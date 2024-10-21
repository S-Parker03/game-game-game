using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
        sanityText.text = "Sanity: " + sanity.ToString() + " / 10";
        int dependency = (int)Math.Round(player.GetComponent<Dependency>().DependencyPercent);
        dependencyText.text = "Dependency: " + dependency.ToString() + "%";
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    GameObject player;

    public TextMeshProUGUI sanityText ;

    void Start()
    {
      player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        int sanity = player.GetComponent<PlayerController>().Sanity;
        sanityText.text = "Sanity: " + sanity.ToString() + " / 10";
    }
}

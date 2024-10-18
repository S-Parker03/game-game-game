using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{

    public GameObject player;
    public NavMeshAgent agent;

    bool active;
    // Start is called before the first frame update
    void Start()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(active){
            chasePlayer();
        }
    }

    private void Awake(){
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>(); 
    }

    private void chasePlayer(){
        agent.SetDestination(player.transform.position);
        
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Player"){
            attackPlayer();
        }
    }
    private void attackPlayer(){
        player.GetComponent<PlayerController>().ChangeSanity(-1);
        
    }
}

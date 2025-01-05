
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{

    public GameObject player;
    public NavMeshAgent agent;

    public bool hasSight;

    public GameObject sightCone;

    public string Mode;

    public List<Vector3> patrolPoints;

    private int oldDestination;

    public enum AttackStyle { KickOut, Hurt, Kill };

    public AttackStyle attackStyle;

    bool active;
    // Start is called before the first frame update
    void Start()
    {
        patrolPoints = new List<Vector3> { new Vector3(-2.8f, 0f, -13.111f), new Vector3(-8.08f, 0f, -7.05f) };
        active = true;
        oldDestination = 0;
        if (hasSight){
            sightCone = this.transform.Find("SightCone").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(active){
            if (Mode == "Chase"){ // Mode is a variable that will be set to "Chase" when the player is in the enemy's line of sight
                chasePlayer();
            }else if (Mode == "Patrol"){ // Mode is a variable that will be set to "Patrol" when the player is not in the enemy's line of sight
                if (hasSight && sightCone.GetComponent<SightCone>().canSeePlayer){
                    //cast a ray to check if there is an object between player and enemy
                    RaycastHit hit;
                    Vector3 directionToPlayer = player.transform.position - transform.position;
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit))
                    {
                        if (hit.collider.gameObject == player)
                        {
                            Mode = "Chase";
                            if(agent.speed < 1){
                                agent.speed = 6;
                                agent.acceleration = 8;
                                agent.angularSpeed = 10;
                            }
                        }
                    }
                } else {
                patrol();
                }
            }
        }
        //face the enemy the direction it is moving in
    
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            agent.transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
    }

    //runs when the object is enabled
    private void Awake(){
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>(); 
    }

    //using NavMesh to chase the player
    private void chasePlayer(){
        agent.SetDestination(player.transform.position);
        
    }

    //patrol function, will eventually have a set path but for now just moves the enemy around randomly
    private void patrol(){

        if(enemyReachedDestination()){
            setNewDestination();
        } else {
            agent.SetDestination(agent.destination);
        }

    }

    //checks if the enemy has reached its destination
    private bool enemyReachedDestination(){
        if (!agent.pathPending){
            if (agent.remainingDistance <= 0.5){
                return true;
            }
        }
        return false;
    }

    //sets a new destination for the enemy to move to
    private void setNewDestination(){
        Debug.Log("Old Destination: " + patrolPoints.IndexOf(agent.destination));
        if( oldDestination == 0){
            oldDestination = 1;
            agent.SetDestination(patrolPoints[1]);
        } else {
            oldDestination = 0;
            agent.SetDestination(patrolPoints[0]);
        }
        Debug.Log("New Destination Set to: " + agent.destination);
    }

    //manages collision with player
    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Player"){
            attackPlayer(attackStyle);
        }
    }

    //attack function, eventually with have animaitons etc but for now just decreases sanity.
    private void attackPlayer(AttackStyle style){
        if (style == AttackStyle.KickOut){
            player.GetComponent<PlayerController>().ChangeSanity(-1);
            player.transform.position = new Vector3(0.0478f, 0.71122f, -5.641247f);
            agent.speed = 0.75f;
            agent.acceleration = 0.75f;
            agent.angularSpeed = 1;
            Mode = "Patrol";
            agent.SetDestination(patrolPoints[1]);
        }
        else if(style == AttackStyle.Hurt){
            player.GetComponent<PlayerController>().ChangeSanity(-1);
        }
        else if(style == AttackStyle.Kill){
            player.GetComponent<PlayerController>().ChangeSanity(-9999999);
        }
    }

    public void setActive(bool value){
        active = value;
    }
}

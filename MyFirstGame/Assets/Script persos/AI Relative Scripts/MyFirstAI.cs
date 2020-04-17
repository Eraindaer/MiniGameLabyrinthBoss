using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyFirstAI : MonoBehaviour
{   
    //GameObject et Components pour l'IA
    public NavMeshAgent agent;
    public GameObject origin;
    public GameObject currentDestination;
    public GameObject player;
    Renderer rend;

    //Variables de l'IA
    public int _HP = 3;
    private bool isHit;

        // Start is called before the first frame update
    void Start()
    {
        currentDestination = origin;
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {       
        if(rend.material.color == Color.white)
        {
            isHit = false;
        }

        if(rend.material.color == Color.red && isHit == false)
        {
            _HP--;
            isHit = true;
        }
        if (_HP == 0)
        {
            gameObject.SetActive(false);
            player.GetComponent<FirstPerson>().score += 15;
            rend.material.SetColor("_Color", Color.gray);
            agent.isStopped = true;
        }
        if (Vector3.Distance(player.transform.position, transform.position) <= 10f)
        {
            currentDestination = player;
        }
        if (Vector3.Distance(player.transform.position, transform.position) >= 30f)
        {
            currentDestination = origin;
        }
        agent.SetDestination(currentDestination.transform.position);
    }
}

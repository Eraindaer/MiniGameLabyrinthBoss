using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TorusGenerator : MonoBehaviour
{
    //Variables propres aux différents timer
    public Text TimerSeen;
    public float timer = 0;
    public float bossTimer;
    public float waveTimer;
    private float pauseTimer;
    private bool BeginTimer;

    //Variables des tores et du boss
    public List<GameObject> cloneList = new List<GameObject>();
    System.Random position = new System.Random();
    System.Random rand = new System.Random();   
    public Transform instancePos;
    Transform[] TorusPositions;
    public GameObject TorusList;
    public GameObject WallRef;
    public GameObject cube;
    public GameObject cubeList;
    public GameObject finish;
    private GameObject clone;
    public int randomInit;
    public bool boss;
    public int spawners;
    
    //Tests de paramètres
    public int test;
    public float test1, test2;
    
    //Variables du joueur
    public GameObject player;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        //Initialisation des components et de variables dans les components
        TorusPositions = TorusList.GetComponentsInChildren<Transform>();
        cube.GetComponent<Primitives>().radMax = Vector3.Distance(WallRef.transform.position, cube.transform.position)* Mathf.Sqrt(5);

        //Trace des spawners aléatoires qui ont été supprimés peu après
        foreach (Transform tp in TorusPositions)
        {
            spawners++;
        }
        
        //Initialisation des timers et des rand
        randomInit = 1000;
        bossTimer = 30f;
        waveTimer = 0;
        pauseTimer = 3f;
        boss = true;
        BeginTimer = false;
        
        //statut de certains GameObjects
        finish.SetActive(false);
        cube.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {  
        test = cloneList.Count;
        if(cloneList.Count != 0)
            for (int i = 0; i < cloneList.Count; i++)
            {
                test1 = Mathf.Abs(Vector3.Distance(cloneList[i].transform.position, player.transform.position) - cloneList[i].GetComponent<Primitives>().segmentRadius);
                test2 = Mathf.Abs(Mathf.Abs(player.transform.position.y - cloneList[i].transform.position.y) - player.transform.localScale.y);
                if ((Mathf.Abs(Vector3.Distance(cloneList[i].transform.position, player.transform.position) - cloneList[i].GetComponent<Primitives>().segmentRadius) <= cloneList[i].GetComponent<Primitives>().tubeRadius * 2) && (Mathf.Abs(Mathf.Abs(player.transform.position.y - cloneList[i].transform.position.y) - player.transform.localScale.y) <= cloneList[i].GetComponent<Primitives>().tubeRadius) && (hit==false))
                {               
                    hit = true;
                    timer = 2f;
                    player.GetComponent<JumpingGame>()._hp--;
                }
                if (timer > 0 && hit == true)
                {
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        timer = 0;
                        hit = false;
                    }
                }
                        if (cloneList[i].GetComponent<Primitives>().turn != 1)
                {              
                    Destroy(cloneList[i]);
                    cloneList.Remove(cloneList[i]);
                    break;
               }
            }

        if (rand.Next(randomInit) <= 50 && cloneList.Count < 5 && waveTimer == 0 && BeginTimer == true && boss == true)
        {           
            clone = Instantiate(cube, TorusPositions[position.Next(spawners)].position, Quaternion.identity) as GameObject;
            clone.SetActive(true);
            clone.transform.SetParent(cubeList.transform);
            clone.GetComponent<Primitives>().enabled = true;
            clone.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            cloneList.Add(clone);
            waveTimer = 0.85f;
        }

        if (pauseTimer > 0)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer < 0)
            {
                pauseTimer = 0;
                BeginTimer = true;
            }
        }

        if (waveTimer > 0)
        {
            waveTimer -= Time.deltaTime;
            if(waveTimer < 0)
            {
                waveTimer = 0;
            }
        }

        if(randomInit > 1f)
        {
            randomInit -= 10;
        }

        if (bossTimer > 0 && boss == true && BeginTimer == true)
        {
            bossTimer -= Time.deltaTime;
            if (bossTimer < 0)
            {
                bossTimer = 0;
                boss = false;
                BeginTimer = false;
                cubeList.SetActive(false);
            }
        }

        int showedTimer = (int)bossTimer;
        TimerSeen.text = showedTimer.ToString();
    }
}

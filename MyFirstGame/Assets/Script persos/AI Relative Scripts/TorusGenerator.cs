using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TorusGenerator : MonoBehaviour
{
    public Text TimerSeen;
    public GameObject cube;
    public GameObject cubeList;
    GameObject clone;
    public List<GameObject> cloneList = new List<GameObject>();
    public Transform instancePos;
    public int test;
    System.Random position = new System.Random();

    Transform[] TorusPositions;
    GameObject finalBoss;

    public GameObject TorusList;
    public int spawners;

    float distWithWall;
    public GameObject WallRef;

    public GameObject player;
    public bool hit;
    public float test1, test2;
    public float timer = 0;
    public float bossTimer;
    public float waveTimer;
    private float pauseTimer;
    bool boss;
    public GameObject finish;
    public int randomInit;
    private bool BeginTimer;
    System.Random rand = new System.Random();

    

    // Start is called before the first frame update
    void Start()
    {
        cube.SetActive(false);
        TorusPositions = TorusList.GetComponentsInChildren<Transform>();
        foreach(Transform tp in TorusPositions)
        {
            spawners++;
        }
        distWithWall = Vector3.Distance(WallRef.transform.position, cube.transform.position);
        cube.GetComponent<Primitives>().radMax = distWithWall*Mathf.Sqrt(2);
        randomInit = 1000;
        boss = true;
        bossTimer = 30f;
        waveTimer = 0;
        pauseTimer = 3f;
        BeginTimer = false;
        finalBoss = GameObject.Find("Boss");
        finish.SetActive(false);
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

        if (rand.Next(randomInit) <= 50 && cloneList.Count < 5 && boss == true && waveTimer == 0 && BeginTimer == true)
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
                player.GetComponent<JumpingGame>().score += 50;
            }
        }

        if (boss == false)
        {
            finalBoss.SetActive(false);
            finish.SetActive(true);
            

        }


        if (Mathf.Abs(finish.transform.position.x - player.transform.position.x) <= 2f && Mathf.Abs(finish.transform.position.z - player.transform.position.z) <= 2f)
        {
            ScoreGestionner.Instance.Score += player.GetComponent<JumpingGame>().score;
            SceneManager.LoadScene(4, LoadSceneMode.Single);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        int timer3003Officiel = (int)bossTimer;
        TimerSeen.text = timer3003Officiel.ToString();
    }
}

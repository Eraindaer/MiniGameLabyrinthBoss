using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPerson : MonoBehaviour
{
    //GameObject & Components
    GameObject canvas;
    Renderer[] rendAI;
    Renderer[] rendButton;
    Renderer[] rendItem;
    Transform[] bonuses;
    GameObject AI;
    GameObject Button;
    GameObject Item;
    GameObject Finish;
    GameObject Bonus;

    //Mouvement du joueur
    private CharacterController _controller;
    public float Speed = 10f;

    //Spot IA, bouton, item
    public Vector3 AIPos;
    public Vector3 RayHitPoint;
    public bool toChange = false;
    public bool buttonSpotted = false;
    public bool itemSpotted = false;
    public float timer = 0; 

    //Gestion des variables liées au saut

    public float verticalVelocity;
    public float gravity = 7.0f;
    public float jumpForce = 3.5f;
    private float jumpBoost = 1.0f;
    private float fallAcceleration = 1.005f;

    //Variables liées aux items

    Vector3 originalBootsPos;
    Vector3 originalGunPos;
    public GameObject boots;
    public GameObject gun;
    private bool bootsEquiped = false;
    private bool gunEquiped = false;

    public int _hp = 3;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        ScoreGestionner.Instance.Score = score;
        //Attribution des gameobject/components
        canvas = GameObject.Find("UI");
        AI = GameObject.Find("AIs");
        Button = GameObject.Find("Buttons");
        Item = GameObject.Find("Items");
        Finish = GameObject.Find("Finish");
        Bonus = GameObject.Find("Bonuses");
        rendAI = AI.GetComponentsInChildren<Renderer>();
        rendButton = Button.GetComponentsInChildren<Renderer>();
        rendItem = Item.GetComponentsInChildren<Renderer>();
        _controller = GetComponent<CharacterController>();
        bonuses = Bonus.GetComponentsInChildren<Transform>();

        //Gestion objets
        originalBootsPos = boots.transform.position;
        originalGunPos = gun.transform.position;

    }

 
    // Update is called once per frame
    void Update()
    {
        //Gestion du déplacement du joueur et de ses sauts
        Vector3 move = Vector3.zero;

        float speedFactor = 1.0f;
        
        if (Input.GetKey(KeyCode.LeftShift))
             speedFactor = 2.5f;
        if (Input.GetKey(KeyCode.LeftControl))
            speedFactor = 0.5f;

        float totalSpeed = Speed * speedFactor;

        if (Input.GetKey(KeyCode.Z))
            move += (transform.forward)*totalSpeed;
        if (Input.GetKey(KeyCode.S))
            move -= (transform.forward)*totalSpeed;
        if (Input.GetKey(KeyCode.Q))
            move -= (transform.right)*totalSpeed;
        if (Input.GetKey(KeyCode.D))
            move += (transform.right)*totalSpeed;
        
        if (_controller.isGrounded)
        {
            gravity = 7.0f;
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;            
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            gravity*=fallAcceleration;
        }

        if (bootsEquiped == true)
        {
            jumpBoost = 15.0f ;
        }

        move.y = verticalVelocity * jumpBoost;

        _controller.Move(move * Time.deltaTime);

        //Gestion des bonus

        foreach(Transform t in bonuses)
        {
            int h = 0;
            if (Vector3.Distance(transform.position, Bonus.transform.GetChild(h).position) <= 2.5f)
            {
                Bonus.transform.GetChild(h).position = new Vector3(Bonus.transform.GetChild(h).position.x, Bonus.transform.GetChild(h).position.y - 15f, Bonus.transform.GetChild(h).position.z) ;
                score += 10;
            }
            h++;
        }

        //Gestion du viseur du joueur (et du hitmarker)
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(canvas.transform.position);
        AIPos = AI.transform.position;
      
        if (Physics.Raycast(ray, out hit))
        {
            RayHitPoint = hit.point;

            if (hit.collider.tag == "Button")
            {
                int j = 0;
                foreach(Renderer r in rendButton)
                {
                    if (Vector3.Distance(Button.transform.GetChild(j).position, RayHitPoint) <= Button.transform.GetChild(j).localScale.x){
                        r.material.SetColor("_Color", Color.cyan);
                        buttonSpotted = true;
                    }
                    j++;
                }
            }
            if (hit.collider.tag != "Button" && buttonSpotted == true)
            {
                foreach(Renderer r in rendButton)
                {
                    
                    r.material.SetColor("_Color", Color.white);
                    buttonSpotted = false;
                }
            }

            if (hit.collider.tag == "Item")
            {
                int k = 0;
                foreach (Renderer r in rendItem)
                {
                    if (Vector3.Distance(Item.transform.GetChild(k).position, RayHitPoint) <= Item.transform.GetChild(k).localScale.x)
                    { 
                    r.material.SetColor("_Color", Color.yellow);
                    itemSpotted = true;
                    }
                    k++;
                }
                    
            }

            if (hit.collider.tag != "Item" && itemSpotted == true)
            {
                foreach(Renderer r in rendItem)
                {
                    r.material.SetColor("_Color", Color.white);
                    itemSpotted = false;
                }
            }
                 
            if (Input.GetMouseButtonDown(0) && gunEquiped == true)
            {
                

                if (hit.collider.tag == "Enemy")
                {

                    
                    
                    if (hit.collider != null)
                    {
                        int i = 0;
                        foreach (Renderer r in rendAI)
                        {

                            if (Vector3.Distance(AI.transform.GetChild(i).position, RayHitPoint) <= AI.transform.GetChild(i).localScale.x)
                            {
                                r.material.SetColor("_Color", Color.red);
                                toChange = true;
                                timer = 0.1f;
                            }
                            
                            i++;
                        }
                    }
                }

            }
        }
        if (timer > 0 && toChange == true)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
                toChange = false;
                foreach (Renderer r in rendAI)
                {
                    r.material.SetColor("_Color", Color.white);
                }
                foreach (Renderer r in rendButton)
                {
                    r.material.SetColor("_Color", Color.white);
                }
            }
        }

        //Gestion de la mort

        if ((transform.position.y <= -20f)||(_hp == 0))
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //Gestion des bottes et de l'arme

        Vector3 currentBootsPosition = boots.transform.position;

        if (currentBootsPosition != originalBootsPos)
        {
            bootsEquiped = true;
        }

        Vector3 currentGunPosition = gun.transform.position;

        if (currentGunPosition != originalGunPos)
        {
            gunEquiped = true;
        }

        //Gestion de l'arrivée

        if (Mathf.Abs(transform.position.x - Finish.transform.position.x) <= 2 && Mathf.Abs(transform.position.z - Finish.transform.position.z) <= 2)
        {
            ScoreGestionner.Instance.Score += score;
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        }

    }
}
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPerson : MonoBehaviour
{
    //GameObject & Components
    LayerMask mask;
    Transform[] bonusTransform;
    Transform[] buttonTransform;
    Transform[] AITransform;
    Transform[] itemTransform;

    public GameObject AI;
    public GameObject Button;
    public GameObject Item;
    public GameObject Finish;
    public GameObject Bonus;
    public GameObject canvas;

    public List<GameObject> bonusList;
    public List<GameObject> buttonList;
    public List<GameObject> AIList;
    public List<GameObject> itemList;

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
    private float jumpBoost = 3.0f;
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
        _controller = GetComponent<CharacterController>();
        bonusTransform = Bonus.GetComponentsInChildren<Transform>();
        buttonTransform = Button.GetComponentsInChildren<Transform>();
        AITransform = AI.GetComponentsInChildren<Transform>();
        itemTransform = Item.GetComponentsInChildren<Transform>();

        //Gestion objets
        originalBootsPos = boots.transform.position;
        originalGunPos = gun.transform.position;
        mask = LayerMask.GetMask("Interactible Items");

        //Initialisation des listes

        if (bonusList == null)
            bonusList = new List<GameObject>();
        if (buttonList == null)
            buttonList = new List<GameObject>();
        if (AIList == null)
            AIList = new List<GameObject>();
        if (itemList == null)
            itemList = new List<GameObject>();

        foreach (Transform t in bonusTransform)
        {
            bonusList.Add(t.gameObject);
        }
        foreach(Transform t in buttonTransform)
        {
            buttonList.Add(t.gameObject);
        }
        foreach(Transform t in AITransform)
        {
            AIList.Add(t.gameObject);
        }
        foreach(Transform t in itemTransform)
        {
            itemList.Add(t.gameObject);
        }
        bonusList.Remove(bonusList[0]);
        buttonList.Remove(buttonList[0]);
        AIList.Remove(AIList[0]);
        itemList.Remove(itemList[0]);
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

        foreach(GameObject g in bonusList)
        { 
            if (Vector3.Distance(transform.position, g.transform.position) <= 2.5f)
            {
                g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y - 15f, g.transform.position.z) ;
                score += 10;
            }
        }

        //Gestion du viseur du joueur (et du hitmarker)
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(canvas.transform.position);
        AIPos = AI.transform.position;
      
        if (Physics.Raycast(ray, out hit, 10000.0f, mask))
        {
            RayHitPoint = hit.point;

            if (hit.collider.tag == "Button")
            {
                foreach(GameObject g in buttonList)
                {
                    if (Vector3.Distance(g.transform.position, RayHitPoint) <= g.transform.localScale.x){
                        g.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                        buttonSpotted = true;
                    }
                }
            }
            if (hit.collider.tag != "Button" && buttonSpotted == true)
            {
                foreach(GameObject g in buttonList)
                {
                    g.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    buttonSpotted = false;
                }
            }

            if (hit.collider.tag == "Item")
            {
                foreach (GameObject g in itemList)
                {
                    if (Vector3.Distance(g.transform.position, RayHitPoint) <= g.transform.localScale.x)
                    {
                    g.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                    itemSpotted = true;
                    }
                }
                    
            }

            if (hit.collider.tag != "Item" && itemSpotted == true)
            {
                foreach(GameObject g in itemList)
                {
                    g.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    itemSpotted = false;
                }
            }
                 
            if (Input.GetMouseButtonDown(0) && gunEquiped == true)
            {              
                if (hit.collider.tag == "Enemy")
                { 
                    if (hit.collider != null)
                    {
                        foreach (GameObject g in AIList)
                        {
                            if (Vector3.Distance(g.transform.position, RayHitPoint) <= g.transform.localScale.x)
                            {
                                g.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                                toChange = true;
                                timer = 0.1f;
                            } 
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
                foreach (GameObject g in AIList)
                {
                    g.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
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
 
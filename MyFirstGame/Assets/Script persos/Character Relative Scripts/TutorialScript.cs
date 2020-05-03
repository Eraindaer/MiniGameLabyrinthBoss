using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    //Components des gameObjects
    private CharacterController _controller;
    Renderer rendAI;
    Renderer rendButton;
    Renderer rendGun;
    Transform[] sphereTransform;

    //Listes
    List<GameObject> sphereList;

    //GameObjects à assigner
    public GameObject AI;
    public GameObject Button;
    public GameObject Item;
    public GameObject Sphere;
    public GameObject gun;
    public GameObject canvas;
    public GameObject FinishWithoutSprint;
    public GameObject FinishSprint;
    public GameObject FinishJump;
    public GameObject FinalFinish;

    //Variables du mouvement
    public float Speed = 10f;
    private float fallAcceleration = 1.005f;
    public float verticalVelocity;
    public float gravity = 7.0f;
    public float jumpForce = 3.5f;
    private float jumpBoost = 5.0f;

    //booleens propres à la scene
    private bool stage1, stage2, stage3, stage4, stage5, stage6, stage7;
    bool gunEquiped = false;
    bool buttonActivated = false;
    public bool toChange = false;
    public bool buttonSpotted = false;
    public bool itemSpotted = false;
    public bool sphereSpotted = false;

    //Timers
    public float timer, timer2;

    //Vecteurs
    public Vector3 RayHitPoint;
    Vector3 originalButtonPos;
    Vector3 originalGunPos;
    
    //LayerMask
    public LayerMask mask;

    void Start()
    {   
        //Initialisation des etapes de la scene + initialisation des differents éléments de la scene
        stage1 = true;
        stage2 = false;
        stage3 = false;
        stage4 = false;
        stage5 = false;
        stage6 = false;
        stage7 = false;
        FinishWithoutSprint.SetActive(false);
        FinishSprint.SetActive(false);
        FinishJump.SetActive(false);
        FinalFinish.SetActive(false);
        gun.SetActive(false);
        Button.SetActive(false);

        //Assignation des components
        _controller = GetComponent<CharacterController>();
        rendAI = AI.GetComponent<Renderer>();
        rendButton = Button.GetComponent<Renderer>();
        rendGun = gun.GetComponent<Renderer>();
        sphereTransform = Sphere.GetComponentsInChildren<Transform>();

        //Parametres du timer de sprint
        timer2 = 1f;

        //Initialisation vecteurs
        originalButtonPos = Button.transform.position;
        originalGunPos = gun.transform.position;

        rendAI.material.SetColor("_Color", Color.white);
        mask = LayerMask.GetMask("Interactible Items");

        //Initialisation listes
        if(sphereList == null)
        {
            sphereList = new List<GameObject>();
        }

        foreach(Transform t in sphereTransform)
        {
            sphereList.Add(t.gameObject);
        }
        sphereList.Remove(sphereList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        //Gestion des mouvements du joueur, des sauts
        if (stage2 == true) //etape 2 du tutoriel
        {
            Vector3 move = Vector3.zero;
            float speedFactor = 1.0f;

            if (stage3 == true) //etape 3 du tutoriel
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speedFactor = 2.5f;
                    timer2 -= Time.deltaTime;
                    if (timer2 < 0)
                        timer2 = 0;
                }
                if (Input.GetKey(KeyCode.LeftControl))
                    speedFactor = 0.5f;
                if(timer2 == 0 && Mathf.Abs(FinishSprint.transform.position.x - transform.position.x) <= 1.5f && Mathf.Abs(FinishSprint.transform.position.z - transform.position.z) <= 1.5f)
                {
                    FinishSprint.SetActive(false);
                    FinishJump.SetActive(true);
                    stage4 = true;
                }
            }

            float totalSpeed = Speed * speedFactor;

            if (Input.GetKey(KeyCode.Z))
                move += (transform.forward) * totalSpeed;
            if (Input.GetKey(KeyCode.S))
                move -= (transform.forward) * totalSpeed;
            if (Input.GetKey(KeyCode.Q))
                move -= (transform.right) * totalSpeed;
            if (Input.GetKey(KeyCode.D))
                move += (transform.right) * totalSpeed;

            if (Mathf.Abs(transform.position.x - FinishWithoutSprint.transform.position.x) <= 1.5f && Mathf.Abs(transform.position.z - FinishWithoutSprint.transform.position.z) <= 1.5f)
            {
                stage3 = true;
                FinishWithoutSprint.SetActive(false);
                FinishSprint.SetActive(true);
            }

            if (stage4 == true) //etape 4 du tutoriel
            {
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
                    gravity *= fallAcceleration;
                }
                if (Mathf.Abs(transform.position.x - FinishJump.transform.position.x) <= 1.5f && Mathf.Abs(transform.position.z - FinishJump.transform.position.z) <= 1.5f)
                {
                    FinishJump.SetActive(false);
                    stage5 = true;
                    gun.SetActive(true);
                    Button.SetActive(true);
                }
            }

            move.y = verticalVelocity * jumpBoost;
            _controller.Move(move * Time.deltaTime);
        }

        //Gestion interactions avec des objets/bonus/boutons
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(canvas.transform.position);

        if (stage1 == true) //étape 1 du tutoriel
        {
            if (Physics.Raycast(ray, out hit, 10000.0f, mask))
            {
                RayHitPoint = hit.point;

                if (hit.collider.tag == "SkySphere")
                {
                    foreach(GameObject g in sphereList)
                    {
                        if (Vector3.Distance(g.transform.position, RayHitPoint) <= g.transform.localScale.x)
                        {
                            g.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                            sphereSpotted = true;    
                        }
                    }
                    if (sphereSpotted == true)
                    {
                        stage2 = true;
                        FinishWithoutSprint.SetActive(true);
                        Sphere.SetActive(false);
                    }
                }

                if (stage5 == true)
                {
                    if (hit.collider.tag == "Button")
                    {                     
                        if (Vector3.Distance(Button.transform.position, RayHitPoint) <= Button.transform.localScale.x)
                        {
                            rendButton.material.SetColor("_Color", Color.cyan);
                            buttonSpotted = true;
                        }
                        
                    }
                    if (hit.collider.tag != "Button" && buttonSpotted == true)
                    {
                        rendButton.material.SetColor("_Color", Color.white);
                        buttonSpotted = false;

                    }
                    if (hit.collider.tag == "Item")
                    {
                        rendGun.material.SetColor("_Color", Color.yellow);
                        itemSpotted = true;
                    }

                    if (hit.collider.tag != "Item" && itemSpotted == true)
                    {
                        rendGun.material.SetColor("_Color", Color.white);
                        itemSpotted = false;
                    }

                    if(buttonActivated == true && gunEquiped == true)
                    {
                        stage6 = true;
                    }

                    if (stage6 == true)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (hit.collider.tag == "Enemy")
                            {                          
                                if (hit.collider != null)
                                {
                                    if (Vector3.Distance(AI.transform.position, RayHitPoint) <= AI.transform.localScale.x)
                                    {
                                        rendAI.material.SetColor("_Color", Color.red);
                                        toChange = true;
                                        timer = 0.1f;
                                        stage7 = true;
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
                            rendAI.material.SetColor("_Color", Color.white);          
                            rendButton.material.SetColor("_Color", Color.white);
                        }
                    }
                }
            }
        }

        Vector3 currentButtonPosition = Button.transform.position;
        if (currentButtonPosition != originalButtonPos)
        {
            buttonActivated = true;
        }

        Vector3 currentGunPosition = gun.transform.position;
        if (currentGunPosition != originalGunPos)
        {
            gunEquiped = true;
            stage6 = true;
        }

        //Gestion prochaines instructions
        if (stage7 == true)
        {
            FinalFinish.SetActive(true);
            if(Mathf.Abs(transform.position.x - FinalFinish.transform.position.x) <= 1.5f && Mathf.Abs(transform.position.z - FinalFinish.transform.position.z) <= 1.5f)
            {
                SceneManager.LoadScene(2, LoadSceneMode.Single);
            }
        }
    }
}

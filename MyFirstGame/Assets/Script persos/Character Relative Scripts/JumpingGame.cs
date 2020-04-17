using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpingGame : MonoBehaviour
{

    Transform[] HPTransform;

    List<GameObject> HPList = new List<GameObject>();

    //GameObjects et Components
    private CharacterController _controller;
    public GameObject finish;
    public GameObject finalBoss;
    public GameObject TorusGenerator;
    public GameObject canvas;
    public GameObject healthPoints;

    //Variables du joueur
    public float speed;
    public float verticalVelocity;
    public float gravity = 7.0f;
    public float jumpForce = 3.5f;
    public float jumpBoost = 5.0f;
    private float fallAcceleration = 1.005f;
    public int _hp;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        //Initialisation des variables
        score = 0;
        speed = 10f;
        _hp = 3;

        //Initialisation des components
        _controller = GetComponent<CharacterController>();
        HPTransform = healthPoints.GetComponentsInChildren<Transform>();
        
        //Initialisation des listes
        foreach(Transform t in HPTransform)
        {
            HPList.Add(t.gameObject);
        }
        HPList.Remove(HPList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;

        float speedFactor = 1.0f;

        if (Input.GetKey(KeyCode.LeftShift))
            speedFactor = 2.5f;
        if (Input.GetKey(KeyCode.LeftControl))
            speedFactor = 0.5f;

        float totalSpeed = speed * speedFactor;

        if (Input.GetKey(KeyCode.Z))
            move += (transform.forward) * totalSpeed;
        if (Input.GetKey(KeyCode.S))
            move -= (transform.forward) * totalSpeed;
        if (Input.GetKey(KeyCode.Q))
            move -= (transform.right) * totalSpeed;
        if (Input.GetKey(KeyCode.D))
            move += (transform.right) * totalSpeed;

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

        move.y = verticalVelocity * jumpBoost;
        _controller.Move(move * Time.deltaTime);

        if(_hp < 3)
        {
           HPList[_hp].gameObject.SetActive(false);
        }

        if (finalBoss.GetComponent<TorusGenerator>().boss == false)
        {
            finalBoss.SetActive(false);
            finish.SetActive(true);
        }
        

        if (Mathf.Abs(finish.transform.position.x - transform.position.x) <= 2f && Mathf.Abs(finish.transform.position.z - transform.position.z) <= 2f)
        {
            score += 50;
            ScoreGestionner.Instance.Score += score;
            SceneManager.LoadScene(4, LoadSceneMode.Single);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (_hp <= 0)
        {
            score -= 10;
            ScoreGestionner.Instance.Score += score;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}

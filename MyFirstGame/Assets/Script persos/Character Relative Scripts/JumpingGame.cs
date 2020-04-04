﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpingGame : MonoBehaviour
{
    private CharacterController _controller;
    public GameObject canvas;

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
        score = 0;
        canvas = GameObject.Find("UI");
        _controller = GetComponent<CharacterController>();
        speed = 10f;
        _hp = 3;
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
            canvas.transform.GetChild(_hp).gameObject.SetActive(false);
        }

        if (_hp == 0)
        {
            ScoreGestionner.Instance.Score -= 10;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
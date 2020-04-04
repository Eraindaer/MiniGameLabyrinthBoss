using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 anglesToRotate;
    GameObject cube;
    Renderer rend;
    Animator anim;
    Light sptLight;
    GameObject canvas;
    public Vector3 CubePos, RayHitPoint;
    public float HitBox;
    public float timer = 0;
    public bool inAnimation = false;

    void Start()
    {
        cube = GameObject.Find("Cube");
        rend = cube.GetComponent<Renderer>();
        anim = cube.GetComponent<Animator>();
        sptLight = cube.GetComponentInChildren<Light> (true);
        canvas = GameObject.Find("UI");
    }

    // Update is called once per frame
    void Update()
    {
       
        rend.material.SetColor("_Color", Color.white);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(canvas.transform.position);
        CubePos = cube.transform.position;

        if (Physics.Raycast(ray, out hit))
        {
           
            RayHitPoint = hit.point;
            HitBox = Vector3.Distance(CubePos, RayHitPoint);
            if (hit.collider != null && HitBox <= cube.transform.localScale.x)
            {
                sptLight.intensity = 20;
                rend.material.SetColor("_Color", Color.cyan);
                if (Input.GetKey(KeyCode.E) && inAnimation == false)
                {
                    anim.Play("CubeRotate1", 0, 0.0f);
                    inAnimation = true;
                    timer = 1;

                }
            }
            else sptLight.intensity = 0;
        }
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                inAnimation = false;
                timer = 1;
            }
        }
    }
}
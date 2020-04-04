using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsSelection : MonoBehaviour
{
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bootsPos = new Vector3(transform.position.x, transform.position.y - 20f, transform.position.z);

        if (rend.material.color == Color.yellow)
        {
            if (Input.GetKey(KeyCode.E))
            {
                transform.position = bootsPos;
                rend.material.SetColor("_Color", Color.white);
            } 

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelection : MonoBehaviour
{
    public GameObject Camera;
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
        Vector3 posCamera = new Vector3(Camera.transform.position.x + 2 , Camera.transform.position.y - 2, Camera.transform.position.z + 2);
        if(rend.material.color == Color.yellow)
        {
            if (Input.GetKey(KeyCode.E))
            {
                transform.position = posCamera;
                rend.material.SetColor("_Color", Color.white);
            
            }

        }
    }
}

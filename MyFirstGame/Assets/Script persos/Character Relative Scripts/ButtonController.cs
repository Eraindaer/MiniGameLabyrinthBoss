using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject Door;
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
        Vector3 posButton = new Vector3((this.transform.position.x), this.transform.position.y-15f, this.transform.position.z);
        Vector3 posDoor = new Vector3(Door.transform.position.x, Door.transform.position.y - 10f, Door.transform.position.z);
        if (rend.material.color == Color.cyan)
        {
            if (Input.GetKey(KeyCode.E))
            {
                this.transform.position = posButton;
                Door.transform.position = posDoor;
                rend.material.SetColor("_Color", Color.white);
            }
        }
    }
}

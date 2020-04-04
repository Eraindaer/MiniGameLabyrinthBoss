using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    public enum RotationAxis {
          MouseXY = 0,
          MouseX = 1,
          MouseY = 2
      }
    public RotationAxis axis = RotationAxis.MouseXY;
    public float horSensitivity;
    public float verSensitivity;
    public float maxVert = 60.0f;
    public float minVert = -60.0f;

    private float rotationX = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
         if (axis == RotationAxis.MouseX) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * horSensitivity, 0);
        }

        else if (axis == RotationAxis.MouseY) {
            rotationX += Input.GetAxis("Mouse Y") * verSensitivity;
            rotationX = Mathf.Clamp(rotationX, minVert, maxVert);
            float rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
        else {
            rotationX += Input.GetAxis("Mouse Y") * verSensitivity;
            rotationX = Mathf.Clamp(rotationX, minVert, maxVert);
            float delta = Input.GetAxis("Mouse X") * horSensitivity;
            float rotationY = transform.localEulerAngles.y + delta;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
    }
}
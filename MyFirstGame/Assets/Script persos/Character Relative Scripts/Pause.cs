using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private bool pause = false;
    GameObject pauseObject;
    GameObject UI;

    private void Start()
    {
        UI = GameObject.Find("UI");
        pauseObject = GameObject.Find("Pause");
        pauseObject.SetActive(false);
        UI.SetActive(true);
        pause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            pause = !pause;
           if (pause == true)
            {
                
                pauseObject.SetActive(true);
                UI.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }


            else
            {
                pauseObject.SetActive(false);
                UI.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }

        }
        
    }
    public void pauseControl()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        Time.timeScale = 1;
        pauseObject.SetActive(false);
        UI.SetActive(true);
        pause = false;
        
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

}

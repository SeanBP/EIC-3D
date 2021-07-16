using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PauseMenu : MonoBehaviour
{

    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject credits;
    public GameObject eventMenu;
    public GameObject detectorMenu;
    //private bool EventLoaded = false;
    private GameObject[] hits = null;
    private bool animating = false;
    private float[] timeList;
    private float start_time = 0f;
    private bool clearing = false;
    private bool duration = false;
    private float lastSliderValue = 1;

    // Update is called once per frame

    void Start()
    {
        Resume();
    }
    void Update()
    {      
       if (Input.GetButtonDown("Fire3"))
       {
                if (GameIsPaused)
                {
                    Resume();
                }
            else
            {
                Pause();
            }
        }
        
    }


    void Resume()
    {
        pauseMenuUI.SetActive(false);
        eventMenu.SetActive(false);
        detectorMenu.SetActive(false);
        //Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        if (eventMenu.activeSelf == false && detectorMenu.activeSelf == false)
        {
            pauseMenuUI.SetActive(true);
        }
        //Time.timeScale = 0f;
        GameIsPaused = true;
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EventMenu()
    {
        eventMenu.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void EventMenuBack()
    {
        eventMenu.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
    public void DetectorMenu()
    {
        detectorMenu.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void DetectorMenuBack()
    {
        detectorMenu.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void Credits()
    {
        if (credits.active)
        {
            credits.SetActive(false);
        }
        else
        {
            credits.SetActive(true);
        }
    }

    public void ExpandZ(float newValue)
    {
        GameObject[] detectorParts = GameObject.FindGameObjectsWithTag("Detector");
        
        for (var i = 0; i < detectorParts.Length; i++)
        {
            Vector3 lastPosition = detectorParts[i].transform.position;
            detectorParts[i].transform.localPosition = new Vector3(lastPosition.x, lastPosition.y, (lastPosition.z/lastSliderValue)*newValue);
        }
        
        lastSliderValue = newValue;
    }
    public void XYScale(float newValue)
    {
        GameObject[] detectorParts = GameObject.FindGameObjectsWithTag("Detector");
        Vector3 newScale = new Vector3(newValue, newValue, 1);

        for (var i = 0; i < detectorParts.Length; i++)
        {
            detectorParts[i].transform.localScale = newScale;
        }


    }


    public void ChangeDetectorOpacity(float alpha)
    {
        GameObject[] detectorParts = GameObject.FindGameObjectsWithTag("Detector");
        for (var i = 0; i < detectorParts.Length; i++)
        {
            Material oldMaterial = detectorParts[i].GetComponent<MeshRenderer>().material;
            Material newMaterial = new Material(Shader.Find("Transparent/Diffuse"));
            Color newColor = oldMaterial.color;
            newColor.a = alpha;
            newMaterial.color = newColor;
            newMaterial.renderQueue = oldMaterial.renderQueue;

           detectorParts[i].GetComponent<MeshRenderer>().material = newMaterial;

        }
        
    }
}

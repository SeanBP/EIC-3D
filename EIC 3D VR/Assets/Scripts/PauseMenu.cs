using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
#pragma warning disable 0618

public class PauseMenu : MonoBehaviour
{

    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject credits;
    public GameObject eventMenu;
    public GameObject detectorMenu;
    public GameObject controls;
    public GameObject AudioSource;
    public GameObject menuText;
    public Slider volume;
    private float[] timeList;

 
    private float lastSliderValue = 1;
    GameObject[] menagerie;
    

    // Update is called once per frame

    void Start()
    {
        menagerie = GameObject.FindGameObjectsWithTag("Menagerie");    
        Resume();
    }
    void Update()
    {
        AudioSource.GetComponent<AudioSource>().volume = volume.value;
       
        
    }


    void Resume()
    {
        pauseMenuUI.SetActive(false);
        eventMenu.SetActive(false);
        detectorMenu.SetActive(false);
        GameIsPaused = false;

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
  
    }

    

    public void EventMenu()
    {
        eventMenu.SetActive(true);
        pauseMenuUI.SetActive(false);
        credits.SetActive(false);
        controls.SetActive(false);
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
        credits.SetActive(false);
        controls.SetActive(false);
    }
    public void DetectorMenuBack()
    {
        detectorMenu.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        controls.SetActive(false);
        if (credits.active)
        {
            credits.SetActive(false);
        }
        else
        {
            credits.SetActive(true);
        }
    }

    public void Controls()
    {
        credits.SetActive(false);
        if (controls.active)
        {
            controls.SetActive(false);
        }
        else
        {
            controls.SetActive(true);
        }
    }

    public void ExpandZ(float newValue)
    {
        GameObject[] detectorParts = GameObject.FindGameObjectsWithTag("Detector");
        GameObject[] largegridOrigin = GameObject.FindGameObjectsWithTag("LargeGridOrigin");
        GameObject[] finegridOrigin = GameObject.FindGameObjectsWithTag("FineGridOrigin");


        for (int i = 0; i < detectorParts.Length; i++)
        {
            Vector3 lastPosition = detectorParts[i].transform.position;
            detectorParts[i].transform.localPosition = new Vector3(lastPosition.x, lastPosition.y, (lastPosition.z / lastSliderValue) * newValue);

        }
        if (finegridOrigin.Length > 0)
        {
            Vector3 newScale = new Vector3(finegridOrigin[0].transform.localScale.x, finegridOrigin[0].transform.localScale.y, newValue);
            
                Vector3 lastPosition = finegridOrigin[0].transform.position;
                finegridOrigin[0].transform.localScale = newScale;
            
        }
        if (largegridOrigin.Length > 0)
        {
            Vector3 newScale = new Vector3(largegridOrigin[0].transform.localScale.x, largegridOrigin[0].transform.localScale.y, newValue);
            
            Vector3 lastPosition = largegridOrigin[0].transform.position;
            largegridOrigin[0].transform.localScale = newScale;
            
        }


        lastSliderValue = newValue;
        

        Vector3 newScale3 = new Vector3(menagerie[0].transform.localScale.x, menagerie[0].transform.localScale.y, newValue);
        for (int i = 0; i < menagerie.Length; i++)
        {
            Vector3 lastPosition = menagerie[i].transform.position;
            menagerie[i].transform.localScale = newScale3;
        }

    }
    public void XYScale(float newValue)
    {
        GameObject[] detectorParts = GameObject.FindGameObjectsWithTag("Detector");
        Vector3 newScale = new Vector3(newValue, newValue, 1);

        GameObject[] largegridOrigin = GameObject.FindGameObjectsWithTag("LargeGridOrigin");
        GameObject[] finegridOrigin = GameObject.FindGameObjectsWithTag("FineGridOrigin");

        for (int i = 0; i < detectorParts.Length; i++)
        {
            detectorParts[i].transform.localScale = newScale;
        }
        newScale = new Vector3(newValue, newValue, menagerie[0].transform.localScale.z);
        for(int i = 0; i < menagerie.Length; i++)
        {
            menagerie[i].transform.localScale = newScale;
        }
        if (finegridOrigin.Length > 0)
        {
            newScale = new Vector3(newValue, newValue, finegridOrigin[0].transform.localScale.z);
            
            Vector3 lastPosition = finegridOrigin[0].transform.position;
            finegridOrigin[0].transform.localScale = newScale;  
        }
        if (largegridOrigin.Length > 0)
        {
            newScale = new Vector3(newValue, newValue, largegridOrigin[0].transform.localScale.z);

            Vector3 lastPosition = largegridOrigin[0].transform.position;
            largegridOrigin[0].transform.localScale = newScale;
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

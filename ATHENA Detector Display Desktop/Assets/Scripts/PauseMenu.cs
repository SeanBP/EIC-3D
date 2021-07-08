using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PauseMenu : MonoBehaviour
{

    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
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

        if (clearing)
        {
            DestroyHits();
            clearing = false;
            start_time = Time.time;
        }

        if (animating)
        {
            if (hits == null)
            {
                LoadHits();
            }
            bool hitsLeft = false;
            for (int i = 0; i < hits.Length; i++)
            {
                if (duration == true)
                {
                    if (timeList[i] + 0.1f <= Time.time - start_time)
                    {
                       hits[i].GetComponent<Renderer>().enabled = true;
                    }
                    else
                    {
                        hitsLeft = true;
                    }
                }
                else
                {
                    hits[i].GetComponent<Renderer>().enabled = true;
                }
            }
            if (!hitsLeft)
            {
                animating = false;
            }
        }
        else
        {
            start_time = Time.time;
        }
        
        
        
    }


    void Resume()
    {
        pauseMenuUI.SetActive(false);
        //Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        GameIsPaused = true;
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExpandZ(float newValue)
    {
        GameObject[] detectorParts = GameObject.FindGameObjectsWithTag("Detector");
        
        for (var i = 0; i < detectorParts.Length; i++)
        {
            Vector3 lastPosition = detectorParts[i].transform.position;
            Debug.Log(lastPosition.z);
            detectorParts[i].transform.localPosition = new Vector3(lastPosition.x, lastPosition.y, (lastPosition.z/lastSliderValue)*newValue);
        }
        
        lastSliderValue = newValue;
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

           detectorParts[i].GetComponent<MeshRenderer>().material = newMaterial;

        }
        

    }

    public void ClearEvent()
    {
        animating = false;
        DestroyHits();
    }

    void DestroyHits()
    {
        hits = GameObject.FindGameObjectsWithTag("Hit");
        for (var i = 0; i < hits.Length; i++)
        {
            Destroy(hits[i]);
        }
        
        hits = null;
    }

    void LoadHits()
    {
        var filename = "Collision1.txt";
        var source = new StreamReader(Application.dataPath + "/Collision Data/" + filename);
        var fileContents = source.ReadToEnd();
        source.Close();
        var lines = fileContents.Split("\n"[0]);
        int size = lines.Length;
        float largestZ = 0;
        hits = new GameObject[size];
        float x = 0f;
        float y = 0f;
        float z = 0f;
        float minE = 1000f;
        float maxE = 0f;
        float[] energyList = new float[size];
        timeList = new float[size];
        for (int i = 0; i < lines.Length; i++)
        {
            var coords = lines[i].Split(" "[0]);
            for (int j = 0; j < coords.Length; j++)
            {
                if (j == 0)
                { 
                    timeList[i] = float.Parse(coords[j]) / 8.0f;
                }
                if (j == 1)
                {
                    x = float.Parse(coords[j]);
                }
                if (j == 2)
                {
                    y = float.Parse(coords[j]);
                }
                if (j == 3)
                {
                    z = float.Parse(coords[j]);
                    if(z > largestZ)
                    {
                        largestZ = z;
                    }
                    
                }
                if (j == 4)
                {
                    if (Math.Log(float.Parse(coords[j]),10) < minE)
                    {
                        minE = (float)Math.Log(float.Parse(coords[j]), 10);
                    }
                    if (Math.Log(float.Parse(coords[j]),10) > maxE)
                    {
                        maxE = (float)Math.Log(float.Parse(coords[j]), 10);
                    }
                    energyList[i] = (float)Math.Log(float.Parse(coords[j]),10f);       
                }
            }
            hits[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hits[i].transform.position = new Vector3(x, y, z);
            hits[i].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            hits[i].GetComponent<Collider>().enabled = false;
            hits[i].GetComponent<Renderer>().enabled = false;
            hits[i].tag = "Hit";
        }

        for(int i = 0; i < size; i++)
        {
            float redness = (energyList[i] - minE) / (maxE - minE);
            float blueness = 1f - redness;
            Color color = new Color(redness, 0f, blueness);
            color.a = redness;

            Material material = new Material(Shader.Find("Transparent/Diffuse"));
            material.color = color;

            hits[i].GetComponent<Renderer>().material = material;
            
        }

        animating = true;
        Debug.Log(largestZ);
        
    }

    public void LoadEvent()
    {
        clearing = true;
        animating = true;
        duration = false;
    }

    public void AnimateEvent()
    {
        clearing = true;
        animating = true;
        duration = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class EventLoader : MonoBehaviour
{
    private StreamReader source;
    private string fileContents;
    private string[] events;
    private float[][,] hitData;
    private GameObject[][] hitObjects;
    private int iEvt = 1;
    private int maxiEvt = -1;
    private int clearingiEvt = 0;
    public string filename = "Events.txt";
    private bool animating = false;
    private bool looping = false;
    private bool clearing = false;
    private float start_time = 0f;
    GameObject proton;
    GameObject electron;


    
    private int index = 0;


    // Start is called before the first frame update
    void Start()
    {
        LoadFile();
        proton = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        proton.transform.position = new Vector3(0, 0, 0);
        proton.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        proton.GetComponent<Collider>().enabled = false;
        proton.GetComponent<Renderer>().enabled = false;

        Color pcolor = new Color(1f, 0f, 0f);
        Material pmaterial = new Material(Shader.Find("Transparent/Diffuse"));
        pmaterial.color = pcolor;
        pmaterial.renderQueue = 10000;
        proton.GetComponent<MeshRenderer>().sharedMaterial = pmaterial;

        electron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        electron.transform.position = new Vector3(0, 0, 0);
        electron.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        electron.GetComponent<Collider>().enabled = false;
        electron.GetComponent<Renderer>().enabled = false;

        Color ecolor = new Color(0f, 0f, 1f);
        Material ematerial = new Material(Shader.Find("Transparent/Diffuse"));
        ematerial.color = ecolor;
        ematerial.renderQueue = 10000;
        electron.GetComponent<MeshRenderer>().sharedMaterial = ematerial;
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            for (int i = 0; i < hitObjects[iEvt].Length; i++)
            {
                if (hitData[iEvt][i, 0]*1.3333f + 2.666f <= Time.time - start_time)
                {
                    hitObjects[iEvt][i].GetComponent<Renderer>().enabled = true;
                }
                
            }
            if (Time.time - start_time < 2.666f)
            {
                proton.GetComponent<Renderer>().enabled = true;
                electron.GetComponent<Renderer>().enabled = true;
                proton.transform.position = new Vector3(0, 0, -6 + 2.25f * (Time.time - start_time));
                electron.transform.position = new Vector3(0, 0, 6 - 2.25f * (Time.time - start_time));
            }
            else
            {
                proton.GetComponent<Renderer>().enabled = false;
                electron.GetComponent<Renderer>().enabled = false;
            }
            if (looping == true)
            {
                if (Time.time - start_time >= 9)
                {
                    start_time = Time.time;
                    if (!clearing)
                    {
                        clearing = true;
                        clearingiEvt = iEvt;         
                    }
                    
                    iEvt++;
                    if (iEvt == maxiEvt)
                    {
                        iEvt = 1;
                    }
                    
                }
            }
        }
        if (clearing)
        {
            proton.GetComponent<Renderer>().enabled = false;
            electron.GetComponent<Renderer>().enabled = false;
            ClearHits(clearingiEvt);
        }
    }

    public void NextEvent()
    {
        clearing = true;
        clearingiEvt = iEvt;
        if (iEvt == maxiEvt)
        {
            iEvt = 1;
        }
        
        if (animating == false)
        {
            LoadHits();
        }
        else
        {
            start_time = Time.time;
        }

    }
    public void PreviousEvent()
    {
        looping = false;
        clearing = true;
        clearingiEvt = iEvt;
        iEvt--;
        if (iEvt == 0)
        {
            iEvt = maxiEvt - 1;
        }
        
        if (animating == false)
        {
            LoadHits();
        }
        else
        {
            start_time = Time.time;
        }

    }

    public void LoopAnimation()
    {
        if (looping == true)
        {
            looping = false;
        }
        else
        {
            looping = true;
            AnimateHits();
        }
    }

    public void AnimateHits()
    {
            ClearHits(iEvt);
            animating = true;
            start_time = Time.time;
    }

    public void LoadHits()
    {
        animating = false;
        for (int i = 0; i < hitObjects[iEvt].Length; i++)
        {              
            hitObjects[iEvt][i].GetComponent<Renderer>().enabled = true;
        }
        
    }

    public void StartClearHits()
    {
        proton.GetComponent<Renderer>().enabled = false;
        electron.GetComponent<Renderer>().enabled = false;
        clearing = true;
        index = 0;
        animating = false;
        looping = false;
        while (clearing == true)
        {
            ClearHits(iEvt);
        }
    }

    private void ClearHits(int ievt)
    {
        

        clearing = true;
        
            

        if (index >= hitObjects[ievt].Length)
        {
            
            index = 0;
            start_time = Time.time;
            clearing = false;
        }
        else
        {
            for (int i = index; i < hitObjects[ievt].Length && i < index + 1000; i++)
            {

                hitObjects[ievt][i].GetComponent<Renderer>().enabled = false;
                

            }
            index = index + 1000;
        }
        
    }

    public void LoadFile()
    {
        source = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));
        fileContents = source.ReadToEnd();
        source.Close();
        events = fileContents.Split(new string[] { "Event" }, StringSplitOptions.None);
        maxiEvt = events.Length - 1;

        hitData = new float[maxiEvt][,];
        hitObjects = new GameObject[maxiEvt][];

        for (int ievt = 0; ievt < maxiEvt; ievt++)
        {
            var prelines = events[ievt].Split("\n"[0]);
            int size = 0;
            for (var i = 1; i < prelines.Length; i++)
            {
                if (!string.Equals("", prelines[i]) && !string.Equals("\n", prelines[i]))
                {
                    size++;
                }
            }
            String[] lines = new string[size];
            int index = 0;
            for (var i = 1; i < prelines.Length; i++)
            {
                if (!string.Equals("", prelines[i]) && !string.Equals("\n", prelines[i]))
                {
                    lines[index] = prelines[i];
                    index++;
                }
            }
            for (int i = 1; i < size; i++)
            {
                var coords = lines[i].Split(" "[0]);
                if (string.Equals(coords[0], "Clusters"))
                {
                    size = i - 1;
                }
            }

            float[,] eventHits = new float[size, 5];
            for (int i = 1; i < size; i++)
            {
                var coords = lines[i].Split(" "[0]);
                for (int j = 0; j < coords.Length; j++)
                {
                    if (j == 0)
                    {
                        eventHits[i, 0] = (float.Parse(coords[j]) / 10.0f) * 1.333f;
                    }
                    if (j == 1)
                    {
                        eventHits[i, 1] = float.Parse(coords[j]);
                    }
                    if (j == 2)
                    {
                        eventHits[i, 2] = float.Parse(coords[j]);
                    }
                    if (j == 3)
                    {
                        eventHits[i, 3] = float.Parse(coords[j]);
                    }
                    if (j == 4)
                    {
                        eventHits[i, 4] = float.Parse(coords[j]);
                    }
                }
            }
            hitData[ievt] = eventHits;
            GameObject[] eventObjects = new GameObject[size];
            Color color;
            for (int i = 0; i < size; i++)
            {
                eventObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                eventObjects[i].transform.position = new Vector3(eventHits[i, 1], eventHits[i, 2], eventHits[i, 3]);
                eventObjects[i].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                eventObjects[i].GetComponent<Collider>().enabled = false;
                eventObjects[i].GetComponent<Renderer>().enabled = false;
                

                float redness = eventHits[i, 4] / 200f;
                if(redness > 1)
                {
                    color = new Color(1f, 0f, 0f);
                    color.a = 1;
                }
                else if(redness == 0)
                {
                    color = new Color(1f, 1f, 1f);
                    color.a = 1;
                }
                else if(redness < 0)
                {
                    color = new Color(0f, 0f, 0f);
                    color.a = 1;
                }
                else
                {
                    color = new Color(redness, 0f, 1f - redness);
                    color.a = redness;
                }
                Material material = new Material(Shader.Find("Transparent/Diffuse"));
                material.color = color;
                material.renderQueue = -1;
                eventObjects[i].GetComponent<MeshRenderer>().sharedMaterial = material;
                

            }
            hitObjects[ievt] = eventObjects;

        }



    }
}

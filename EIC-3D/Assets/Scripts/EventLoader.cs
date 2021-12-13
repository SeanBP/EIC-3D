using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class EventLoader : MonoBehaviour
{
    private StreamReader source;
    private string fileContents;
    private string[] events;
    private float[][] hitTime;
    private GameObject[][] hitObjects;
    private GameObject[][] clusterObjects;
    
    private int iEvt = 0;
    private int clearingiEvt = 0;
    private int clearingiEvtC = 0;
    public string filename = "Events.txt";
    private bool animating = false;
    private bool looping = false;
    private bool clearing = false;
    private bool clearingC = false;
    private bool clusterToggle = false;
    private int index = 0;
    private int indexC = 0;
    private float start_time = 0f;
    GameObject proton;
    GameObject electron;
    private int maxiEvt;
    public float rate = 3; //speed of light is [rate] m/s
    public InputField rateField;


    // Start is called before the first frame update
    void Start()
    {
        rateField.text = "Speed";
        LoadHitFile();
        LoadClusterFile();
        proton = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        proton.transform.position = new Vector3(0, 0, 0);
        proton.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        proton.GetComponent<Collider>().enabled = false;
        proton.GetComponent<Renderer>().enabled = false;

        Color pcolor = new Color(1f, 0f, 0f);
        Material pmaterial = new Material(Shader.Find("Transparent/Diffuse"));
        pmaterial.color = pcolor;
        pmaterial.renderQueue = -1;
        proton.GetComponent<MeshRenderer>().sharedMaterial = pmaterial;

        electron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        electron.transform.position = new Vector3(0, 0, 0);
        electron.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        electron.GetComponent<Collider>().enabled = false;
        electron.GetComponent<Renderer>().enabled = false;

      

        Color ecolor = new Color(0f, 0f, 1f);
        Material ematerial = new Material(Shader.Find("Transparent/Diffuse"));
        ematerial.color = ecolor;
        ematerial.renderQueue = -1;
        electron.GetComponent<MeshRenderer>().sharedMaterial = ematerial;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            rate = float.Parse(rateField.text);
        }
        catch
        {
            rate = 3f;
        }
        if(rate < 0)
        {
            rate = 3f;
        }
        if (animating)
        {
            for (int i = 0; i < hitObjects[iEvt].Length; i++)
            {
                if ((hitTime[iEvt][i] / 3.33564f)/rate + (6f / rate) <= Time.time - start_time)
                {
                    hitObjects[iEvt][i].GetComponent<Renderer>().enabled = true;
                }
                
            }
            if (Time.time - start_time < 6f / rate)
            {
                proton.GetComponent<Renderer>().enabled = true;
                electron.GetComponent<Renderer>().enabled = true;
  
                proton.transform.position = new Vector3(0, 0, -6 + rate * (Time.time - start_time));
                electron.transform.position = new Vector3(0, 0, 6 - rate * (Time.time - start_time));
               
            }
            else
            {
                proton.GetComponent<Renderer>().enabled = false;
                electron.GetComponent<Renderer>().enabled = false;
              
                if (clusterToggle)
                {
                    LoadClusters();
                }
            }
            if (looping == true)
            {
                if (Time.time - start_time >= (15f / rate) + 1)
                {
                    start_time = Time.time;
                    if (!clearing)
                    {
                        clearing = true;
                        clearingiEvt = iEvt;         
                    }
                    if (!clearingC)
                    {
                        clearingC = true;
                        clearingiEvtC = iEvt;
                    }

                    iEvt++;
                    if (iEvt == hitObjects.Length)
                    {
                        iEvt = 0;
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
        if (clearingC)
        {         
            ClearClusters(clearingiEvtC);
        }
    }

    public void NextEvent()
    {
        if (!clearing && !clearingC)
        {
            looping = false;
            clearingiEvt = iEvt;
            clearing = true;
            clearingC = true;
            ClearClusters(iEvt);
            iEvt++;
            if (iEvt == hitObjects.Length)
            {
                iEvt = 0;
            }
            if (clusterToggle && !animating)
            {
                LoadClusters();
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

    }
    public void PreviousEvent()
    {
        if (!clearing && !clearingC)
        {
            looping = false;
            clearing = true;
            clearingC = true;
            ClearClusters(iEvt);
            clearingiEvt = iEvt;
            clearingiEvtC = iEvt;
            iEvt--;
            if (iEvt == -1)
            {
                iEvt = hitObjects.Length - 1;
            }
            if (clusterToggle && !animating)
            {
                LoadClusters();
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

    }

    public void LoopAnimation()
    {
        clearing = true;
        clearingC = true;
        ClearClusters(iEvt);
        ClearHits(iEvt);
        clearingiEvt = iEvt;
        clearingiEvtC = iEvt;
        if (looping == true)
        {
            looping = false;
            animating = false;
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
       ClearClusters(iEvt);
       clearingiEvt = iEvt;
       clearingiEvtC = iEvt;
       animating = true;
       start_time = Time.time;
        
    }

    public void LoadHits()
    {
        animating = false;
        looping = false;
        for (int i = 0; i < hitObjects[iEvt].Length; i++)
        {              
            hitObjects[iEvt][i].GetComponent<Renderer>().enabled = true;
        }
        
    }

    public void StartClearHits()
    {
        if (!clearing)
        {
            proton.GetComponent<Renderer>().enabled = false;
            electron.GetComponent<Renderer>().enabled = false;
            clearing = true;
            index = 0;
            animating = false;
            looping = false;
            clearingiEvt = iEvt;
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
    public void LoadClusters()
    {
        
        for (int i = 0; i < clusterObjects[iEvt].Length; i++)
        {
            clusterObjects[iEvt][i].GetComponent<Renderer>().enabled = true;
        }
        clusterToggle = true;

    }
    public void StartClearClusters()
    {
        if (!clearingC)
        {
            clusterToggle = false;
            clearingC = true;
            indexC = 0;

            clearingiEvtC = iEvt;
            ClearClusters(iEvt);
        }
    }

    private void ClearClusters(int ievt)
    {
        
        clearingC = true;

        if (indexC >= clusterObjects[ievt].Length)
        {

            indexC = 0;
            
            clearingC = false;
        }
        else
        {
            for (int i = indexC; i < clusterObjects[ievt].Length && i < indexC + 1000; i++)
            {

                clusterObjects[ievt][i].GetComponent<Renderer>().enabled = false;


            }
            indexC = indexC + 1000;
        }
        
    }
   
    public void LoadClusterFile()
    {
        source = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));
        fileContents = source.ReadToEnd();
        source.Close();
        events = fileContents.Split("Event");
        string[] lines;
        clusterObjects = new GameObject[events.Length - 1][];
 
        float length;
        int clusterSize;
        float granularity = 0f;
        float x, y, z;
        int start = -1;
        for (int i = 1; i < events.Length; i++)
        {
            start = -1;
            lines = events[i].Split("\n");
            clusterSize = 0;
            for (int j = 2; j < lines.Length; j++)
            {          
                if (lines[j].Contains("Ecal") ^ lines[j].Contains("Hcal"))
                {
                    clusterSize++;
                    if(start == -1)
                    {
                        start = j;
                    }
                }            
            }
            GameObject [] clusters = new GameObject[clusterSize];
            for(int j = 0; j < clusterSize; j++)
            {
                var coords = lines[j+start].Split(" "[0]);
                clusters[j] = GameObject.CreatePrimitive(PrimitiveType.Cube);

                clusters[j].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                clusters[j].GetComponent<Collider>().enabled = false;
                clusters[j].GetComponent<Renderer>().enabled = false;

                clusters[j].layer = 3;
                
 
                length = float.Parse(coords[5]) / 30f;

                x = float.Parse(coords[2]);

                y = float.Parse(coords[3]);

                z = float.Parse(coords[4]);


                if (string.Equals(coords[0], "Ecal"))
                {
                    granularity = 0.025f;
                    Material material = new Material(Shader.Find("Standard"));
                    material.color = Color.green;
                    material.renderQueue = 999;
                    clusters[j].GetComponent<MeshRenderer>().sharedMaterial = material;

                }
                if (string.Equals(coords[0], "Hcal"))
                {
                    granularity = 0.1f;
                    Material material = new Material(Shader.Find("Standard"));
                    material.color = Color.blue;
                    material.renderQueue = 1000;
                    clusters[j].GetComponent<MeshRenderer>().sharedMaterial = material;

                }


                if (string.Equals(coords[1], "Endcap"))
                {
                    clusters[j].transform.localScale = new Vector3(granularity, granularity, 1f * length + 0.01f);
                    if (z < 0)
                    {
                        clusters[j].transform.position = new Vector3(x, y, z - (length + 0.01f) / 2);
                    }
                    else
                    {
                        clusters[j].transform.position = new Vector3(x, y, z + (length + 0.01f) / 2);
                    }
                }
                else
                {
                    int sides = 12;
                    GameObject pivot = new GameObject();
                    clusters[j].transform.localScale = new Vector3((length + 0.01f), granularity, granularity);
                    pivot.transform.localPosition = new Vector3(x, y, z);
                    clusters[j].transform.localPosition = new Vector3(x, y, z);
                    float angle = (float)Math.Atan(y / x);
                    float xmag = 1;
                    if (x != 0)
                    {
                        xmag = (x / (float)Math.Abs(x));
                    }
                    float angle2 = (float)(180 / Math.PI) * (float)Math.Atan(y / x);
                    if (!(x == 0 && y == 0))
                    {

                        int side = Convert.ToInt32(Math.Round(angle2 / (360f / (float)sides)));

                        float desiredAngle = side * (360f / 12f);

                        clusters[j].transform.RotateAround(pivot.transform.position, new Vector3(0, 0, 1), desiredAngle);

                        float magnitude = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                        float newX = xmag * (magnitude + ((length + 0.01f) / 2f)) * (float)Math.Cos(Math.PI * desiredAngle / 180.0f);
                        float newY = (magnitude + ((length + 0.01f) / 2f)) * (float)Math.Sin(Math.PI * desiredAngle / 180.0f);
                        clusters[j].transform.position = new Vector3(newX, xmag * newY, z);
                    }
                    else
                    {
                        clusters[j].SetActive(false);
                    }
                    Destroy(pivot);
                }



            }
            clusterObjects[i-1] = clusters;

        }


    }

    public void LoadHitFile()
    {
        source = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));
        fileContents = source.ReadToEnd();
        source.Close();
        events = fileContents.Split("Event");
        string[] lines;
        hitObjects = new GameObject[events.Length - 1][];
        hitTime = new float[events.Length - 1][];
        bool checkCluster;
        Material material;
        Color color;
        for (int i = 1; i < events.Length; i++)
        {
            
            lines = events[i].Split("\n");
            int hitSize = 0;
            checkCluster = false;
            for (int j = 2; j < lines.Length; j++)
            {                  
                if (lines[j].Contains("Cluster"))
                {
                    checkCluster = true;
                }
                if (!checkCluster)
                {
                    hitSize++;
                }
            }
            
            float[] timeData = new float[hitSize];
            string[] coords;
            GameObject[] eventObjects = new GameObject[hitSize];

            for (int j = 0; j < hitSize; j++)
            {
                coords = lines[j + 2].Split(" ");
                timeData[j] = float.Parse(coords[0]);
                eventObjects[j] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                eventObjects[j].transform.position = new Vector3(float.Parse(coords[1]), float.Parse(coords[2]), float.Parse(coords[3]));
                eventObjects[j].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                eventObjects[j].GetComponent<Collider>().enabled = false;
                eventObjects[j].GetComponent<Renderer>().enabled = false;

                float redness = float.Parse(coords[4]) / 200f;
                if (redness > 1)
                {
                    color = new Color(1f, 0f, 0f);
                    color.a = 1;
                }
                else if (redness == 0)
                {
                    color = new Color(1f, 1f, 1f);
                    color.a = 1;
                }
                else if (redness < 0)
                {
                    color = new Color(0f, 0f, 0f);
                    color.a = 1;
                }
                else
                {
                    color = new Color(redness, 0f, 1f - redness);
                    color.a = redness;
                }
                material = new Material(Shader.Find("Transparent/Diffuse"));
                material.color = color;
                material.renderQueue = -1;
                eventObjects[j].GetComponent<MeshRenderer>().sharedMaterial = material;
            }
            hitTime[i - 1] = timeData;
            hitObjects[i - 1] = eventObjects;

        }

    }

}

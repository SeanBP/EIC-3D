using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class EventMaker : MonoBehaviour
{

    //private bool EventLoaded = false;
    public GameObject text;
    private GameObject[] hits = null;
    private GameObject[] clusters = null;
    private bool animating = false;
    private float[] timeList;
    private float start_time = 0f;
    private bool clearing = false;
    private bool duration = false;
    private bool looping = false;
    private StreamReader source;
    private string fileContents;
    private string[] events;
    public string filename = "Events.txt";
    public int iEvt = 1;
    private int maxiEvt = 1;
    GameObject proton;
    GameObject electron;


    // Update is called once per frame
    void Start()
    {
        source = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));
        fileContents = source.ReadToEnd();
        source.Close();
        events = fileContents.Split(new string[] { "Event" }, StringSplitOptions.None);
        maxiEvt = events.Length - 1;

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

    void Update()
    {
        if(animating && duration == true && Time.time - start_time < 2.6666f)
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


    }



    void LateUpdate()
    {
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
            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (duration == true)
                    {
                        if (timeList[i] + 3f <= Time.time - start_time)
                        {
                            hits[i].GetComponent<Renderer>().enabled = true;
                        }
                        else
                        {
                            hitsLeft = true;
                        }

                        if (looping == true)
                        {
                            if (Time.time - start_time >= 9)
                            {
                                NextEvent();
                            }
                        }

                    }
                    else
                    {
                        hits[i].GetComponent<Renderer>().enabled = true;
                    }
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

    public void LoopAnimation()
    {
        if(looping == true)
        {
            looping = false;
        }
        else
        {
            looping = true;
            AnimateHits();
        }
        

    }

    public void NextEvent()
    {
        
        iEvt++;
        if(iEvt == maxiEvt+1)
        {
            iEvt = 1;
        }
        
        if(clusters != null && clusters.Length != 0)
        {
            ClearClusters();
            LoadClusters();
        }
        
        if (hits != null && hits.Length != 0)
        {
            ClearHits();
            LoadHits();
            
        }    

        start_time = Time.time;
        

    }
    public void PreviousEvent()
    {
        iEvt--;
        if (iEvt == 0)
        {
            iEvt = maxiEvt;
        }
        
        if (clusters != null && clusters.Length != 0)
        {
            ClearClusters();
            LoadClusters();
        }
        
        if (hits != null && hits.Length != 0)
        {
            
            LoadHits();
        }
        start_time = Time.time;
    }

    public void ClearHits()
    {
        animating = false;
        DestroyHits();
        proton.GetComponent<Renderer>().enabled = false;
        electron.GetComponent<Renderer>().enabled = false;
    }
    public void ClearClusters()
    {
        if (clusters != null)
        {
            for (var i = 0; i < clusters.Length; i++)
            {
                Destroy(clusters[i]);
            }
        }
        clusters = null;
    }
    void DestroyHits()
    { 
        if (hits != null)
        {
            for (var i = 0; i < hits.Length; i++)
            {
                Destroy(hits[i]);
            }
        }
        hits = null;
    }
    public void LoadClusters()
    {
        ClearClusters();

        try
        {
            
            var source = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));
            
            GameObject clusterSig = GameObject.CreatePrimitive(PrimitiveType.Cube);
            clusterSig.tag = "Cluster";
            clusterSig.GetComponent<Collider>().enabled = false;
            clusterSig.GetComponent<Renderer>().enabled = false;

            var fileContents = source.ReadToEnd();
            source.Close();

            var events = fileContents.Split(new string[] { "Event" }, StringSplitOptions.None);
            var prelines = events[iEvt].Split("\n"[0]);
            var size = 0;

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


            var start = 0;
            for (var i = 0; i < size; i++)
            {
                var coords = lines[i].Split(" "[0]);
                if (string.Equals(coords[0].TrimEnd('\r', '\n'), "Clusters"))
                {
                    start = i + 1;
                }
            }

            size = size - start;


            clusters = new GameObject[size];
            float x = 0f;
            float y = 0f;
            float z = 0f;
            float minE = 1000f;
            float maxE = 0f;
            float[] energyList = new float[size];

            for (var i = start; i < lines.Length; i++)
            {
                var coords = lines[i].Split(" "[0]);

                if (float.Parse(coords[5]) < minE)
                {
                    minE = float.Parse(coords[5]);
                }
                if (float.Parse(coords[5]) > maxE)
                {
                    maxE = float.Parse(coords[5]);
                }
                energyList[i - start] = float.Parse(coords[5]);

            }

            for (var i = start; i < lines.Length; i++)
            {
                var coords = lines[i].Split(" "[0]);
                Color color = new Color(0, 0, 0);

                x = float.Parse(coords[2]);

                y = float.Parse(coords[3]);

                z = float.Parse(coords[4]);

                clusters[i - start] = GameObject.CreatePrimitive(PrimitiveType.Cube);

                clusters[i - start].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                clusters[i - start].GetComponent<Collider>().enabled = false;
                clusters[i - start].GetComponent<Renderer>().enabled = true;
                clusters[i - start].tag = "Cluster";
                clusters[i - start].layer = 3;
                float length = 0f;
                
                length = energyList[i - start] / 30f;
                
                
                float granularity = 0f;
                
                if (string.Equals(coords[0], "Ecal"))
                {
                    granularity = 0.025f;
                    Material material = new Material(Shader.Find("Standard"));
                    material.color = Color.green;
                    material.renderQueue = 999;
                    clusters[i - start].GetComponent<MeshRenderer>().sharedMaterial = material;

                }
                if (string.Equals(coords[0], "Hcal"))
                {
                    granularity = 0.1f;
                    Material material = new Material(Shader.Find("Standard"));
                    material.color = Color.blue;
                    material.renderQueue = 1000;
                    clusters[i - start].GetComponent<MeshRenderer>().sharedMaterial = material;
                    
                }


                if (string.Equals(coords[1], "Endcap"))
                {
                    clusters[i - start].transform.localScale = new Vector3(granularity, granularity, 1f * length + 0.01f);
                    if (z < 0)
                    {
                        clusters[i - start].transform.position = new Vector3(x, y, z - (length + 0.01f) / 2);
                    }
                    else
                    {
                        clusters[i - start].transform.position = new Vector3(x, y, z + (length + 0.01f) / 2);
                    }
                }
                else
                {
                    int sides = 12;
                    GameObject pivot = new GameObject();
                    clusters[i - start].transform.localScale = new Vector3((length + 0.01f), granularity, granularity);
                    pivot.transform.localPosition = new Vector3(x, y, z);
                    clusters[i - start].transform.localPosition = new Vector3(x, y, z);
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

                        clusters[i - start].transform.RotateAround(pivot.transform.position, new Vector3(0, 0, 1), desiredAngle);

                        float magnitude = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                        float newX = xmag * (magnitude + ((length + 0.01f) / 2f)) * (float)Math.Cos(Math.PI * desiredAngle / 180.0f);
                        float newY = (magnitude + ((length + 0.01f) / 2f)) * (float)Math.Sin(Math.PI * desiredAngle / 180.0f);
                        clusters[i - start].transform.position = new Vector3(newX, xmag * newY, z);
                    }
                    else
                    {
                        clusters[i - start].SetActive(false);
                    }
                    Destroy(pivot);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            text.GetComponent<UnityEngine.UI.Text>().text = e.Message;
            GameObject cube1 = new GameObject();
            cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Material material2 = new Material(Shader.Find("Transparent/Diffuse"));
            material2.color = Color.red;
            cube1.GetComponent<MeshRenderer>().sharedMaterial = material2;
        }
        
        }

    public void LoadHits()
    {
       
        ClearHits();
        proton.GetComponent<Renderer>().enabled = false;
        electron.GetComponent<Renderer>().enabled = false;

        GameObject hitSig = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hitSig.tag = "Hit";
        hitSig.GetComponent<Collider>().enabled = false;
        hitSig.GetComponent<Renderer>().enabled = false;


        
        var prelines = events[iEvt].Split("\n"[0]);
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
        
        hits = new GameObject[size - 1];
        float x = 0f;
        float y = 0f;
        float z = 0f;
        
        float maxE = 0f;
        float[] energyList = new float[size];
        timeList = new float[size];
        for (int i = 1; i < size; i++)
        {
            var coords = lines[i].Split(" "[0]);
            for (int j = 0; j < coords.Length; j++)
            {
                if (j == 0)
                {
                    timeList[i - 1] = (float.Parse(coords[j]) / 10.0f) * 1.333f;
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
                    

                }
                if (j == 4)
                {
                    
                    if (Math.Log(float.Parse(coords[j]), 10) > maxE)
                    {
                        maxE = float.Parse(coords[j]);
                    }
                    energyList[i - 1] = float.Parse(coords[j]);
                }
            }
            hits[i - 1] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hits[i - 1].transform.position = new Vector3(x, y, z);
            hits[i - 1].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            hits[i - 1].GetComponent<Collider>().enabled = false;
            hits[i - 1].GetComponent<Renderer>().enabled = false;
            hits[i - 1].tag = "Hit";
            
        }

        for (int i = 0; i < hits.Length; i++)
        {

            float redness = 0f;
            if (maxE != 0)
            {
                redness = energyList[i] / 200;
            }
            if(redness > 1)
            {
                redness = 1;
            }
            
            float blueness = 1f - redness;
            Color color = new Color(redness, 0f, blueness);
            color.a = redness;
            if(energyList[i] == 0)
            {
                color.a = 1;
                color = new Color(1f, 1f, 1f);
            }
            if (energyList[i] < 0)
            {
                color.a = 1;
                color = new Color(0f, 0f, 0f);
            }


            Material material = new Material(Shader.Find("Transparent/Diffuse"));
            material.color = color;
            material.renderQueue = -1;
            hits[i].GetComponent<MeshRenderer>().sharedMaterial = material;
            
            
        }
        animating = true;
        
    }

    public void LoadEvent()
    {
        clearing = true;
        animating = true;
        duration = false;
    }

    public void AnimateHits()
    {
        clearing = true;
        animating = true;
        duration = true;
    }
}
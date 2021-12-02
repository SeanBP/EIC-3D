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
    private GameObject[][] clusters;
    private int iEvt = 1;
    private int maxiEvt = -1;
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


    
    

    // Start is called before the first frame update
    void Start()
    {
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
                if (clusterToggle)
                {
                    LoadClusters();
                }
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
                    if (!clearingC)
                    {
                        clearingC = true;
                        clearingiEvtC = iEvt;
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
            if (iEvt == maxiEvt)
            {
                iEvt = 1;
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
            if (iEvt == 0)
            {
                iEvt = maxiEvt - 1;
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
        
        for (int i = 0; i < clusters[iEvt].Length; i++)
        {
            clusters[iEvt][i].GetComponent<Renderer>().enabled = true;
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

        if (indexC >= clusters[ievt].Length)
        {

            indexC = 0;
            
            clearingC = false;
        }
        else
        {
            for (int i = indexC; i < clusters[ievt].Length && i < indexC + 1000; i++)
            {

                clusters[ievt][i].GetComponent<Renderer>().enabled = false;


            }
            indexC = indexC + 1000;
        }

    }

    public void LoadClusterFile()
    {
        var source = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));       

        var fileContents = source.ReadToEnd();
        source.Close();
        
        var events = fileContents.Split(new string[] { "Event" }, StringSplitOptions.None);
        maxiEvt = events.Length - 1;
        clusters = new GameObject[maxiEvt][];

        for (int ievt = 0; ievt < maxiEvt; ievt++) {
            var prelines = events[ievt].Split("\n"[0]);
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


            clusters[ievt] = new GameObject[size];
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

                clusters[ievt][i - start] = GameObject.CreatePrimitive(PrimitiveType.Cube);

                clusters[ievt][i - start].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                clusters[ievt][i - start].GetComponent<Collider>().enabled = false;
                clusters[ievt][i - start].GetComponent<Renderer>().enabled = false;
 
                clusters[ievt][i - start].layer = 3;
                float length = 0f;

                length = energyList[i - start] / 30f;


                float granularity = 0f;

                if (string.Equals(coords[0], "Ecal"))
                {
                    granularity = 0.025f;
                    Material material = new Material(Shader.Find("Standard"));
                    material.color = Color.green;
                    material.renderQueue = 999;
                    clusters[ievt][i - start].GetComponent<MeshRenderer>().sharedMaterial = material;

                }
                if (string.Equals(coords[0], "Hcal"))
                {
                    granularity = 0.1f;
                    Material material = new Material(Shader.Find("Standard"));
                    material.color = Color.blue;
                    material.renderQueue = 1000;
                    clusters[ievt][i - start].GetComponent<MeshRenderer>().sharedMaterial = material;

                }


                if (string.Equals(coords[1], "Endcap"))
                {
                    clusters[ievt][i - start].transform.localScale = new Vector3(granularity, granularity, 1f * length + 0.01f);
                    if (z < 0)
                    {
                        clusters[ievt][i - start].transform.position = new Vector3(x, y, z - (length + 0.01f) / 2);
                    }
                    else
                    {
                        clusters[ievt][i - start].transform.position = new Vector3(x, y, z + (length + 0.01f) / 2);
                    }
                }
                else
                {
                    int sides = 12;
                    GameObject pivot = new GameObject();
                    clusters[ievt][i - start].transform.localScale = new Vector3((length + 0.01f), granularity, granularity);
                    pivot.transform.localPosition = new Vector3(x, y, z);
                    clusters[ievt][i - start].transform.localPosition = new Vector3(x, y, z);
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

                        clusters[ievt][i - start].transform.RotateAround(pivot.transform.position, new Vector3(0, 0, 1), desiredAngle);

                        float magnitude = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                        float newX = xmag * (magnitude + ((length + 0.01f) / 2f)) * (float)Math.Cos(Math.PI * desiredAngle / 180.0f);
                        float newY = (magnitude + ((length + 0.01f) / 2f)) * (float)Math.Sin(Math.PI * desiredAngle / 180.0f);
                        clusters[ievt][i - start].transform.position = new Vector3(newX, xmag * newY, z);
                    }
                    else
                    {
                        clusters[ievt][i - start].SetActive(false);
                    }
                    Destroy(pivot);
                }
            }

        }


    }

    public void LoadHitFile()
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

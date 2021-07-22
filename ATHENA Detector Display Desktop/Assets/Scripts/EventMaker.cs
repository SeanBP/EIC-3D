using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class EventMaker : MonoBehaviour
{

    //private bool EventLoaded = false;
    private GameObject[] hits = null;
    private GameObject[] clusters = null;
    private bool animating = false;
    private float[] timeList;
    private float start_time = 0f;
    private bool clearing = false;
    private bool duration = false;
    public string filename = "Test.txt";
    public int iEvt = 2;

    // Update is called once per frame


    void Update()
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

    public void NextEvent()
    {
        iEvt++;
        if(iEvt == 101)
        {
            iEvt = 2;
        }
        clusters = GameObject.FindGameObjectsWithTag("Cluster");
        if(clusters.Length != 0)
        {
            ClearClusters();
            LoadClusters();
        }
        hits = GameObject.FindGameObjectsWithTag("Hit");
        if (hits.Length != 0)
        {
            ClearHits();
            LoadHits();
        }

        
    }
    public void PreviousEvent()
    {
        iEvt--;
        if (iEvt == 1)
        {
            iEvt = 100;
        }
        clusters = GameObject.FindGameObjectsWithTag("Cluster");
        if (clusters.Length != 0)
        {
            ClearClusters();
            LoadClusters();
        }
        hits = GameObject.FindGameObjectsWithTag("Hit");
        if (hits.Length != 0)
        {
            ClearHits();
            LoadHits();
        }
    }

    public void ClearHits()
    {
        animating = false;
        DestroyHits();
    }
    public void ClearClusters()
    {
        clusters = GameObject.FindGameObjectsWithTag("Cluster");
        for (var i = 0; i < clusters.Length; i++)
        {
            Destroy(clusters[i]);
        }
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
    public void LoadClusters()
    {
        ClearClusters();
        var source = new StreamReader(Application.dataPath + "/Collision Data/" + filename);
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
            if (string.Equals(coords[0].TrimEnd('\r','\n'), "Clusters"))
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
            float length = (energyList[i - start] - minE) / (maxE - minE);

            var cubeRenderer = clusters[i - start].GetComponent<Renderer>();
            if (string.Equals(coords[0], "Ecal"))
            {
                cubeRenderer.material.SetColor("_Color", Color.green);

            }
            if (string.Equals(coords[0], "Hcal"))
            {
                cubeRenderer.material.SetColor("_Color", Color.blue);
            }


            if (string.Equals(coords[1], "Endcap"))
            {
                clusters[i - start].transform.localScale = new Vector3(0.05f, 0.05f, 1f * length + 0.01f);
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
                clusters[i - start].transform.localScale = new Vector3((length + 0.01f), 0.05f, 0.05f);
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

                    int side = Convert.ToInt32(Math.Round(angle2 / (360f / 12f)));

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

    void LoadHits()
    {
        ClearHits();
        var source = new StreamReader(Application.dataPath + "/Collision Data/" + filename);
        var fileContents = source.ReadToEnd();
        source.Close();
        var events = fileContents.Split(new string[] { "Event" }, StringSplitOptions.None);
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
        float minE = 1000f;
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
                    timeList[i - 1] = float.Parse(coords[j]) / 8.0f;
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
                    if (Math.Log(float.Parse(coords[j]), 10) < minE)
                    {
                        minE = (float)Math.Log(float.Parse(coords[j]), 10);
                    }
                    if (Math.Log(float.Parse(coords[j]), 10) > maxE)
                    {
                        maxE = (float)Math.Log(float.Parse(coords[j]), 10);
                    }
                    energyList[i - 1] = (float)Math.Log(float.Parse(coords[j]), 10f);
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
            float redness = (energyList[i] - minE) / (maxE - minE);
            float blueness = 1f - redness;
            Color color = new Color(redness, 0f, blueness);
            color.a = redness;

            Material material = new Material(Shader.Find("Transparent/Diffuse"));
            material.color = color;
            material.renderQueue = 1000;
            hits[i].GetComponent<Renderer>().material = material;

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ClusterMaker : MonoBehaviour
{
    private GameObject[] hits = null;
    // Start is called before the first frame update
    void Start()
    {
        var filename = "Event11.txt";
        var source = new StreamReader(Application.dataPath + "/Collision Data/" + filename);
        var fileContents = source.ReadToEnd();
        source.Close();
        var lines = fileContents.Split("\n"[0]);
        int size = lines.Length;
        int start = 0;
        for (int i = 0; i < size; i++)
        {
            var coords = lines[i].Split(" "[0]);
            if (string.Equals(coords[0], "Clusters"))
            {
                start = i+1;
            }
        }
        size = size - start;

        hits = new GameObject[size];
        float x = 0f;
        float y = 0f;
        float z = 0f;
        float minE = 1000f;
        float maxE = 0f;
        float[] energyList = new float[size];

        for (int i = start; i < lines.Length; i++)
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

        for (int i = start; i < lines.Length; i++)
        {
            var coords = lines[i].Split(" "[0]);
            Color color = new Color(0,0,0);
        
            x = float.Parse(coords[2]);
                             
            y = float.Parse(coords[3]);
                              
            z = float.Parse(coords[4]);
                                          
            hits[i-start] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            hits[i-start].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            hits[i-start].GetComponent<Collider>().enabled = false;
            hits[i-start].GetComponent<Renderer>().enabled = true;
            hits[i-start].tag = "Cluster";
            hits[i - start].layer = 3;
            float length = (energyList[i-start] - minE) / (maxE - minE);
           
            var cubeRenderer = hits[i - start].GetComponent<Renderer>();
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
                hits[i - start].transform.localScale = new Vector3(0.05f, 0.05f, 1f * length + 0.01f);
                if (z < 0)
                {
                    hits[i - start].transform.position = new Vector3(x, y, z - (length+0.01f)/2);
                }
                else
                {
                    hits[i - start].transform.position = new Vector3(x, y, z + (length + 0.01f) / 2);
                }
            }
            else
            {
                int sides = 12;
                GameObject pivot = new GameObject();
                hits[i - start].transform.localScale = new Vector3((length + 0.01f), 0.05f, 0.05f);
                pivot.transform.localPosition = new Vector3(x, y, z);
                hits[i - start].transform.localPosition = new Vector3(x, y, z);
                float angle = (float)Math.Atan(y / x);
                //hits[i - start].transform.RotateAround(pivot.transform.position, new Vector3(0, 0, 1), angle * (180 / (float)Math.PI));

                /*int side = Convert.ToInt32(Math.Floor(((float)Math.Atan(y/x) / 360f)*12));
                Debug.Log(side+" "+ Math.Atan(y / x));
                float angle = (float)side * (350f / 12f);
                hits[i - start].transform.localScale = new Vector3(1f * length + 0.01f, 0.05f, 0.05f);
                hits[i - start].transform.Rotate(0, 0, angle, Space.Self);
                hits[i - start].transform.localPosition = new Vector3(x + ((float)Math.Cos(angle)* (length + 0.01f) / 2), y + ((float)Math.Sin(angle) * (length + 0.01f) / 2), z);
                */
                //int side = Convert.ToInt32(Math.Floor(((float)Math.Atan(y / x) / 360f) * 12));
                //Debug.Log(side + " " + Math.Atan(y / x));
                
                float xmag = 1;
                if (x != 0) {
                    xmag = (x / (float)Math.Abs(x));
                }
                float angle2 = (angle * (180 / (float)Math.PI) + 360) % 360;
                int side = Convert.ToInt32(Math.Round(angle2 / (360f/12f)));
                float desiredAngle = side * (360f / 12f);
                //hits[i - start].transform.position = new Vector3(x + xmag*((float)Math.Cos(angle) * (length + 0.01f) / 2), y + ((float)Math.Sin(angle) * (length + 0.01f) / 2), z);
                //hits[i - start].transform.RotateAround(pivot.transform.position, new Vector3(0, 0, 1), 0f * (180 / (float)Math.PI));

                //hits[i - start].transform.position = new Vector3(x + ((float)Math.Cos(Math.PI * desiredAngle / 180.0) * (length + 0.01f) / 2), y + ((float)Math.Sin(Math.PI * desiredAngle / 180.0) * (length + 0.01f) / 2), z);

                //hits[i - start].transform.position = new Vector3(0, 0, 0);

                //Debug.Log(x + " " + desiredAngle+" "+Math.Cos(Math.PI * desiredAngle / 180.0) +" "+(x + ((float)Math.Cos(Math.PI*desiredAngle/180.0))));
                hits[i - start].transform.RotateAround(pivot.transform.position, new Vector3(0, 0, 1), desiredAngle);
                float magnitude = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                float newX = xmag*(magnitude + ((length + 0.01f) / 2f)) * (float)Math.Cos(Math.PI * desiredAngle / 180.0f);
                float newY = (magnitude + ((length + 0.01f) / 2f)) * (float)Math.Sin(Math.PI * desiredAngle / 180.0f);
                hits[i - start].transform.position = new Vector3(newX, newY, z);
            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

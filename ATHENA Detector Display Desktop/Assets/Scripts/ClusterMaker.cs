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
        var filename = "Event10.txt";
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
            //Material material = new Material(Shader.Find("Diffuse"));
            //Material material = hits[i - start].GetComponent<MeshRenderer>().sharedMaterial;
            var cubeRenderer = hits[i - start].GetComponent<Renderer>();
            if (string.Equals(coords[0], "Ecal"))
            {
                cubeRenderer.material.SetColor("_Color", Color.green);
                //color = new Color(0, 1, 0);
                //material.color = color;
            }
            if (string.Equals(coords[0], "Hcal"))
            {
                cubeRenderer.material.SetColor("_Color", Color.blue);
                //color = new Color(0, 1, 0);
                //material.color = color;
            }
            
            //material.renderQueue = 1001;
            //hits[i-start].GetComponent<Renderer>().material = material;
            hits[i-start].transform.localScale = new Vector3(0.05f, 0.05f, 1f * length + 0.01f);


            if (string.Equals(coords[1], "Endcap"))
            {
                if (z < 0)
                {
                    hits[i - start].transform.position = new Vector3(x, y, z - length/2);
                }
                else
                {
                    hits[i - start].transform.position = new Vector3(x, y, z + length / 2);
                }
            }
            else
            {
                hits[i - start].transform.position = new Vector3(x, y, z);
            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

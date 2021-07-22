using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ComponentMaker : MonoBehaviour
{
    private bool menagerieActive = false;
    GameObject[] menagerie;
    public Slider zSlider;
    public Slider xySlider;
    // Start is called before the first frame update
    void Start()
    {
        menagerie = GameObject.FindGameObjectsWithTag("Menagerie");
        for (int i = 0; i < menagerie.Length; i++)
        {
           menagerie[i].SetActive(false);
        }
        //blue
        //Color color = new Color(9f / 255f, 130f / 255f, 250f / 255f);

        //green
        //Color color = new Color(146f / 255f, 208f / 255f, 80f / 255f);

        //yellow
        //Color color = new Color(255f / 255f, 196f / 255f, 47f / 255f);

        //HCAL Barrel
        MakeComponent(12, 2.24f, 3.24f, 4.975f, 0.5225f, 0, 0.02f, 9, 130, 250, 0);
        //HCAL EndcapP
        MakeComponent(12, 0.2f, 3.24f, 1f, 3.01f + (1f / 2f), 0, 0.02f, 9, 130, 250, 0);
        //HCAL EndcapN
        MakeComponent(12, 0.3f, 3.24f, 0.75f, -1.965f - (0.75f / 2f), 0, 0.02f, 9, 130, 250, 0);

        //ECAL Barrel
        MakeComponent(12, 0.955f, 1.34772f, 3.14772f, -0.23636f, 0, 0.02f, 146, 208, 80, 2);
        //ECAL EndcapP
        MakeComponent(12, .2f, 2.24f / (float)Math.Cos(Math.PI / 12), 0.48f, 2.53f + (0.48f / 2f), 0, 0.02f, 146, 208, 80, 2);
        
        //ECAL EndcapN
        MakeComponent(12, .3f, 0.955f / (float)Math.Cos(Math.PI / 12), 0.41f, -1.555f - (0.41f / 2f), 0, 0.02f, 146, 208, 80, 2);

        //Tracker Barrel
        //MakeComponent(100, .2f, .78f, 2.6f, 0.005f, 0, 0.03f, 255, 196, 47, 0.2f);
        
        //Solenoid
        MakeComponent(100, 1.6f, 2.24f, 3.84f, 0f, 0, 0.01f, 127, 127, 127, 1);

    }
    public void ToggleMenagerie()
    {
        zSlider.value = 1f;
        xySlider.value = 1f;
        GameObject[] detectorParts = GameObject.FindGameObjectsWithTag("Detector");
        if (!menagerieActive)
        {
            for (int i = 0; i < detectorParts.Length; i++)
            {
                Destroy(detectorParts[i]);
            }
            for (int i = 0; i < menagerie.Length; i++)
            {
                menagerie[i].SetActive(true);
                
            }
            menagerieActive = true;
            
        }
        else
        {
            for (int i = 0; i < menagerie.Length; i++)
            {
                menagerie[i].SetActive(false);
            }
            buildSimModel();
            
            menagerieActive = false;

        }
        
    }
    public void buildSimModel()
    {
        GameObject[] components = GameObject.FindGameObjectsWithTag("Detector");
        GameObject[] menagerie = GameObject.FindGameObjectsWithTag("Menagerie");
        for(int i = 0; i < components.Length; i++)
        {
            Destroy(components[i]);
        }
        for(int i = 0; i < menagerie.Length; i++)
        {
            menagerie[i].SetActive(false);
        }

        //HCAL Barrel
        MakeComponent(12, 2.24f, 3.24f, 4.975f, 0.5225f, 0, 0.02f, 9, 130, 250, 0);
        //HCAL EndcapP
        MakeComponent(12, 0.2f, 3.24f, 1f, 3.01f + (1f / 2f), 0, 0.02f, 9, 130, 250, 0);
        //HCAL EndcapN
        MakeComponent(12, 0.3f, 3.24f, 0.75f, -1.965f - (0.75f / 2f), 0, 0.02f, 9, 130, 250, 0);

        //ECAL Barrel
        MakeComponent(12, 0.955f, 1.34772f, 3.14772f, -0.23636f, 0, 0.02f, 146, 208, 80, 2);
        //ECAL EndcapP
        MakeComponent(12, .2f, 2.24f / (float)Math.Cos(Math.PI / 12), 0.48f, 2.53f + (0.48f / 2f), 0, 0.02f, 146, 208, 80, 2);

        //ECAL EndcapN
        MakeComponent(12, .3f, 0.955f / (float)Math.Cos(Math.PI / 12), 0.41f, -1.555f - (0.41f / 2f), 0, 0.02f, 146, 208, 80, 2);

        //Tracker Barrel
        //MakeComponent(100, .2f, .78f, 2.6f, 0.005f, 0, 0.03f, 255, 196, 47, 0.2f);

        //Solenoid
        MakeComponent(100, 1.6f, 2.24f, 3.84f, 0f, 0, 0.01f, 127, 127, 127, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleLines()
    {
        GameObject[] components = GameObject.FindGameObjectsWithTag("Detector");

        for(int i = 0; i < components.Length; i++)
        {
            
            List<GameObject> lines = new List<GameObject>();
            
            for (int j = 0; j < components[i].transform.childCount; j++)
            {
                var child = components[i].transform.GetChild(j);
                if (child.tag == "Line")
                {
                    //lines.Add(child.gameObject);
                    if (child.gameObject.activeSelf)
                    {
                        child.gameObject.SetActive(false);
                    }
                    else
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                
            }
 
        }

        

    }

    void MakeComponent(int sides, float innerR, float outerR, float length, float offset, float rotate, float lineThickness, int r, int g, int b, int renderQueue)
    {
        innerR = innerR / (float)Math.Cos(Math.PI / sides);

        if (sides % 2 == 0)
        {
            rotate = rotate + (360 / (sides * 2));
        }
        else
        {
            rotate = rotate + 90;
        }
        Vector3[] vertices = new Vector3[sides * 4];
        int[] triangles = new int[sides * 12 * 4];
        GameObject[] lines = new GameObject[sides * 8];

        int index = 0;
        int lineIndex = 0;
        for (int i = 0; i < sides; i++)
        {
            float angle = (360f / sides) * i + rotate;
            double theta = Math.PI * angle / 180.0;
            vertices[index] = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (-length / 2));
            index++;
            vertices[index] = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (-length / 2));
            index++;
            vertices[index] = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (length / 2));
            index++;
            vertices[index] = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (length / 2));
            index++;


            float angle2 = (360f / sides) * (i + 1) + rotate;
            double theta2 = Math.PI * angle2 / 180.0;
            Vector3 start;
            Vector3 end;
            LineRenderer lr = new LineRenderer();
            Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
            for (float j = (-length / 2); j <= (length / 2); j = j + length)
            {
                start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), j );
                end = new Vector3(outerR * (float)Math.Cos(theta2), outerR * (float)Math.Sin(theta2), j );
                lines[lineIndex] = new GameObject();
                lines[lineIndex].tag = "Line";
                lines[lineIndex].transform.position = start;
                lines[lineIndex].AddComponent<LineRenderer>();
                lr = lines[lineIndex].GetComponent<LineRenderer>();
                lr.material = whiteDiffuseMat;
                lr.material.renderQueue = 100;
                lr.SetWidth(lineThickness, lineThickness);
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                lineIndex++;
                if (innerR != 0)
                {
                    start = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), j );
                    end = new Vector3(innerR * (float)Math.Cos(theta2), innerR * (float)Math.Sin(theta2), j );
                    lines[lineIndex] = new GameObject();
                    lines[lineIndex].tag = "Line";
                    lines[lineIndex].transform.position = start;
                    lines[lineIndex].AddComponent<LineRenderer>();
                    lr = lines[lineIndex].GetComponent<LineRenderer>();
                    lr.material = whiteDiffuseMat;
                    lr.material.renderQueue = 100;
                    lr.SetWidth(lineThickness, lineThickness);
                    lr.SetPosition(0, start);
                    lr.SetPosition(1, end);
                    lineIndex++;
                    if (sides <= 50)
                    {
                        start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), j );
                        end = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), j );
                        lines[lineIndex] = new GameObject();
                        lines[lineIndex].tag = "Line";
                        lines[lineIndex].transform.position = start;
                        lines[lineIndex].AddComponent<LineRenderer>();
                        lr = lines[lineIndex].GetComponent<LineRenderer>();
                        lr.material = whiteDiffuseMat;
                        lr.material.renderQueue = 100;
                        lr.SetWidth(lineThickness, lineThickness);
                        lr.SetPosition(0, start);
                        lr.SetPosition(1, end);
                        lineIndex++;
                    }

                }
            }
            if (sides <= 50)
            {
                start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (-length / 2) );
                end = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (length / 2) );
                lines[lineIndex] = new GameObject();
                lines[lineIndex].tag = "Line";
                lines[lineIndex].transform.position = start;
                lines[lineIndex].AddComponent<LineRenderer>();
                lr = lines[lineIndex].GetComponent<LineRenderer>();
                lr.material = whiteDiffuseMat;
                lr.material.renderQueue = 100;
                lr.SetWidth(lineThickness, lineThickness);
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                lineIndex++;
                //float lineOffset = lineThickness / 2f;
                float lineOffset = 0f;
                if (innerR >= 0.06f)
                {
                    start = new Vector3((innerR - lineOffset) * (float)Math.Cos(theta), (innerR - lineOffset) * (float)Math.Sin(theta), (-length / 2) );
                    end = new Vector3((innerR - lineOffset) * (float)Math.Cos(theta), (innerR - lineOffset) * (float)Math.Sin(theta), (length / 2) );
                    lines[lineIndex] = new GameObject();
                    lines[lineIndex].tag = "Line";
                    lines[lineIndex].transform.position = start;
                    lines[lineIndex].AddComponent<LineRenderer>();
                    lr = lines[lineIndex].GetComponent<LineRenderer>();
                    lr.material = whiteDiffuseMat;
                    lr.material.renderQueue = 100;
                    lr.SetWidth(lineThickness, lineThickness);
                    lr.SetPosition(0, start);
                    lr.SetPosition(1, end);
                    lineIndex++;
                }
            }

        }
        index = 0;

        //front and back faces
        for (int i = 0; i < sides; i++)
        {
            for (int j = 0; j <= 2; j = j + 2)
            {
                //side 1
                triangles[index] = (i * 4) + j;
                index++;
                triangles[index] = (((i + 1) * 4) % (sides * 4)) + j;
                index++;
                triangles[index] = ((i * 4) + 1) + j;
                index++;
                triangles[index] = ((i * 4) + 1) + j;
                index++;
                triangles[index] = (((i + 1) * 4) % (sides * 4)) + j;
                index++;
                triangles[index] = ((((i + 1) * 4) % (sides * 4)) + 1) + j;
                index++;

                //side 2
                triangles[index] = ((((i + 1) * 4) % (sides * 4)) + 1) + j;
                index++;
                triangles[index] = (((i + 1) * 4) % (sides * 4)) + j;
                index++;
                triangles[index] = ((i * 4) + 1) + j;
                index++;
                triangles[index] = ((i * 4) + 1) + j;
                index++;
                triangles[index] = (((i + 1) * 4) % (sides * 4)) + j;
                index++;
                triangles[index] = (i * 4) + j;
                index++;
            }
        }

        //inner and outer faces
        for (int i = 0; i < sides; i++)
        {
            for (int j = 0; j <= 1; j++)
            {
                //outer pointing faces
                triangles[index] = (i * 4) + j;
                index++;
                triangles[index] = (((i + 1) * 4) % (sides * 4)) + j;
                index++;
                triangles[index] = (i * 4) + 2 + j;
                index++;
                triangles[index] = (i * 4) + 2 + j;
                index++;
                triangles[index] = (((i + 1) * 4) % (sides * 4)) + j;
                index++;
                triangles[index] = (((i * 4) + 6) % (sides * 4)) + j;
                index++;

                //inner pointing faces
                triangles[index] = (((i * 4) + 6) % (sides * 4)) + j;
                index++;
                triangles[index] = (((i + 1) * 4) % (sides * 4)) + j;
                index++;
                triangles[index] = (i * 4) + 2 + j;
                index++;
                triangles[index] = (i * 4) + 2 + j;
                index++;
                triangles[index] = (((i + 1) * 4) % (sides * 4)) + j;
                index++;
                triangles[index] = (i * 4) + j;
                index++;
            }
        }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        GameObject gameObject = new GameObject("Detector Piece", typeof(MeshFilter), typeof(MeshRenderer));
        gameObject.tag = "Detector";
        gameObject.GetComponent<MeshFilter>().mesh = mesh;


        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != null)
            {
                
                lines[i].transform.parent = gameObject.transform;
                lines[i].GetComponent<LineRenderer>().useWorldSpace = false;
                lines[i].transform.position = new Vector3(0, 0, 0);
                lines[i].tag = "Line";
                //lines[i].SetActive(false);
            }
        }

        Material material = new Material(Shader.Find("Transparent/Diffuse"));
        Color color = new Color(r / 255f, g / 255f, b / 255f);


        color.a = 0.3f;
        material.color = color;
        material.renderQueue = renderQueue;

        gameObject.GetComponent<MeshRenderer>().material = material;

        gameObject.transform.position = new Vector3(0, 0, offset);
        
    }
}

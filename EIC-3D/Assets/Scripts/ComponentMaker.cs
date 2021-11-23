using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
#pragma warning disable 0618

public class ComponentMaker : MonoBehaviour
{
    private bool menagerieActive = false;
    GameObject[] menagerie;
    public Slider zSlider;
    public Slider xySlider;
    public GameObject pauseMenuUI;
    public string filename = "Detector.txt";
    // Start is called before the first frame update
    void Start()
    {
        menagerie = GameObject.FindGameObjectsWithTag("Menagerie");
        for (int i = 0; i < menagerie.Length; i++)
        {
            menagerie[i].SetActive(false);
        }

        buildSimModel();
        

    }
    public void ToggleMenagerie()
    {
        GameObject[] finegridOrigin = GameObject.FindGameObjectsWithTag("FineGridOrigin");
        if(finegridOrigin.Length > 0)
        {
            Destroy(finegridOrigin[0]);
        }
        GameObject[] largegridOrigin = GameObject.FindGameObjectsWithTag("LargeGridOrigin");
        if (largegridOrigin.Length > 0)
        {
            Destroy(largegridOrigin[0]);
        }

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
        for (int i = 0; i < components.Length; i++)
        {
            Destroy(components[i]);
        }
        for (int i = 0; i < menagerie.Length; i++)
        {
            menagerie[i].SetActive(false);
        }

        var source = new StreamReader(Path.Combine(Application.streamingAssetsPath, filename));
        var fileContents = source.ReadToEnd();
        source.Close();
        var parts = fileContents.Split("\n"[0]);
        for (int i = 1; i < parts.Length; i = i + 2)
        {
            var dparams = parts[i].Split(" "[0]);

            MakeComponent(int.Parse(dparams[0]), float.Parse(dparams[1]), float.Parse(dparams[2]), float.Parse(dparams[3]),
                float.Parse(dparams[4]), float.Parse(dparams[5]), float.Parse(dparams[6]), float.Parse(dparams[7]), float.Parse(dparams[8]),
                float.Parse(dparams[9]), int.Parse(dparams[10]), int.Parse(dparams[11]), int.Parse(dparams[12]), parts.Length - i);

        }
        
    }

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
    
    void MakeComponent(int sides, float innerR, float outerR, float innerR2, float outerR2, float lengthOut, float lengthIn, float offset, float offsetIn, float rotate, int r, int g, int b, int renderQueue)
    {
 
        float lineThickness = 0.02f;
        if(outerR < 1)
        {
            lineThickness = 0.01f;
        }
        if (outerR < 0.5)
        {
            lineThickness = 0.004f;
        }

        if (sides % 2 == 0)
        {
            rotate = rotate + (360 / (sides * 2)) + 90;
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
            vertices[index] = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (-lengthOut / 2));
            index++;
            vertices[index] = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (-lengthIn / 2) + offsetIn);
            index++;
            vertices[index] = new Vector3(outerR2 * (float)Math.Cos(theta), outerR2 * (float)Math.Sin(theta), (lengthOut / 2));
            index++;
            vertices[index] = new Vector3(innerR2 * (float)Math.Cos(theta), innerR2 * (float)Math.Sin(theta), (lengthIn / 2) + offsetIn);
            index++;


            float angle2 = (360f / sides) * (i + 1) + rotate;
            double theta2 = Math.PI * angle2 / 180.0;
            Vector3 start;
            Vector3 end;
            LineRenderer lr = new LineRenderer();
            Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
            for (float j = (-0.5f); j <= 0.5f; j++)
            {
                
                start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), j * lengthOut);
                end = new Vector3(outerR * (float)Math.Cos(theta2), outerR * (float)Math.Sin(theta2), j * lengthOut);
                if(j > 0)
                {
                    start = new Vector3(outerR2 * (float)Math.Cos(theta), outerR2 * (float)Math.Sin(theta), j * lengthOut);
                    end = new Vector3(outerR2 * (float)Math.Cos(theta2), outerR2 * (float)Math.Sin(theta2), j * lengthOut);
                }
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
                if ((innerR != 0 && j < 0) ^ (innerR2 != 0 && j > 0))
                {
                    start = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (j * lengthIn) + offsetIn);
                    end = new Vector3(innerR * (float)Math.Cos(theta2), innerR * (float)Math.Sin(theta2), (j * lengthIn) + offsetIn);
                    if (j > 0)
                    {
                        start = new Vector3(innerR2 * (float)Math.Cos(theta), innerR2 * (float)Math.Sin(theta), (j * lengthIn) + offsetIn);
                        end = new Vector3(innerR2 * (float)Math.Cos(theta2), innerR2 * (float)Math.Sin(theta2), (j * lengthIn) + offsetIn);
                    }
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
                        start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), j * lengthOut );
                        end = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (j * lengthIn) + offsetIn);
                        if (j > 0)
                        {
                            start = new Vector3(outerR2 * (float)Math.Cos(theta), outerR2 * (float)Math.Sin(theta), j * lengthOut);
                            end = new Vector3(innerR2 * (float)Math.Cos(theta), innerR2 * (float)Math.Sin(theta), (j * lengthIn) + offsetIn);
                        }
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
                start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (-lengthOut / 2) );
                end = new Vector3(outerR2 * (float)Math.Cos(theta), outerR2 * (float)Math.Sin(theta), (lengthOut / 2) );
                
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
                
                
                if (innerR > 0f && innerR2 > 0f)
                {
                    start = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (-lengthIn / 2) + offsetIn );
                    end = new Vector3(innerR2 * (float)Math.Cos(theta), innerR2 * (float)Math.Sin(theta), (lengthIn / 2) + offsetIn );
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
                lines[i].SetActive(false);
            }
        }

        Material material = new Material(Shader.Find("Transparent/Diffuse"));
        Color color = new Color(r / 255f, g / 255f, b / 255f);


        color.a = 0.3f;
        material.color = color;
        material.renderQueue = renderQueue;

        gameObject.GetComponent<MeshRenderer>().sharedMaterial = material;

        gameObject.transform.position = new Vector3(0, 0, offset);
        
    }
}

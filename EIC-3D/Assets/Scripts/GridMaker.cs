using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
#pragma warning disable 0618
public class GridMaker : MonoBehaviour
{
    public Slider zSlider;
    public Slider xySlider;
    public Transform lookAhead;
    float renderDistance = 0f;
    Vector3 startPoint;
    Vector3 endPoint;
    LineRenderer liner = new LineRenderer();
    bool toggleGrid = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleGrid)
        {
            GameObject[] currentGrid = GameObject.FindGameObjectsWithTag("Grid");

            for (int i = 0; i < currentGrid.Length; i++)
            {
                liner = currentGrid[i].GetComponent<LineRenderer>();
                startPoint = liner.GetPosition(0);
                endPoint = liner.GetPosition(1);
                if (startPoint.x == -endPoint.x && endPoint.x != 0)
                {
                    if (Math.Abs(lookAhead.position.y - startPoint.y * xySlider.value) <= renderDistance && Math.Abs(lookAhead.position.z - startPoint.z * zSlider.value) <= renderDistance)
                    {
                        liner.enabled = true;
                    }
                    else
                    {
                        liner.enabled = false;
                    }
                }
                if (startPoint.y == -endPoint.y && endPoint.y != 0)
                {
                    if (Math.Abs(lookAhead.position.x - startPoint.x * xySlider.value) <= renderDistance && Math.Abs(lookAhead.position.z - startPoint.z * zSlider.value) <= renderDistance)
                    {
                        liner.enabled = true;
                    }
                    else
                    {
                        liner.enabled = false;
                    }
                }
                if (startPoint.z == -endPoint.z && endPoint.z != 0)
                {
                    if (Math.Abs(lookAhead.position.y - startPoint.y * xySlider.value) <= renderDistance && Math.Abs(lookAhead.position.x - startPoint.x * xySlider.value) <= renderDistance)
                    {
                        liner.enabled = true;
                    }
                    else
                    {
                        liner.enabled = false;
                    }
                }
            }
        }
    }

    public void RenderAll()
    {
        if (toggleGrid)
        {
            toggleGrid = false;
            GameObject[] grid = GameObject.FindGameObjectsWithTag("Grid");
            for(int i = 0; i < grid.Length; i++)
            {
                grid[i].GetComponent<LineRenderer>().enabled = true;
            }
        }
        else
        {
            toggleGrid = true;
        }
    }

    public void OneMGrid()
    {
        zSlider.value = 1f;
        xySlider.value = 1f;
        GameObject[] finegridOrigin = GameObject.FindGameObjectsWithTag("FineGridOrigin");
        GameObject[] largegridOrigin = GameObject.FindGameObjectsWithTag("LargeGridOrigin");
        if (finegridOrigin.Length > 0)
        {
            Destroy(finegridOrigin[0]);
            
        }
        else
        {
            if (largegridOrigin.Length > 0)
            {
                Destroy(largegridOrigin[0]);
                
            }
            
            MakeGrid(1f, 5);

        }

    }
    public void FiveMGrid()
    {
        zSlider.value = 1f;
        xySlider.value = 1f;
        GameObject[] finegridOrigin = GameObject.FindGameObjectsWithTag("FineGridOrigin");
        GameObject[] largegridOrigin = GameObject.FindGameObjectsWithTag("LargeGridOrigin");
        if (largegridOrigin.Length > 0)
        {
            Destroy(largegridOrigin[0]);        
        }
        else
        {
            if (finegridOrigin.Length > 0)
            {
                Destroy(finegridOrigin[0]);  
            }
            MakeGrid(5f, 15);           
        }
    }

    void MakeGrid(float spacing, int length)
    {
        renderDistance = spacing;
        GameObject gridOrigin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        if (spacing >= 5)
        {
            gridOrigin.tag = "LargeGridOrigin";
        }
        else
        {
            gridOrigin.tag = "FineGridOrigin";
        }
        gridOrigin.transform.position = new Vector3(0, 0, 0);
        gridOrigin.GetComponent<Collider>().enabled = false;
        gridOrigin.GetComponent<Renderer>().enabled = false;

        float thinLine = 0.01f;
        float thickLine = 0.04f;
        Color color;
        GameObject[] lines = new GameObject[(int)Math.Ceiling(Math.Pow((length / spacing) * 2 + 1, 2) * 3)];
        Vector3 start;
        Vector3 end;
        LineRenderer lr = new LineRenderer();
        Material whiteDiffuseMat = new Material(Shader.Find("Sprites/Default"));
        int lineIndex = 0;
        for (float i = -length; i <= length; i = (float)Math.Round(i + spacing, 2))
        {
            for (float j = -length; j <= length; j = (float)Math.Round(j + spacing, 2))
            {
                start = new Vector3(j, -length, i);
                end = new Vector3(j, length, i);

                lines[lineIndex] = new GameObject();
                lines[lineIndex].tag = "Grid";
                lines[lineIndex].transform.position = start;
                lines[lineIndex].AddComponent<LineRenderer>();
                lr = lines[lineIndex].GetComponent<LineRenderer>();
                lr.material = whiteDiffuseMat;
                lr.material.renderQueue = -1;
                if (i % 5 == 0 && j % 5 == 0)
                {
                    lr.SetWidth(thickLine, thickLine);
                    color = new Color(1, 1, 1);
                }
                else
                {
                    lr.SetWidth(thinLine, thinLine);
                    color = new Color(1, 1, 1);
                }
                lr.SetColors(color, color);
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                lineIndex++;
            }
        }
        for (float i = -length; i <= length; i = (float)Math.Round(i + spacing, 2))
        {
            for (float j = -length; j <= length; j = (float)Math.Round(j + spacing, 2))
            {
                start = new Vector3(-length, j, i);
                end = new Vector3(length, j, i);

                lines[lineIndex] = new GameObject();
                lines[lineIndex].tag = "Grid";
                lines[lineIndex].transform.position = start;
                lines[lineIndex].AddComponent<LineRenderer>();
                lr = lines[lineIndex].GetComponent<LineRenderer>();
                lr.material = whiteDiffuseMat;
                lr.material.renderQueue = -1;
                if (i % 5 == 0 && j % 5 == 0)
                {
                    lr.SetWidth(thickLine, thickLine);
                    color = new Color(1, 1, 1);
                }
                else
                {
                    lr.SetWidth(thinLine, thinLine);
                    color = new Color(1, 1, 1);
                }
                lr.SetColors(color, color);
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                lineIndex++;
            }
        }
        for (float i = -length; i <= length; i = (float)Math.Round(i + spacing, 2))
        {
            for (float j = -length; j <= length; j = (float)Math.Round(j + spacing, 2))
            {
                start = new Vector3(j, i, -length);
                end = new Vector3(j, i, length);

                lines[lineIndex] = new GameObject();
                lines[lineIndex].tag = "Grid";
                lines[lineIndex].transform.position = start;
                lines[lineIndex].AddComponent<LineRenderer>();
                lr = lines[lineIndex].GetComponent<LineRenderer>();
                lr.material = whiteDiffuseMat;
                lr.material.renderQueue = -1;
                if (i % 5 == 0 && j % 5 == 0)
                {
                    lr.SetWidth(thickLine, thickLine);
                    color = new Color(1, 1, 1);
                }
                else
                {
                    lr.SetWidth(thinLine, thinLine);
                    color = new Color(1, 1, 1);
                }
                
                lr.SetColors(color, color);
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                lineIndex++;
            }
        }

        for (int i = 0; i < lines.Length; i++)
        {

            lines[i].transform.parent = gridOrigin.transform;
            lines[i].GetComponent<LineRenderer>().useWorldSpace = false;
            lines[i].transform.position = new Vector3(0, 0, 0);
        }
    }
}

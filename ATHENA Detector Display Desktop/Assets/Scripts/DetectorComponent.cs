using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectorComponent : MonoBehaviour
{
    //private GameObject component = null;
    public int sides = 12;
    public float innerR = 1f;
    public float outerR = 2f;
    public float length = 3f;
    public float rotate = 0f;
    public float offset = 0f;
    public float alpha = 0.3f;
    public bool boarders = true;
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] vertices = new Vector3[sides * 4];
        int[] triangles = new int[sides * 12 * 4];
        GameObject[] lines = new GameObject[sides * 8];

        int index = 0;
        int lineIndex = 0;
        for (int i = 0; i < sides; i++)
        {
            float angle = (360f / sides) * i + rotate;
            double theta = Math.PI * angle / 180.0;
            vertices[index] = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (-length/2) + offset);
            index++;
            vertices[index] = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (-length / 2) + offset);
            index++;
            vertices[index] = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (length/2) + offset);
            index++;
            vertices[index] = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (length/2) + offset);
            index++;


            float angle2 = (360f / sides) * (i + 1) + rotate;
            double theta2 = Math.PI * angle2 / 180.0;
            Vector3 start;
            Vector3 end;
            LineRenderer lr = new LineRenderer();
            Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
            for (float j = (-length / 2) + offset; j <= length/2; j = j + length)
            {
                start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), j);
                end = new Vector3(outerR * (float)Math.Cos(theta2), outerR * (float)Math.Sin(theta2), j);
                lines[lineIndex] = new GameObject();
                lines[lineIndex].transform.position = start;
                lines[lineIndex].AddComponent<LineRenderer>();
                lr = lines[lineIndex].GetComponent<LineRenderer>();
                lr.material = whiteDiffuseMat;
                lr.SetWidth(0.03f, 0.03f);
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                lineIndex++;
                if (innerR != 0)
                {
                    start = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), j);
                    end = new Vector3(innerR * (float)Math.Cos(theta2), innerR * (float)Math.Sin(theta2), j);
                    lines[lineIndex] = new GameObject();
                    lines[lineIndex].transform.position = start;
                    lines[lineIndex].AddComponent<LineRenderer>();
                    lr = lines[lineIndex].GetComponent<LineRenderer>();
                    lr.material = whiteDiffuseMat;
                    lr.SetWidth(0.03f, 0.03f);
                    lr.SetPosition(0, start);
                    lr.SetPosition(1, end);
                    lineIndex++;
                    if (sides <= 50)
                    {
                        start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), j);
                        end = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), j);
                        lines[lineIndex] = new GameObject();
                        lines[lineIndex].transform.position = start;
                        lines[lineIndex].AddComponent<LineRenderer>();
                        lr = lines[lineIndex].GetComponent<LineRenderer>();
                        lr.material = whiteDiffuseMat;
                        lr.SetWidth(0.03f, 0.03f);
                        lr.SetPosition(0, start);
                        lr.SetPosition(1, end);
                        lineIndex++;
                    }

                }
            }
            if (sides <= 50)
            {
                start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (-length / 2) + offset);
                end = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (length/2) + offset);
                lines[lineIndex] = new GameObject();
                lines[lineIndex].transform.position = start;
                lines[lineIndex].AddComponent<LineRenderer>();
                lr = lines[lineIndex].GetComponent<LineRenderer>();
                lr.material = whiteDiffuseMat;
                lr.SetWidth(0.03f, 0.03f);
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                lineIndex++;
                if(innerR != 0)
                {
                    start = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (-length / 2) + offset);
                    end = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (length / 2) + offset);
                    lines[lineIndex] = new GameObject();
                    lines[lineIndex].transform.position = start;
                    lines[lineIndex].AddComponent<LineRenderer>();
                    lr = lines[lineIndex].GetComponent<LineRenderer>();
                    lr.material = whiteDiffuseMat;
                    lr.SetWidth(0.03f, 0.03f);
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
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        for (int i = 0; i < lines.Length; i++)
        {
            //lines[i].transform.SetParent(gameObject.transform);
            if (lines[i] != null)
            {
                lines[i].transform.parent = gameObject.transform;
            }
            if (!boarders)
            {
                Destroy(lines[i]);
            }
        }

        Material material = new Material(Shader.Find("Transparent/Diffuse"));
        Color color = new Color(0f, 0f, 1f);
        color.a = alpha;
        material.color = color;
        
        gameObject.GetComponent<MeshRenderer>().material = material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

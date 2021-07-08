using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComponentMaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //blue
        //Color color = new Color(9f / 255f, 130f / 255f, 250f / 255f);

        //green
        //Color color = new Color(146f / 255f, 208f / 255f, 80f / 255f);

        //yellow
        //Color color = new Color(255f / 255f, 196f / 255f, 47f / 255f);

        //HCAL Barrel
        MakeComponent(12, 2.24f, 3.24f, 4.975f, 0.5225f, 0, 0.03f, 9, 130, 250, 0.2f);
        //HCAL EndcapP
        MakeComponent(12, 0.2f, 3.24f, 1f, 3.01f + (1f / 2f), 0, 0.03f, 9, 130, 250, 0.2f);
        //HCAL EndcapN
        MakeComponent(12, 0.3f, 3.24f, 0.75f, -1.965f - (0.75f / 2f), 0, 0.03f, 9, 130, 250, 0.2f);


        //ECAL Barrel
        MakeComponent(12, 0.955f, 1.34772f, 3.14772f, -0.23636f, 0, 0.03f, 146, 208, 80, 0.2f);
        //ECAL EndcapP
        MakeComponent(12, .2f, 2.24f, 0.48f, 2.53f + (0.48f / 2f), 0, 0.03f, 146, 208, 80, 0.2f);

        //ECAL EndcapN
        MakeComponent(12, .3f, 1.6f, 0.41f, -1.555f - (0.41f / 2f), 0, 0.03f, 146, 208, 80, 0.2f);

        //Tracker Barrel
        //MakeComponent(100, .2f, .78f, 2.6f, 0.005f, 0, 0.03f, 255, 196, 47, 0.2f);

        //Solenoid
        MakeComponent(100, 1.6f, 2.24f, 3.84f, 0f, 0, 0.01f, 127, 127, 127, 0.2f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeComponent(int sides, float innerR, float outerR, float length, float offset, float rotate, float lineThickness, int r, int g, int b, float alpha)
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
            vertices[index] = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (-length / 2) + offset);
            index++;
            vertices[index] = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (-length / 2) + offset);
            index++;
            vertices[index] = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (length / 2) + offset);
            index++;
            vertices[index] = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), (length / 2) + offset);
            index++;


            float angle2 = (360f / sides) * (i + 1) + rotate;
            double theta2 = Math.PI * angle2 / 180.0;
            Vector3 start;
            Vector3 end;
            LineRenderer lr = new LineRenderer();
            Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
            for (float j = (-length / 2) ; j <= (length / 2); j = j + length)
            {
                start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), j + offset);
                end = new Vector3(outerR * (float)Math.Cos(theta2), outerR * (float)Math.Sin(theta2), j + offset);
                lines[lineIndex] = new GameObject();
                lines[lineIndex].transform.position = start;
                lines[lineIndex].AddComponent<LineRenderer>();
                lr = lines[lineIndex].GetComponent<LineRenderer>();
                lr.material = whiteDiffuseMat;
                lr.SetWidth(lineThickness, lineThickness);
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                lineIndex++;
                if (innerR != 0)
                {
                    start = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), j + offset);
                    end = new Vector3(innerR * (float)Math.Cos(theta2), innerR * (float)Math.Sin(theta2), j + offset);
                    lines[lineIndex] = new GameObject();
                    lines[lineIndex].transform.position = start;
                    lines[lineIndex].AddComponent<LineRenderer>();
                    lr = lines[lineIndex].GetComponent<LineRenderer>();
                    lr.material = whiteDiffuseMat;
                    lr.SetWidth(lineThickness, lineThickness);
                    lr.SetPosition(0, start);
                    lr.SetPosition(1, end);
                    lineIndex++;
                    if (sides <= 50)
                    {
                        Debug.Log("Test " + j);
                        start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), j + offset);
                        end = new Vector3(innerR * (float)Math.Cos(theta), innerR * (float)Math.Sin(theta), j + offset);
                        lines[lineIndex] = new GameObject();
                        lines[lineIndex].transform.position = start;
                        lines[lineIndex].AddComponent<LineRenderer>();
                        lr = lines[lineIndex].GetComponent<LineRenderer>();
                        lr.material = whiteDiffuseMat;
                        lr.SetWidth(lineThickness, lineThickness);
                        lr.SetPosition(0, start);
                        lr.SetPosition(1, end);
                        lineIndex++;
                    }

                }
            }
            if (sides <= 50)
            {
                start = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (-length / 2) + offset);
                end = new Vector3(outerR * (float)Math.Cos(theta), outerR * (float)Math.Sin(theta), (length / 2) + offset);
                lines[lineIndex] = new GameObject();
                lines[lineIndex].transform.position = start;
                lines[lineIndex].AddComponent<LineRenderer>();
                lr = lines[lineIndex].GetComponent<LineRenderer>();
                lr.material = whiteDiffuseMat;
                lr.SetWidth(lineThickness, lineThickness);
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                lineIndex++;
                float lineOffset = lineThickness / 2f;
                if (innerR >= 0.06f)
                {
                    start = new Vector3((innerR - lineOffset) * (float)Math.Cos(theta), (innerR - lineOffset) * (float)Math.Sin(theta), (-length / 2) + offset);
                    end = new Vector3((innerR - lineOffset) * (float)Math.Cos(theta), (innerR - lineOffset) * (float)Math.Sin(theta), (length / 2) + offset);
                    lines[lineIndex] = new GameObject();
                    lines[lineIndex].transform.position = start;
                    lines[lineIndex].AddComponent<LineRenderer>();
                    lr = lines[lineIndex].GetComponent<LineRenderer>();
                    lr.material = whiteDiffuseMat;
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
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != null)
            {
                lines[i].transform.parent = gameObject.transform;
            }          
        }

        Material material = new Material(Shader.Find("Transparent/Diffuse"));
        Color color = new Color(r / 255f, g / 255f, b / 255f);
        

        color.a = alpha;
        material.color = color;

        gameObject.GetComponent<MeshRenderer>().material = material;


    }
}

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
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] vertices = new Vector3[sides*4];
        int[] triangles = new int[sides * 12 * 2];

        int index = 0;
        for(int i = 0; i < sides; i++)
        {
            float angle = (360f / sides) * i;
            double theta = Math.PI * angle / 180.0;
            vertices[index] = new Vector3(outerR * (float) Math.Cos(theta), outerR * (float) Math.Sin(theta), 0f);
            index++;
            vertices[index] = new Vector3(innerR * (float) Math.Cos(theta), innerR * (float) Math.Sin(theta), 0f);
            index++;
            vertices[index] = new Vector3(outerR * (float) Math.Cos(theta), outerR * (float) Math.Sin(theta), length);
            index++;
            vertices[index] = new Vector3(innerR * (float) Math.Cos(theta), innerR * (float) Math.Sin(theta), length);
            index++;

        }
        index = 0;

        
        for(int i = 0; i < sides; i++)
        {
            //front face
            //side 1

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



        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        
        mesh.triangles = triangles;

        GameObject gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
        //GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        

        gameObject.GetComponent<MeshFilter>().mesh = mesh;

        Material material = new Material(Shader.Find("Transparent/Diffuse"));
        Color color = new Color(1f, 1f, 1f);
        color.a = 0.5f;
        material.color = color;
        
        gameObject.GetComponent<MeshRenderer>().material = material;
        //gameObject.transform.position = new Vector3(0, 0, 0);
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

        //component = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //component.GetComponent<Collider>().enabled = false;
        //component.GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

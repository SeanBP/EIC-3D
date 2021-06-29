using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class CreateSpheres : MonoBehaviour
{
    // Start is called before the first frame update
   /* void Start()
    {
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.position = new Vector3(0, 0.5f, 0);
        var filename = "Collision.txt";
        var source = new StreamReader(Application.dataPath + "/Collision Data/" + filename);
        var fileContents = source.ReadToEnd();
        source.Close();
        var lines = fileContents.Split("\n"[0]);
        int size = lines.Length;
        GameObject[] hits = new GameObject[size];
        float x = 0f;
        float y = 0f;
        float z = 0f;
        for (int i = 0; i < lines.Length; i++)
        {
            var coords = lines[i].Split(" "[0]);
            for(int j = 0; j < coords.Length; j++)
            {
                if(j == 0)
                {
                    //x = float.Parse(coords[j]);
                }
                if (j == 1)
                {
                    //y = float.Parse(coords[j]);
                }
                if (j == 2)
                {
                    //z = float.Parse(coords[j]);
                }
            }
            hits[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hits[i].transform.position = new Vector3(x, y, z);
            hits[i].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            hits[i].GetComponent<Collider>().enabled = false;

        }     
    }

    // Update is called once per frame
   */
}


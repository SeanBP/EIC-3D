using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRDebugger : MonoBehaviour
{
    GameObject[] menagerie;
    // Start is called before the first frame update
    void Start()
    {
        menagerie = GameObject.FindGameObjectsWithTag("Menagerie");
        for (int i = 0; i < menagerie.Length; i++)
        {
            menagerie[i].SetActive(false);
        }
        
        GameObject tester = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Color color = new Color(1f, 0f, 0f);
        Material material = new Material(Shader.Find("Standard"));
        material.SetFloat("_Mode", 3);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON"); material.SetFloat("_Mode", 3);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.color = color;
        material.renderQueue = -1;
        color.a = 0.3f;


        tester.GetComponent<MeshRenderer>().sharedMaterial = material;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VertexDebug : MonoBehaviour
{
    public string debugText = "Hello World";
    public static Vector3[] vertex;

    private TextMesh textMesh;


    private void Update()
    {
        if (vertex.Length > 3)
        {
            for (int i = 0; i < vertex.Length; i++)
            {

                // int cuk = vertex.FindIndex(x => x == vertex[i]);
                // if (cuk > )
                // {
                //     GameObject text2 = GameObject.Find("Vertex" + cuk);
                //     TextMesh a = text2.GetComponent<TextMesh>();
                //     a.text += $", {i}";

                // }
                // else
                // {
                //     GameObject textObject = new GameObject("Vertex" + i);
                //     textObject.transform.localScale = Vector3.one * .1f;
                //     TextMesh textMesh = textObject.AddComponent<TextMesh>();
                //     textMesh.text = $"id : {i}";
                //     textMesh.transform.position = vertex[i];
                // }
                GameObject textObject = new GameObject("Vertex" + i);
                textObject.transform.localScale = Vector3.one * .05f;
                TextMesh textMesh = textObject.AddComponent<TextMesh>();
                textMesh.text = $"id : {i}";
                textMesh.transform.position = vertex[i];
                // Mengatur teks pada TextMesh


                // Mengatur posisi TextMesh

            }
            // vertex = new Vector3[1];
            vertex = new[] { Vector3.zero };
        }
    }
}
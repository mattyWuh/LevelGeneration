using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SixVertexMesh : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        MakeMeshData();
        CreateMesh();
    }

    void MakeMeshData()
    {
        // Create array of vertices
        vertices = new Vector3[]{ 
            new Vector3(0, YValue.ins.yValue, 0), 
            Vector3.forward, 
            Vector3.right,
            Vector3.right,
            Vector3.forward, 
            new Vector3(1, 0, 1)
        };

        // Create array of integers
        triangles = new int[]{ 0, 1, 2, 3, 4, 5 };
    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}

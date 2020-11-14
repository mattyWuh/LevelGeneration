using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRender : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    public float scale = 1.0f;

    float adjScale;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        adjScale = scale * 0.5f;
    }

    public void Render(int[,] data)
    {
        VoxelData voxelData = new VoxelData(data);
        GenerateVoxelMesh(voxelData);
        UpdateMesh();
    }

    void GenerateVoxelMesh(VoxelData data)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int z = 0; z < data.Depth; z++)
        {
            for (int x = 0; x < data.Width; x++)
            {
                if (data.GetCell(x,z) == 0)
                    continue;
                
                Vector3 pos = new Vector3(x, 0, z) * scale;
                MakeCube(adjScale, pos, x, z, data);
            }
        }
    }

    void MakeCube(float cubeScale, Vector3 cubePos, int x, int z, VoxelData data)
    {

        for (int i = 0; i < 6; i++)
        {
            if (data.GetNeighbor(x, z, (Direction)i) == 0)
                MakeFace((Direction)i, cubeScale, cubePos);
        }        
    }

    void MakeFace(Direction dir, float faceScale, Vector3 facePos)
    {
        vertices.AddRange(CubeMeshData.faceVertices(dir, faceScale, facePos));
        int vCount = vertices.Count;

        triangles.Add(vCount-4);
        triangles.Add(vCount-3);
        triangles.Add(vCount-2);
        triangles.Add(vCount-4);
        triangles.Add(vCount-2);
        triangles.Add(vCount-1);
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}

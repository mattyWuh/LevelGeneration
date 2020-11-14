using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // Grid settings
    public float cellSize;
    public Vector3 gridOffset;
    public int gridSizeX;
    public int gridSizeZ;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;

    }

    void Start()
    {
        MakeContinuousProceduralGrid();
        UpdateMesh();
    }

    void MakeContinuousProceduralGrid()
    {
        // Set array sizes
        vertices = new Vector3[(gridSizeX + 1) * (gridSizeZ + 1)];
        triangles = new int[gridSizeX * gridSizeZ * 6];

        // Set tracker integers
        int vertexTracker = 0;
        int triangleTracker = 0;

        // Set vertex offset
        float vertexOffset = cellSize * 0.5f;

        // Create vertex grid
        for (int x = 0; x <= gridSizeX; x++)
        {
            for (int z = 0; z <= gridSizeZ; z++)
            {
                vertices[vertexTracker] = new Vector3((x * cellSize) - vertexOffset, Mathf.Exp(x+z)*0.2f, (z * cellSize) - vertexOffset);
                vertexTracker++;
            }
        }

        // Reset vertex tracker
        vertexTracker = 0;

        // Set each cell's triangles
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                triangles[triangleTracker] = vertexTracker;
                triangles[triangleTracker+1] = vertexTracker + 1;
                triangles[triangleTracker+2] = vertexTracker + (gridSizeZ + 1);
                triangles[triangleTracker+3] = triangles[triangleTracker+2];
                triangles[triangleTracker+4] = triangles[triangleTracker+1];
                triangles[triangleTracker+5] = vertexTracker + (gridSizeZ + 1) + 1;

                vertexTracker++;
                triangleTracker += 6;
            }
            vertexTracker++;
        }
    }

    void MakeDiscreteProceduralGrid()
    {
        // Set array sizes
        vertices = new Vector3[gridSizeX * gridSizeZ * 4];
        triangles = new int[gridSizeX * gridSizeZ * 6];

        // Set tracker integers
        int vertexTracker = 0;
        int triangleTracker = 0;

        // Set vertex offset
        float vertexOffset = cellSize * 0.5f;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 cellOffset = new Vector3(x*cellSize, 0, z*cellSize);
                int yVal = x+z;

                // Populate the vertices and triangles arrrays
                vertices[vertexTracker] = new Vector3(-vertexOffset, yVal, -vertexOffset) + gridOffset + cellOffset;
                vertices[vertexTracker+1] = new Vector3(-vertexOffset, yVal, vertexOffset) + gridOffset + cellOffset;
                vertices[vertexTracker+2] = new Vector3(vertexOffset, yVal, -vertexOffset) + gridOffset + cellOffset;
                vertices[vertexTracker+3] = new Vector3(vertexOffset, yVal, vertexOffset) + gridOffset + cellOffset;

                triangles[triangleTracker] = vertexTracker;
                triangles[triangleTracker+1] = vertexTracker + 1;
                triangles[triangleTracker+2] = vertexTracker + 2;
                triangles[triangleTracker+3] = triangles[triangleTracker+2];
                triangles[triangleTracker+4] = triangles[triangleTracker+1];
                triangles[triangleTracker+5] = vertexTracker + 3;

                vertexTracker += 4;
                triangleTracker += 6;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
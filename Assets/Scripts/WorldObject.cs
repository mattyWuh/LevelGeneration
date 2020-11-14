using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator), typeof(VoxelRender))]
public class WorldObject : MonoBehaviour
{
    MapGenerator mapGenerator;
    VoxelRender voxelRender;

    void Awake()
    {
        mapGenerator = GetComponent<MapGenerator>();
        voxelRender = GetComponent<VoxelRender>();
    }
    void Start()
    {
        GenerateWorld();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GenerateWorld();
        }
    }

    void GenerateWorld()
    {
        int[,] map = mapGenerator.GenerateMap();
        voxelRender.Render(map);
    }
}

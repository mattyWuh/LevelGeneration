using System.Collections;
using UnityEngine;

public abstract class MapGenerator : MonoBehaviour
{
    public abstract int[,] GenerateMap();
}
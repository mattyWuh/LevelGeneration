/// <summary>
/// Generates a cave-like map using cellular automata
/// </summary>

using UnityEngine;
using System;
using System.Collections;

public class CellularAutomataMap : MapGenerator 
{
    
    public int mapWidth;
    public int mapHeight;

    public string seed;

    public int smoothIters = 5;

    [Range(1,8)]
    public int adjacentWallThresh;

    [Range(0,100)]
    public int randomFillPercent;

    int [,] map;

    public override int[,] GenerateMap() 
    {
        map = new int[mapWidth, mapHeight];
        RandomFillMap();

        for (int i = 0; i < smoothIters; i++)
        {
            SmoothMap();
        }

        int borderSize = 5;
        int[,] borederedMap = new int[mapWidth + borderSize * 2, mapHeight + borderSize * 2];

        for (int width = 0; width < borederedMap.GetLength(0); width++)
        {
            for (int height = 0; height < borederedMap.GetLength(1); height++)
            {
                if (width > borderSize && width < mapWidth + borderSize && height > borderSize && height < mapHeight + borderSize)
                {
                    borederedMap[width, height] = map[width - borderSize, height - borderSize];
                }
                else
                {
                    borederedMap[width, height] = 1;
                }
            }
        }

        // Set seed to null so it can be regenerated.
        seed = null;

        return borederedMap;
    }

    void RandomFillMap()
    {
        if (seed == null)
            seed = Time.time.ToString();

        System.Random randomNumberGenerator = new System.Random(seed.GetHashCode());

        for (int width = 0; width < mapWidth; width++)
        {
            for (int height = 0; height < mapHeight; height++)
            {
                bool isBorder = (width == 0 || width == mapWidth - 1 || height == 0 || height == mapHeight - 1);
                map[width, height] = (isBorder) ? 1 : 
                    (randomNumberGenerator.Next(0,100) < randomFillPercent) ? 1 : 0;
            }
        }
    }


    void SmoothMap()
    {
        for (int width = 0; width < mapWidth; width++)
        {
            for (int height = 0; height < mapHeight; height++)
            {
                int adjWallTiles = GetSurroundingWallCount(width, height);

                if (adjWallTiles > adjacentWallThresh)
                    map[width, height] = 1;
                else if (adjWallTiles < adjacentWallThresh)
                    map[width, height] = 0;
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

        for (int adjacentX = gridX - 1; adjacentX <= gridX + 1; adjacentX++)
        {
            for (int adjacentY = gridY - 1; adjacentY <= gridY +1; adjacentY++)
            {
                bool inBounds = (adjacentX >= 0 && adjacentX < mapWidth && adjacentY >= 0 && adjacentY < mapHeight);
                if (inBounds)
                {
                    if (adjacentX != gridX || adjacentY != gridY)
                    wallCount += map[adjacentX, adjacentY];
                }
                else
                {
                    wallCount++;
                }
                
            }
        }

        return wallCount;
    }

    
}
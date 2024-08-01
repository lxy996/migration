using System.Collections.Generic;
using UnityEngine;

public class CustomTerrain : MonoBehaviour
{
    public TerrainType terrainType;
    public TerrainResource resources;
    public bool isPassable = true; // 默认可通行
    public int passCost = 1; // 默认通行消耗

    public List<CustomTerrain> neighbors = new List<CustomTerrain>(); // 邻近地块

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // 根据地形类型初始化资源和通行属性
        switch (terrainType)
        {
            case TerrainType.Grass:
                resources = new TerrainResource(5, 3, 2, 3);
                isPassable = true;
                passCost = 1;
                break;
            case TerrainType.Plains:
                resources = new TerrainResource(2, 5, 3, 0);
                isPassable = true;
                passCost = 1;
                break;
            case TerrainType.Hills:
                resources = new TerrainResource(3, 2, 5, 3);
                isPassable = true;
                passCost = 2;
                break;
            case TerrainType.Swamps:
                resources = new TerrainResource(4, 4, 4, 4);
                isPassable = true;
                passCost = 2;
                break;
            case TerrainType.BiggerRiver:
                resources = new TerrainResource(0, 0, 0, int.MaxValue, true);
                isPassable = false;
                passCost = int.MaxValue;
                break;
            case TerrainType.River:
                resources = new TerrainResource(0, 0, 0, 3);
                isPassable = true;
                passCost = 1;
                break;
            case TerrainType.Lakes:
                resources = new TerrainResource(0, 0, 0, int.MaxValue, true);
                isPassable = false;
                passCost = int.MaxValue;
                break;
            case TerrainType.Mountains:
                resources = new TerrainResource(0, 0, 0, 0);
                isPassable = false;
                passCost = int.MaxValue;
                break;
        }
    }

    public void FindNeighbors(CustomTerrain[,] grid, int x, int y)
    {
        int[] dxEven = { 1, 1, 0, -1, -1, 0 };
        int[] dyEven = { 0, -1, -1, 0, 1, 1 };
        int[] dxOdd = { 1, 1, 0, -1, -1, 0 };
        int[] dyOdd = { 1, 0, -1, -1, 0, 1 };

        int[] dx = (y % 2 == 0) ? dxEven : dxOdd;
        int[] dy = (y % 2 == 0) ? dyEven : dyOdd;

        for (int i = 0; i < 6; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (nx >= 0 && ny >= 0 && nx < grid.GetLength(0) && ny < grid.GetLength(1))
            {
                CustomTerrain neighbor = grid[nx, ny];
                if (neighbor != null)
                {
                    neighbors.Add(neighbor);
                }
            }
        }
    }

    public void UpdateSurroundingWater()
    {
        if (terrainType == TerrainType.Lakes)
        {
            foreach (var neighbor in neighbors)
            {
                neighbor.resources.water = int.MaxValue;
                neighbor.resources.isWaterInfinite = true;
            }
        }
        else if (terrainType == TerrainType.River)
        {
            foreach (var neighbor in neighbors)
            {
                neighbor.resources.water += 3;
            }
        }
    }
}
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public CustomTerrain[,] grid;
    public int gridWidth;
    public int gridHeight;

    void Start()
    {
        InitializeGrid();
        SetNeighbors();
    }

    void InitializeGrid()
    {
        // 初始化网格，假设所有地块已经手动放置并分配到grid中
        grid = new CustomTerrain[gridWidth, gridHeight];
        CustomTerrain[] allTerrains = FindObjectsOfType<CustomTerrain>();
        foreach (CustomTerrain terrain in allTerrains)
        {
            Vector3 position = terrain.transform.position;
            int x = Mathf.RoundToInt(position.x / (terrain.GetComponent<Renderer>().bounds.size.x * 1.5f));
            int y = Mathf.RoundToInt(position.z / (terrain.GetComponent<Renderer>().bounds.size.z * Mathf.Sqrt(3) / 2));
            grid[x, y] = terrain;
        }
    }

    void SetNeighbors()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].FindNeighbors(grid, x, y);
                }
            }
        }
    }
}

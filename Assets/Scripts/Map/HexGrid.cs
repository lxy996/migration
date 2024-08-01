using System.IO;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float hexSize = 1f;
    public GameObject[] terrainPrefabs; // 存储多个地形预制体
    public HexCell[,] cells;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        cells = new HexCell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = CalculateHexPosition(x, y);
                GameObject hexGO = Instantiate(terrainPrefabs[0], position, Quaternion.identity, transform); // 默认放置第一个预制体
                cells[x, y] = hexGO.GetComponent<HexCell>();
                cells[x, y].coordinates = new Vector2Int(x, y);
            }
        }
    }

    public Vector3 CalculateHexPosition(int x, int y)
    {
        float width = Mathf.Sqrt(3) * hexSize;
        float height = 2 * hexSize;
        float horizontalSpacing = width * 0.75f;
        float verticalSpacing = height * 0.5f;

        float offsetX = x * horizontalSpacing;
        float offsetY = y * height + (x % 2 == 0 ? 0 : height / 2);

        return new Vector3(offsetX, 0, offsetY);
    }

    public HexCell GetCellAt(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / (Mathf.Sqrt(3) * hexSize * 0.75f));
        int y = Mathf.RoundToInt(position.z / (hexSize * 2));

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return cells[x, y];
        }
        return null;
    }

    public void SaveMap(string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    HexCell cell = cells[x, y];
                    if (cell != null)
                    {
                        foreach (var entry in cell.GetAllObjects())
                        {
                            string type = entry.Key;
                            foreach (var obj in entry.Value)
                            {
                                string objName = obj.name.Replace("(Clone)", "").Trim();
                                writer.WriteLine($"{x},{y},{type},{objName}");
                            }
                        }
                    }
                }
            }
        }
    }

    public void LoadMap(string fileName)
    {
        using (StreamReader reader = new StreamReader(fileName))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                int x = int.Parse(parts[0]);
                int y = int.Parse(parts[1]);
                string type = parts[2];
                string objectName = parts[3];

                Vector3 position = CalculateHexPosition(x, y);
                GameObject prefab = Resources.Load<GameObject>($"Prefabs/{objectName}");
                if (prefab != null)
                {
                    GameObject hexGO = Instantiate(prefab, position, Quaternion.identity, transform);
                    HexCell cell = cells[x, y] = hexGO.GetComponent<HexCell>();
                    cell.AddObject(type, hexGO);
                }
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexGrid : MonoBehaviour
{
    public int radius = 3; // 大六边形的半径（层数），如3层就会包含7个六边形
    public float hexSize = 1f;
    public HexCell hexCellPrefab; // 用于生成小六边形网格的HexCell预制体
    public List<HexCell> cells = new List<HexCell>();

    void Start()
    {
        CreateHexagonGrid();
    }

    void OnValidate()
    {
        CreateHexagonGrid(); // 每次调整参数时，自动生成网格
    }

    void CreateHexagonGrid()
    {
        ClearGrid(); // 清除现有的网格

        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                CreateHexCell(q, r);
            }
        }
    }

    void ClearGrid()
    {
        // 区分运行时和编辑模式下的清除策略
        if (Application.isPlaying)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        cells.Clear(); // 清空列表
    }

    void CreateHexCell(int q, int r)
    {
        Vector3 position = CalculateHexPosition(q, r);

        // 创建一个旋转：X 轴 90 度，Z 轴 30 度
        Quaternion rotation = Quaternion.Euler(90f, 0f, 30f);

        HexCell cell = Instantiate(hexCellPrefab, position, rotation, transform);
        cell.coordinates = new Vector2Int(q, r);
        cells.Add(cell);
    }

    public Vector3 CalculateHexPosition(int q, int r)
    {
        float x = hexSize * (Mathf.Sqrt(3) * q + Mathf.Sqrt(3) / 2 * r);
        float z = hexSize * (3f / 2f * r);
        return new Vector3(x, 0, z);
    }
}
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexGrid : MonoBehaviour
{
    public int radius = 3;  // 六边形网格的半径
    public GameObject terrainPrefab;  // 地形预制体，代表六边形的外观
    public List<HexCell> cells = new List<HexCell>();  // 存储HexCell

    private float hexWidth;   // 六边形的宽度
    private float hexHeight;  // 六边形的高度

    // 通过地形预制体计算六边形的尺寸
    void CalculateHexSize()
    {
        if (terrainPrefab != null)
        {
            Renderer terrainRenderer = terrainPrefab.GetComponent<Renderer>();
            if (terrainRenderer != null)
            {
                hexWidth = terrainRenderer.bounds.size.x;  // 获取地形预制体的宽度
                hexHeight = terrainRenderer.bounds.size.z; // 获取地形预制体的高度
            }
            else
            {
                Debug.LogError("地形预制体没有 Renderer 组件");
            }
        }
        else
        {
            Debug.LogError("未指定地形预制体");
        }
    }

    public void CreateHexagonGrid()
    {
        ClearGrid();  // 清除现有网格
        CalculateHexSize();  // 计算六边形尺寸

        // 生成六边形网格
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

    public void ClearGrid()
    {
        // 立即销毁现有的HexCell对象
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        cells.Clear();  // 清空HexCell列表
    }

    void CreateHexCell(int q, int r)
    {
        // 计算每个六边形的位置
        Vector3 position = CalculateHexPosition(q, r);

        // 实例化 HexCell 逻辑对象
        GameObject hexCellObject = new GameObject("HexCell");
        HexCell cell = hexCellObject.AddComponent<HexCell>();  // 新建 HexCell 对象
        cell.transform.position = position;
        cell.transform.parent = transform;
        cell.coordinates = new Vector2Int(q, r);
        cells.Add(cell);

        // 设置地形预制体（初始地形）
        cell.SetTerrain(terrainPrefab);  // 这里你可以传入不同的地形预制体
    }

    // 为所有存在的 HexCell 重新设置地形
    public void SetAllHexCellTerrain()
    {
        // 获取所有仍然存在的 HexCell
        HexCell[] hexCells = transform.GetComponentsInChildren<HexCell>();

        foreach (HexCell cell in hexCells)
        {
            if (cell != null && cell.terrainPrefab != null)  // 确保 HexCell 存在且有地形预制体
            {
                cell.SetTerrain(cell.terrainPrefab);  // 让每个 HexCell 使用自己的 terrainPrefab 设置地形
            }
        }
    }
    public Vector3 CalculateHexPosition(int q, int r)
    {
        float x = hexWidth * (3f / 4f * q); // 水平方向：六边形高度的 3/4 倍
        float z = hexWidth * Mathf.Sqrt(3) / 2 * (r + q / 2f);  // 垂直方向：调整为高度的 sqrt(3)/2 倍确保错位

        return new Vector3(x, 0, z);
    }
}
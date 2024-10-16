using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexGrid : MonoBehaviour
{
    public int radius = 3;  // ����������İ뾶
    public GameObject terrainPrefab;  // ����Ԥ���壬���������ε����
    public List<HexCell> cells = new List<HexCell>();  // �洢HexCell

    private float hexWidth;   // �����εĿ��
    private float hexHeight;  // �����εĸ߶�

    // ͨ������Ԥ������������εĳߴ�
    void CalculateHexSize()
    {
        if (terrainPrefab != null)
        {
            Renderer terrainRenderer = terrainPrefab.GetComponent<Renderer>();
            if (terrainRenderer != null)
            {
                hexWidth = terrainRenderer.bounds.size.x;  // ��ȡ����Ԥ����Ŀ��
                hexHeight = terrainRenderer.bounds.size.z; // ��ȡ����Ԥ����ĸ߶�
            }
            else
            {
                Debug.LogError("����Ԥ����û�� Renderer ���");
            }
        }
        else
        {
            Debug.LogError("δָ������Ԥ����");
        }
    }

    public void CreateHexagonGrid()
    {
        ClearGrid();  // �����������
        CalculateHexSize();  // ���������γߴ�

        // ��������������
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
        // �����������е�HexCell����
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        cells.Clear();  // ���HexCell�б�
    }

    void CreateHexCell(int q, int r)
    {
        // ����ÿ�������ε�λ��
        Vector3 position = CalculateHexPosition(q, r);

        // ʵ���� HexCell �߼�����
        GameObject hexCellObject = new GameObject("HexCell");
        HexCell cell = hexCellObject.AddComponent<HexCell>();  // �½� HexCell ����
        cell.transform.position = position;
        cell.transform.parent = transform;
        cell.coordinates = new Vector2Int(q, r);
        cells.Add(cell);

        // ���õ���Ԥ���壨��ʼ���Σ�
        cell.SetTerrain(terrainPrefab);  // ��������Դ��벻ͬ�ĵ���Ԥ����
    }

    // Ϊ���д��ڵ� HexCell �������õ���
    public void SetAllHexCellTerrain()
    {
        // ��ȡ������Ȼ���ڵ� HexCell
        HexCell[] hexCells = transform.GetComponentsInChildren<HexCell>();

        foreach (HexCell cell in hexCells)
        {
            if (cell != null && cell.terrainPrefab != null)  // ȷ�� HexCell �������е���Ԥ����
            {
                cell.SetTerrain(cell.terrainPrefab);  // ��ÿ�� HexCell ʹ���Լ��� terrainPrefab ���õ���
            }
        }
    }
    public Vector3 CalculateHexPosition(int q, int r)
    {
        float x = hexWidth * (3f / 4f * q); // ˮƽ���������θ߶ȵ� 3/4 ��
        float z = hexWidth * Mathf.Sqrt(3) / 2 * (r + q / 2f);  // ��ֱ���򣺵���Ϊ�߶ȵ� sqrt(3)/2 ��ȷ����λ

        return new Vector3(x, 0, z);
    }
}
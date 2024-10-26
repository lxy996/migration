using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexGrid : MonoBehaviour
{
    public int radius = 3;  // ����������İ뾶
    public GameObject terrainPrefab;  // ����Ԥ���壬���������ε����
    public GameObject highlightPrefab; // ���ڸ���������
    public GameObject pathIndicatorPrefab; // ������ʾ·��������
    public GameObject turnIndicatorPrefab;  // ������ʾ�غ���������
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

    // ��ȡָ�� HexCell ���ھ�
    public List<HexCell> GetNeighbors(HexCell cell)
    {
        List<HexCell> neighbors = new List<HexCell>();

        // ���������ڷ����ƫ����
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(1, 0),  // ��
        new Vector2Int(1, -1), // ����
        new Vector2Int(0, -1), // ����
        new Vector2Int(-1, 0), // ��
        new Vector2Int(-1, 1), // ����
        new Vector2Int(0, 1)   // ����
        };

        foreach (Vector2Int dir in directions)
        {
            // �����ھӵ�����
            Vector2Int neighborCoords = cell.coordinates + dir;

            // �����ھӵ�Ԫ��ȷ������Ϊ null
            HexCell neighbor = cells.Find(c => c != null && c.coordinates == neighborCoords);

            // ֻ����ǿյ� HexCell
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
                Debug.Log($"�ҵ��ھӣ���ǰ��Ԫ������: {cell.coordinates}, �ھ�����: {neighbor.coordinates}");
            }
            else
            {
                Debug.LogWarning($"δ�ҵ��ھӣ���ǰ��Ԫ������: {cell.coordinates}, ���Բ��ҵ��ھ�����: {neighborCoords}");
            }
        }

        return neighbors;
    }

    // ��ȡ��ǰ�غϵ�λ���ƶ��ĵ�Ԫ��
    public List<HexCell> GetMovableCells(HexCell start, int moveRange)
    {
        List<HexCell> movableCells = new List<HexCell>();
        Queue<HexCell> frontier = new Queue<HexCell>();
        frontier.Enqueue(start);

        Dictionary<HexCell, int> costSoFar = new Dictionary<HexCell, int>();
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            HexCell current = frontier.Dequeue();

            foreach (HexCell neighbor in GetNeighbors(current))
            {
                int newCost = costSoFar[current] + neighbor.GetPassCost();
                if (newCost <= moveRange && (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor]))
                {
                    costSoFar[neighbor] = newCost;
                    frontier.Enqueue(neighbor);
                    movableCells.Add(neighbor);
                }
            }
        }
        return movableCells;
    }

    // �������ƶ��ĵ�Ԫ��
    public void HighlightMovableCells(List<HexCell> movableCells)
    {
        foreach (HexCell cell in movableCells)
        {
            // ����һ���������壬���ӵ���Ԫ��
            GameObject highlight = Instantiate(highlightPrefab, cell.transform.position + Vector3.up * 1.5f, Quaternion.identity);
            cell.SetHighlight(highlight); // HexCell ��Ҫ�洢�����Ķ���
        }
    }

    // ������и���
    public void ClearHighlights()
    {
        foreach (HexCell cell in cells)
        {
            cell.ClearHighlight(); // �Ƴ�ÿ����Ԫ���ϵĸ���
        }
    }

    // ��ʾ·��
    public void ShowPath(List<HexCell> path, int turns)
    {
        foreach (HexCell cell in path)
        {
            GameObject pathIndicator = Instantiate(pathIndicatorPrefab, cell.transform.position + Vector3.up * 1.5f, Quaternion.identity);
            cell.SetPathIndicator(pathIndicator);
        }

        // ��·���յ���ʾ�غ���
        HexCell lastCell = path[path.Count - 1];
        GameObject turnIndicator = Instantiate(turnIndicatorPrefab, lastCell.transform.position + Vector3.up * 1.5f, Quaternion.identity);
        turnIndicator.GetComponent<TextMesh>().text = turns.ToString(); // ���ûغ���
        lastCell.SetTurnIndicator(turnIndicator);
    }

    public void ClearPath()
    {
        foreach (HexCell cell in cells)
        {
            cell.ClearPathIndicator();
            cell.ClearTurnIndicator();
        }
    }
}
using UnityEngine;

public class GizmosDrawer : MonoBehaviour
{
    public HexGrid hexGrid;
    public float hexHeight = 0.5f; // �������ĸ߶�

    void OnDrawGizmos()
    {
        if (hexGrid == null)
        {
            hexGrid = GetComponent<HexGrid>();
        }

        if (hexGrid != null && hexGrid.cells != null)
        {
            Gizmos.color = Color.green; // ����Gizmos����ɫ
            for (int x = 0; x < hexGrid.width; x++)
            {
                for (int y = 0; y < hexGrid.height; y++)
                {
                    HexCell cell = hexGrid.cells[x, y];
                    if (cell != null)
                    {
                        DrawHexagon(cell.transform.position, hexGrid.hexSize, hexHeight);
                    }
                }
            }
        }
    }

    void DrawHexagon(Vector3 position, float radius, float height)
    {
        Vector3[] vertices = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i);
            vertices[i] = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle)) + position;
        }

        // ���������εĵײ�
        for (int i = 0; i < 6; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[(i + 1) % 6]);
        }

        // ���������εĶ���
        for (int i = 0; i < 6; i++)
        {
            Vector3 topVertex = vertices[i] + new Vector3(0, height, 0);
            Vector3 nextTopVertex = vertices[(i + 1) % 6] + new Vector3(0, height, 0);
            Gizmos.DrawLine(topVertex, nextTopVertex);
        }

        // ���������εĴ�ֱ��
        for (int i = 0; i < 6; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[i] + new Vector3(0, height, 0));
        }
    }
}


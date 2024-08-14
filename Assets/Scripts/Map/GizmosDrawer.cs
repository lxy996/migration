using UnityEngine;

public class GizmosDrawer : MonoBehaviour
{
    public HexGrid hexGrid;
    public float hexSize = 1f;

    void OnDrawGizmos()
    {
        if (hexGrid == null)
        {
            hexGrid = GetComponent<HexGrid>(); // 尝试获取同一GameObject上的HexGrid组件
        }

        if (hexGrid != null && hexGrid.cells != null)
        {
            Gizmos.color = Color.green;
            foreach (var cell in hexGrid.cells)
            {
                if (cell != null)
                {
                    DrawHexagon(cell.transform.position);

                    // 高亮显示特定类型的物体
                    var objects = cell.GetObjectsByType("Reward");
                    if (objects != null)
                    {
                        Gizmos.color = Color.yellow;
                        foreach (var obj in objects)
                        {
                            Gizmos.DrawSphere(obj.transform.position, 0.1f); // 使用小球表示
                        }
                    }
                }
            }
        }
    }

    void DrawHexagon(Vector3 position)
    {
        float radius = hexSize;
        for (int i = 0; i < 6; i++)
        {
            float angle1 = Mathf.PI / 3 * i;
            float angle2 = Mathf.PI / 3 * (i + 1);
            Vector3 point1 = position + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
            Vector3 point2 = position + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;
            Gizmos.DrawLine(point1, point2);
        }
    }
}


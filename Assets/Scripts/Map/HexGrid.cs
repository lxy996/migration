using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexGrid : MonoBehaviour
{
    public int radius = 3; // �������εİ뾶������������3��ͻ����7��������
    public float hexSize = 1f;
    public HexCell hexCellPrefab; // ��������С�����������HexCellԤ����
    public List<HexCell> cells = new List<HexCell>();

    void Start()
    {
        CreateHexagonGrid();
    }

    void OnValidate()
    {
        CreateHexagonGrid(); // ÿ�ε�������ʱ���Զ���������
    }

    void CreateHexagonGrid()
    {
        ClearGrid(); // ������е�����

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
        // ��������ʱ�ͱ༭ģʽ�µ��������
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

        cells.Clear(); // ����б�
    }

    void CreateHexCell(int q, int r)
    {
        Vector3 position = CalculateHexPosition(q, r);

        // ����һ����ת��X �� 90 �ȣ�Z �� 30 ��
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
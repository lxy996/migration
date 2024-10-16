using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // ��ʾĬ�ϵ� Inspector ����
        DrawDefaultInspector();

        HexGrid hexGrid = (HexGrid)target;

        // ���һ����ť����������ɵ�ͼ
        if (GUILayout.Button("���ɵ�ͼ"))
        {
            hexGrid.CreateHexagonGrid();
            EditorUtility.SetDirty(hexGrid);
        }

        // ���һ����ť������������ͼ
        if (GUILayout.Button("�����ͼ"))
        {
            hexGrid.ClearGrid();
            EditorUtility.SetDirty(hexGrid);
        }

        // ���һ����ť�������Ϊ�����ִ�� HexCell �������õ���
        if (GUILayout.Button("������������ HexCell �ĵ���"))
        {
            hexGrid.SetAllHexCellTerrain();  // �����������е��εķ���
            EditorUtility.SetDirty(hexGrid);
        }
    }
}

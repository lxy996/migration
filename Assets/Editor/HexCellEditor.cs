using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexCell))]
public class HexCellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // ��ʾĬ�ϵ� Inspector ����
        DrawDefaultInspector();

        HexCell hexCell = (HexCell)target;

        // ���һ����ť������������ǰ����
        if (GUILayout.Button("�������"))
        {
            hexCell.ClearTerrain();  // �ֶ������ǰ����
            EditorUtility.SetDirty(hexCell);  // ��� HexCell Ϊ�����ݣ�ȷ�����ı���
        }

        // ���һ����ť����������õ���
        if (GUILayout.Button("���õ���"))
        {
            hexCell.SetTerrain(hexCell.terrainPrefab);  // �ֶ����õ���
            EditorUtility.SetDirty(hexCell);  // ��� HexCell Ϊ�����ݣ�ȷ�����ı���
        }
    }
}

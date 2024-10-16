using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexCell))]
public class HexCellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 显示默认的 Inspector 界面
        DrawDefaultInspector();

        HexCell hexCell = (HexCell)target;

        // 添加一个按钮，点击后清除当前地形
        if (GUILayout.Button("清除地形"))
        {
            hexCell.ClearTerrain();  // 手动清除当前地形
            EditorUtility.SetDirty(hexCell);  // 标记 HexCell 为脏数据，确保更改保存
        }

        // 添加一个按钮，点击后设置地形
        if (GUILayout.Button("设置地形"))
        {
            hexCell.SetTerrain(hexCell.terrainPrefab);  // 手动设置地形
            EditorUtility.SetDirty(hexCell);  // 标记 HexCell 为脏数据，确保更改保存
        }
    }
}

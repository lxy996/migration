using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 显示默认的 Inspector 界面
        DrawDefaultInspector();

        HexGrid hexGrid = (HexGrid)target;

        // 添加一个按钮，点击后生成地图
        if (GUILayout.Button("生成地图"))
        {
            hexGrid.CreateHexagonGrid();
            EditorUtility.SetDirty(hexGrid);
        }

        // 添加一个按钮，点击后清除地图
        if (GUILayout.Button("清除地图"))
        {
            hexGrid.ClearGrid();
            EditorUtility.SetDirty(hexGrid);
        }

        // 添加一个按钮，点击后为所有现存的 HexCell 重新设置地形
        if (GUILayout.Button("重新设置所有 HexCell 的地形"))
        {
            hexGrid.SetAllHexCellTerrain();  // 调用设置所有地形的方法
            EditorUtility.SetDirty(hexGrid);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexCell : MonoBehaviour
{
    public Vector2Int coordinates;  // 存储单元格坐标
    public GameObject terrainPrefab;  // 手动拖入的地形预制体
    private GameObject instantiatedPrefab;  // 保存当前实例化的地形预制体
    public CustomTerrain customTerrain;  // 引用 CustomTerrain 组件
    private GameObject highlight;// 用于高亮的物体
    private GameObject pathIndicator;// 用于显示路径的物体
    private GameObject turnIndicator;// 用于显示回合数的物体
    // 用于存储不同类型的对象，如单位、建筑等
    private Dictionary<string, List<GameObject>> objectsInCell = new Dictionary<string, List<GameObject>>();

    void Start()
    {
        // 获取当前 HexCell 下的 CustomTerrain 组件
        customTerrain = GetComponentInChildren<CustomTerrain>();
    }

    // 获取该单元格的通行消耗
    public int GetPassCost()
    {
        if (customTerrain != null)
        {
            return customTerrain.passCost;
        }
        return int.MaxValue;  // 如果没有 CustomTerrain，返回最大消耗，表示不可通行
    }

    // Manually called method to clear the current terrain
    public void ClearTerrain()
    {
        // If a terrain prefab instance already exists, destroy it
        if (instantiatedPrefab != null)
        {
            DestroyImmediate(instantiatedPrefab);  // Immediately destroy in editor mode
        }
    }
    // Manually called method to set the terrain
    public void SetTerrain(GameObject newTerrainPrefab)
    {
        // First, clear the previous terrain
        ClearTerrain();
        // Assign the new terrain prefab
        terrainPrefab = newTerrainPrefab;
        // Get the height range from the TerrainHeightRange component
        TerrainHeightRange heightRange = terrainPrefab.GetComponent<TerrainHeightRange>();
        if (heightRange != null)
        {
            // Randomly generate height within the specified range
            float randomHeight = Random.Range(heightRange.minHeight, heightRange.maxHeight);
            Vector3 terrainPosition = new Vector3(transform.position.x, randomHeight, transform.position.z);

            // Randomly select a rotation angle on the Y-axis (60, 120, 180, 240, 300, 360 degrees)
            int[] possibleRotations = { 60, 120, 180, 240, 300, 360 };
            int randomRotation = possibleRotations[Random.Range(0, possibleRotations.Length)];
            Quaternion randomYRotation = Quaternion.Euler(0, randomRotation, 0);  // Rotate only around the Y-axis

            // Instantiate and store the new terrain prefab
            if (terrainPrefab != null)
            {
                instantiatedPrefab = Instantiate(terrainPrefab, terrainPosition, randomYRotation, transform);  // Apply random rotation and height
                instantiatedPrefab.name = terrainPrefab.name;  // Remove the (Clone) suffix
                instantiatedPrefab.transform.localPosition = new Vector3(0, randomHeight, 0);  // Ensure the position is correct
            }
        }
        else
        {
            Debug.LogWarning("The terrain prefab lacks the TerrainHeightRange component");
        }
    }

    // 设置高亮物体
    public void SetHighlight(GameObject highlightObj)
    {
        highlight = highlightObj;
    }

    // 清除高亮
    public void ClearHighlight()
    {
        if (highlight != null)
        {
            Destroy(highlight);
        }
    }

    // 设置路径指示物体
    public void SetPathIndicator(GameObject pathObj)
    {
        pathIndicator = pathObj;
    }
    public void SetTurnIndicator(GameObject turnObj)
    {
        turnIndicator = turnObj;
    }

    // 清除路径指示
    public void ClearPathIndicator()
    {
        if (pathIndicator != null)
        {
            Destroy(pathIndicator);
        }
    }

    public void ClearTurnIndicator()
    {
        if (turnIndicator != null)
        {
            Destroy(turnIndicator);
        }
    }

    // 添加对象到单元格
    public void AddObject(string type, GameObject obj)
    {
        if (!objectsInCell.ContainsKey(type))
        {
            objectsInCell[type] = new List<GameObject>();
        }
        objectsInCell[type].Add(obj);
    }

    // 按类型获取对象列表
    public List<GameObject> GetObjectsByType(string type)
    {
        if (objectsInCell.ContainsKey(type))
        {
            return objectsInCell[type];
        }
        return null;
    }

    // 从类型列表中移除对象
    public void RemoveObject(string type, GameObject obj)
    {
        if (objectsInCell.ContainsKey(type))
        {
            objectsInCell[type].Remove(obj);
            if (objectsInCell[type].Count == 0)
            {
                objectsInCell.Remove(type);
            }
        }
    }

    // 在编辑器中显示 HexCell 坐标
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.color = Color.white;
        Gizmos.DrawIcon(transform.position, "HexIcon.png", true);  // 可选：显示坐标
    }
}
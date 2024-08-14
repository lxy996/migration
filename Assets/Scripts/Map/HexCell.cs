using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Vector2Int coordinates;
    public GameObject terrainPrefab;  // 用于手动拖入地形预制体
    private GameObject instantiatedPrefab;  // 保存实例化的预制体

    // 用于存储不同类型的对象
    private Dictionary<string, List<GameObject>> objectsInCell = new Dictionary<string, List<GameObject>>();

    void OnValidate()
    {
        // 如果有新的地形预制体，则在当前单元格中实例化它
        if (terrainPrefab != null)
        {
            if (instantiatedPrefab != null)
            {
                DestroyImmediate(instantiatedPrefab);
            }
            instantiatedPrefab = Instantiate(terrainPrefab, transform.position, Quaternion.identity, transform);
            instantiatedPrefab.name = terrainPrefab.name; // 移除(Clone)后缀

            // 添加到对象列表中（如果需要的话，可以选择分类存储）
            AddObject("Terrain", instantiatedPrefab);
        }
    }

    // 添加对象到特定类型的列表中
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

    void OnDrawGizmos()
    {
        // 在编辑器中显示单元格位置的一个小图标
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
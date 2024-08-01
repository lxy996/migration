using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Vector2Int coordinates;
    private Dictionary<string, List<GameObject>> objectsInCell;

    void Awake()
    {
        objectsInCell = new Dictionary<string, List<GameObject>>();
    }

    public void AddObject(string type, GameObject obj)
    {
        if (!objectsInCell.ContainsKey(type))
        {
            objectsInCell[type] = new List<GameObject>();
        }
        objectsInCell[type].Add(obj);
    }

    public List<GameObject> GetObjectsByType(string type)
    {
        if (objectsInCell.ContainsKey(type))
        {
            return objectsInCell[type];
        }
        return null;
    }

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

    public Dictionary<string, List<GameObject>> GetAllObjects()
    {
        return objectsInCell;
    }
}
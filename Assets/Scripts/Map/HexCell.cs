using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Vector2Int coordinates;
    public GameObject terrainPrefab;  // �����ֶ��������Ԥ����
    private GameObject instantiatedPrefab;  // ����ʵ������Ԥ����

    // ���ڴ洢��ͬ���͵Ķ���
    private Dictionary<string, List<GameObject>> objectsInCell = new Dictionary<string, List<GameObject>>();

    void OnValidate()
    {
        // ������µĵ���Ԥ���壬���ڵ�ǰ��Ԫ����ʵ������
        if (terrainPrefab != null)
        {
            if (instantiatedPrefab != null)
            {
                DestroyImmediate(instantiatedPrefab);
            }
            instantiatedPrefab = Instantiate(terrainPrefab, transform.position, Quaternion.identity, transform);
            instantiatedPrefab.name = terrainPrefab.name; // �Ƴ�(Clone)��׺

            // ��ӵ������б��У������Ҫ�Ļ�������ѡ�����洢��
            AddObject("Terrain", instantiatedPrefab);
        }
    }

    // ��Ӷ����ض����͵��б���
    public void AddObject(string type, GameObject obj)
    {
        if (!objectsInCell.ContainsKey(type))
        {
            objectsInCell[type] = new List<GameObject>();
        }
        objectsInCell[type].Add(obj);
    }

    // �����ͻ�ȡ�����б�
    public List<GameObject> GetObjectsByType(string type)
    {
        if (objectsInCell.ContainsKey(type))
        {
            return objectsInCell[type];
        }
        return null;
    }

    // �������б����Ƴ�����
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
        // �ڱ༭������ʾ��Ԫ��λ�õ�һ��Сͼ��
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
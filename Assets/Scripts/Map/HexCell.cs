using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexCell : MonoBehaviour
{
    public Vector2Int coordinates;  // �洢��Ԫ������
    public GameObject terrainPrefab;  // �ֶ�����ĵ���Ԥ����
    private GameObject instantiatedPrefab;  // ���浱ǰʵ�����ĵ���Ԥ����
    public CustomTerrain customTerrain;  // ���� CustomTerrain ���
    private GameObject highlight;// ���ڸ���������
    private GameObject pathIndicator;// ������ʾ·��������
    private GameObject turnIndicator;// ������ʾ�غ���������
    // ���ڴ洢��ͬ���͵Ķ����絥λ��������
    private Dictionary<string, List<GameObject>> objectsInCell = new Dictionary<string, List<GameObject>>();

    void Start()
    {
        // ��ȡ��ǰ HexCell �µ� CustomTerrain ���
        customTerrain = GetComponentInChildren<CustomTerrain>();
    }

    // ��ȡ�õ�Ԫ���ͨ������
    public int GetPassCost()
    {
        if (customTerrain != null)
        {
            return customTerrain.passCost;
        }
        return int.MaxValue;  // ���û�� CustomTerrain������������ģ���ʾ����ͨ��
    }

    // �ֶ����õķ��������������ǰ����
    public void ClearTerrain()
    {
        // ����Ѿ���ʵ�����ĵ���Ԥ���壬������
        if (instantiatedPrefab != null)
        {
            DestroyImmediate(instantiatedPrefab);  // �ڱ༭��ģʽ����������
        }
    }

    // �ֶ����õķ������������õ���
    public void SetTerrain(GameObject newTerrainPrefab)
    {
        // �����֮ǰ�ĵ���
        ClearTerrain();

        // ��ֵ�µĵ���Ԥ����
        terrainPrefab = newTerrainPrefab;

        // ��ȡ TerrainHeightRange ����еĸ߶ȷ�Χ
        TerrainHeightRange heightRange = terrainPrefab.GetComponent<TerrainHeightRange>();
        if (heightRange != null)
        {
            // ��ָ���ķ�Χ��������ɸ߶�
            float randomHeight = Random.Range(heightRange.minHeight, heightRange.maxHeight);
            Vector3 terrainPosition = new Vector3(transform.position.x, randomHeight, transform.position.z);

            // ���ѡ�� Y �����ת�Ƕȣ�60��120��180��240��300��360�ȣ�
            int[] possibleRotations = { 60, 120, 180, 240, 300, 360 };
            int randomRotation = possibleRotations[Random.Range(0, possibleRotations.Length)];
            Quaternion randomYRotation = Quaternion.Euler(0, randomRotation, 0);  // ֻΧ�� Y ����ת

            // ʵ�����µĵ���Ԥ���岢�洢
            if (terrainPrefab != null)
            {
                instantiatedPrefab = Instantiate(terrainPrefab, terrainPosition, randomYRotation, transform);  // ʹ�������ת�͸߶�
                instantiatedPrefab.name = terrainPrefab.name;  // ȥ�� (Clone) ��׺
                instantiatedPrefab.transform.localPosition = new Vector3(0, randomHeight, 0);  // ȷ��λ����ȷ
            }
        }
        else
        {
            Debug.LogWarning("����Ԥ����ȱ�� TerrainHeightRange ���");
        }
    }

    // ���ø�������
    public void SetHighlight(GameObject highlightObj)
    {
        highlight = highlightObj;
    }

    // �������
    public void ClearHighlight()
    {
        if (highlight != null)
        {
            Destroy(highlight);
        }
    }

    // ����·��ָʾ����
    public void SetPathIndicator(GameObject pathObj)
    {
        pathIndicator = pathObj;
    }
    public void SetTurnIndicator(GameObject turnObj)
    {
        turnIndicator = turnObj;
    }

    // ���·��ָʾ
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

    // ��Ӷ��󵽵�Ԫ��
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

    // �ڱ༭������ʾ HexCell ����
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.color = Color.white;
        Gizmos.DrawIcon(transform.position, "HexIcon.png", true);  // ��ѡ����ʾ����
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMovement : MonoBehaviour
{
    private Queue<HexCell> pathQueue;
    public float moveSpeed = 2f;  // �����ڵ�Ԫ��֮���ƶ��Ķ����ٶ�
    private GameObject teamModel;  // С�ӵ�ģ������

    // �����ƶ��߼�
    public void StartMove(GameObject teamModel, HexCell start, HexCell goal, HexGrid hexGrid, Pathfinder pathfinder, float speed)
    {
        this.teamModel = teamModel;  // ��ȡ������С��ģ��
        List<HexCell> calculatedPath = pathfinder.FindPath(start, goal, hexGrid, speed);
        if (calculatedPath != null && calculatedPath.Count > 0)
        {
            pathQueue = new Queue<HexCell>(calculatedPath);
            StartCoroutine(MoveAlongPath()); // �����ƶ�����
        }
        else
        {
            Debug.LogWarning("δ�ҵ�·����");
        }
    }

    // Э�̣�����·���ƶ�
    private IEnumerator MoveAlongPath()
    {
        while (pathQueue.Count > 0)
        {
            HexCell nextCell = pathQueue.Dequeue();

            // ��ȡ HexCell ���Ӷ���CustomTerrain��
            CustomTerrain targetTerrain = nextCell.GetComponentInChildren<CustomTerrain>();
            if (targetTerrain != null)
            {
                Vector3 targetPosition = targetTerrain.transform.position + new Vector3(0, 0.5f, 0);  // �����Ӷ�������Ϸ�

                // ����С��ģ���ƶ���Ŀ����ε����Ϸ�
                while (Vector3.Distance(teamModel.transform.position, targetPosition) > 0.1f)
                {
                    teamModel.transform.position = Vector3.MoveTowards(teamModel.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                // �ƶ�����Ԫ���ͣ��һ���
                yield return new WaitForSeconds(0.2f);
            }
        }

        Debug.Log("�ƶ���ɣ�");
    }
}

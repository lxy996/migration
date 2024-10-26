using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    // FindPath �����ҵ��� start �� goal ��·��
    public List<HexCell> FindPath(HexCell start, HexCell goal, HexGrid hexGrid, float speed)
    {
        List<HexCell> openList = new List<HexCell>();
        HashSet<HexCell> closedList = new HashSet<HexCell>();
        Dictionary<HexCell, HexCell> cameFrom = new Dictionary<HexCell, HexCell>();
        Dictionary<HexCell, int> gCost = new Dictionary<HexCell, int>();
        Dictionary<HexCell, int> fCost = new Dictionary<HexCell, int>();

        // ��ʼ��·��
        List<HexCell> path = new List<HexCell>();

        openList.Add(start);
        gCost[start] = 0;
        fCost[start] = Heuristic(start, goal);

        float remainingMoveSpeed = speed;

        while (openList.Count > 0)
        {
            HexCell current = GetCellWithLowestFCost(openList, fCost);

            // ���ʣ���ƶ�������򵽴�Ŀ��
            if (remainingMoveSpeed <= 0 || current == goal)
            {
                path = ReconstructPath(cameFrom, current); // �ؽ�·��
                break; // ֹͣ����
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (HexCell neighbor in hexGrid.GetNeighbors(current))
            {
                if (closedList.Contains(neighbor))
                {
                    continue;
                }

                int tentativeGCost = gCost[current] + neighbor.GetPassCost();

                // ���ʣ���ƶ��������Խ����ھӵ�Ԫ��������
                if (remainingMoveSpeed < neighbor.GetPassCost())
                {
                    continue;
                }

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
                else if (tentativeGCost >= gCost[neighbor])
                {
                    continue; // ���·���������
                }

                cameFrom[neighbor] = current;
                gCost[neighbor] = tentativeGCost;
                fCost[neighbor] = gCost[neighbor] + Heuristic(neighbor, goal);
            }

            // ����ʣ���ƶ���
            remainingMoveSpeed -= current.GetPassCost();
        }

        return path; // ����·��
    }

    // ��������ʽ���룬����A*�㷨
    private int Heuristic(HexCell a, HexCell b)
    {
        return Mathf.Abs(a.coordinates.x - b.coordinates.x) + Mathf.Abs(a.coordinates.y - b.coordinates.y);
    }

    // ��ȡ�������fCost�ĵ�Ԫ��
    private HexCell GetCellWithLowestFCost(List<HexCell> openList, Dictionary<HexCell, int> fCost)
    {
        HexCell lowest = openList[0];
        foreach (HexCell cell in openList)
        {
            if (fCost[cell] < fCost[lowest])
            {
                lowest = cell;
            }
        }
        return lowest;
    }

    // �ؽ�·������Ŀ�굥Ԫ��׷�ݵ����
    private List<HexCell> ReconstructPath(Dictionary<HexCell, HexCell> cameFrom, HexCell current)
    {
        List<HexCell> path = new List<HexCell>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse(); // ��·����תΪ����㵽Ŀ��
        return path;
    }

    // �����ܻغ���������·����С�ӵ��ٶ�
    public int CalculateTurns(List<HexCell> path, int speed)
    {
        int totalCost = 0;
        foreach (HexCell cell in path)
        {
            totalCost += cell.GetPassCost();
        }
        return Mathf.CeilToInt((float)totalCost / speed);
    }
}
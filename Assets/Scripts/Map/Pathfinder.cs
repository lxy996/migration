using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    // FindPath 方法找到从 start 到 goal 的路径
    public List<HexCell> FindPath(HexCell start, HexCell goal, HexGrid hexGrid, float speed)
    {
        List<HexCell> openList = new List<HexCell>();
        HashSet<HexCell> closedList = new HashSet<HexCell>();
        Dictionary<HexCell, HexCell> cameFrom = new Dictionary<HexCell, HexCell>();
        Dictionary<HexCell, int> gCost = new Dictionary<HexCell, int>();
        Dictionary<HexCell, int> fCost = new Dictionary<HexCell, int>();

        // 初始化路径
        List<HexCell> path = new List<HexCell>();

        openList.Add(start);
        gCost[start] = 0;
        fCost[start] = Heuristic(start, goal);

        float remainingMoveSpeed = speed;

        while (openList.Count > 0)
        {
            HexCell current = GetCellWithLowestFCost(openList, fCost);

            // 如果剩余移动力不足或到达目标
            if (remainingMoveSpeed <= 0 || current == goal)
            {
                path = ReconstructPath(cameFrom, current); // 重建路径
                break; // 停止搜索
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

                // 如果剩余移动力不足以进入邻居单元格，跳过它
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
                    continue; // 如果路径更差，跳过
                }

                cameFrom[neighbor] = current;
                gCost[neighbor] = tentativeGCost;
                fCost[neighbor] = gCost[neighbor] + Heuristic(neighbor, goal);
            }

            // 减少剩余移动力
            remainingMoveSpeed -= current.GetPassCost();
        }

        return path; // 返回路径
    }

    // 计算启发式距离，用于A*算法
    private int Heuristic(HexCell a, HexCell b)
    {
        return Mathf.Abs(a.coordinates.x - b.coordinates.x) + Mathf.Abs(a.coordinates.y - b.coordinates.y);
    }

    // 获取具有最低fCost的单元格
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

    // 重建路径，从目标单元格追溯到起点
    private List<HexCell> ReconstructPath(Dictionary<HexCell, HexCell> cameFrom, HexCell current)
    {
        List<HexCell> path = new List<HexCell>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse(); // 将路径反转为从起点到目标
        return path;
    }

    // 计算总回合数，给定路径和小队的速度
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
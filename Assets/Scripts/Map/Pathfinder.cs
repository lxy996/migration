using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public class Node
    {
        public CustomTerrain terrain;
        public Node parent;
        public float gCost; // ����㵽��ǰ���ӵ��ƶ�����
        public float hCost; // ����ʽ���ۣ���ǰ���ӵ�Ŀ����ӵĹ��ƴ��ۣ�
        public float FCost => gCost + hCost; // �ܴ���

        public Node(CustomTerrain terrain)
        {
            this.terrain = terrain;
        }
    }

    public List<CustomTerrain> FindPath(CustomTerrain start, CustomTerrain target, float maxSpeed, out float totalCost)
    {
        List<Node> openSet = new List<Node>();
        HashSet<CustomTerrain> closedSet = new HashSet<CustomTerrain>();
        Node startNode = new Node(start);
        Node targetNode = new Node(target);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode.terrain);

            if (currentNode.terrain == targetNode.terrain)
            {
                totalCost = currentNode.gCost;
                return RetracePath(startNode, currentNode);
            }

            foreach (CustomTerrain neighbor in currentNode.terrain.neighbors)
            {
                if (closedSet.Contains(neighbor) || !neighbor.isPassable)
                {
                    continue;
                }

                float newMovementCostToNeighbor = currentNode.gCost + neighbor.passCost;
                Node neighborNode = new Node(neighbor)
                {
                    parent = currentNode,
                    gCost = newMovementCostToNeighbor,
                    hCost = GetDistance(neighbor, targetNode.terrain)
                };

                if (!openSet.Exists(n => n.terrain == neighbor && n.FCost <= neighborNode.FCost))
                {
                    openSet.Add(neighborNode);
                }
            }
        }

        totalCost = float.MaxValue; // ��ʾ�޷�����
        return null; // û���ҵ�·��
    }

    private List<CustomTerrain> RetracePath(Node startNode, Node endNode)
    {
        List<CustomTerrain> path = new List<CustomTerrain>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.terrain);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private float GetDistance(CustomTerrain a, CustomTerrain b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }
}
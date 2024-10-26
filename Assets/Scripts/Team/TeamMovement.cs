using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMovement : MonoBehaviour
{
    private Queue<HexCell> pathQueue;
    public float moveSpeed = 2f;  // 控制在单元格之间移动的动画速度
    private GameObject teamModel;  // 小队的模型引用

    // 启动移动逻辑
    public void StartMove(GameObject teamModel, HexCell start, HexCell goal, HexGrid hexGrid, Pathfinder pathfinder, float speed)
    {
        this.teamModel = teamModel;  // 获取并保存小队模型
        List<HexCell> calculatedPath = pathfinder.FindPath(start, goal, hexGrid, speed);
        if (calculatedPath != null && calculatedPath.Count > 0)
        {
            pathQueue = new Queue<HexCell>(calculatedPath);
            StartCoroutine(MoveAlongPath()); // 启动移动过程
        }
        else
        {
            Debug.LogWarning("未找到路径！");
        }
    }

    // 协程：逐步沿路径移动
    private IEnumerator MoveAlongPath()
    {
        while (pathQueue.Count > 0)
        {
            HexCell nextCell = pathQueue.Dequeue();

            // 获取 HexCell 的子对象（CustomTerrain）
            CustomTerrain targetTerrain = nextCell.GetComponentInChildren<CustomTerrain>();
            if (targetTerrain != null)
            {
                Vector3 targetPosition = targetTerrain.transform.position + new Vector3(0, 0.5f, 0);  // 地形子对象的正上方

                // 控制小队模型移动到目标地形的正上方
                while (Vector3.Distance(teamModel.transform.position, targetPosition) > 0.1f)
                {
                    teamModel.transform.position = Vector3.MoveTowards(teamModel.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                // 移动到单元格后停顿一会儿
                yield return new WaitForSeconds(0.2f);
            }
        }

        Debug.Log("移动完成！");
    }
}

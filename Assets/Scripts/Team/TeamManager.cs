using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public Team team;
    public CustomTerrain currentTile;
    public HexGrid hexGrid;
    public TeamMovement teamMovement;  // 引用动物小队的移动脚本
    public Pathfinder pathfinder; // 路径寻路器
    public GameObject teamModel;  // 小队的模型

    public int speed;  // 单位移动力
    public bool isSelected = false; // 小队是否被选中
    private bool hasMoved = false; // 当前回合是否已经移动

    private HexCell startCell = null;   // 起始点
    private HexCell targetCell = null;  // 目标点

    private List<CustomTerrain> currentPath = new List<CustomTerrain>(); // 当前路径

    void Start()
    {
        // 初始化小队
        team = new Team("Zebra", 5);
        speed = team.speed;

        // 获取小队初始所在的地块
        currentTile = GetTileAtPosition(transform.position);

        if (currentTile != null)
        {
            Debug.Log("小队初始位置: " + currentTile.name);
        }

        teamMovement = FindObjectOfType<TeamMovement>();  // 找到场景中的 TeamMovement 脚本

    }

    // 选中小队
    public void Select()
    {
        isSelected = true;
        UIManager.Instance.ShowTeamInfo(team);
        
        // 单位被选中后，计算可移动范围
        currentTile = GetTileAtPosition(transform.position);
        startCell = currentTile.GetComponentInParent<HexCell>();
        List<HexCell> movableCells = hexGrid.GetMovableCells(startCell, speed);
        hexGrid.HighlightMovableCells(movableCells);
        
        Debug.Log("队伍已选中");
    }

    // 取消小队选中
    public void Deselect()
    {
        isSelected = false;
        targetCell = null;
        UIManager.Instance.HideTeamInfo();
        // 当单位取消选中时，清除高亮
        hexGrid.ClearHighlights();
        Debug.Log("队伍已取消选中");
    }

    // 当选中一个单元格时
    public void OnCellSelected(HexCell cell)
    {
        if (targetCell == null)
        {
            // 选中目标单元格
            targetCell = cell;
            Debug.Log("设定目标单元格: " + cell.coordinates);
        }
        else if (targetCell == cell)
        {
            // 如果再次点击相同单元格，执行移动
            ExecuteMove();
        }
        else
        {
            // 重新设定目标
            targetCell = cell;
            Debug.Log("更改目标单元格: " + cell.coordinates);
        }
    }

    public void OnRightClick(HexCell targetCell)
    {
        currentTile = GetTileAtPosition(transform.position);
        startCell = currentTile.GetComponentInParent<HexCell>();
        List<HexCell> path = pathfinder.FindPath(startCell, targetCell, hexGrid, speed);

        int turns = pathfinder.CalculateTurns(path, speed);

        // 调用 HexGrid 显示路径，并在路径终点显示回合数
        hexGrid.ShowPath(path, turns);
    }

    // 执行移动操作
    void ExecuteMove()
    {
        currentTile = GetTileAtPosition(transform.position);
        startCell = currentTile.GetComponentInParent<HexCell>();

        if (currentTile != null && targetCell != null)
        {
            teamMovement.StartMove(teamModel, startCell, targetCell, GameManager.Instance.hexGrid, GameManager.Instance.pathfinder, speed);
            Debug.Log("移动开始");
        }
    }

    CustomTerrain GetTileAtPosition(Vector3 position)
    {
        // 定义一个只检测地形的 LayerMask
        LayerMask terrainLayerMask = LayerMask.GetMask("TerrainLayer"); 

        Ray ray = new Ray(position + Vector3.down * 10f, Vector3.up);
        // 使用 LayerMask 忽略其他层的 Collider
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, terrainLayerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            return hit.collider.GetComponent<CustomTerrain>();
        }
        return null;
    }

    public void ResetMove()
    {
        hasMoved = false;  // 重置小队的移动状态
    }

    public bool HasMoved()
    {
        return hasMoved;
    }

    public void UpdateMemberStates()
    {
        team.UpdateMemberStates(-2, -2); // 每次移动减少饱腹度和口渴度
    }

    public void CollectResources()
    {
        team.CollectResources();
    }

    public void ResolveEvents()
    {
        team.ResolveEvents();
    }

    public void ShowTileInfo(CustomTerrain tile)
    {
        Debug.Log("显示地块信息：" + tile.name);
        // 在 UI 中显示地块信息
        UIManager.Instance.ShowTileInfo(tile);
    }
}
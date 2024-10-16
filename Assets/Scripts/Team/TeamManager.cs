using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public Team team;
    public CustomTerrain currentTile;
    public Pathfinder pathfinder; // 路径寻路器
    public float maxMovementSpeed = 3f; // 小队最大移动力
    public bool isSelected = false; // 小队是否被选中
    private bool hasMoved = false; // 当前回合是否已经移动
    private List<CustomTerrain> currentPath = new List<CustomTerrain>(); // 当前路径
    private CustomTerrain selectedTile = null; // 记录当前选中的地块

    void Start()
    {
        // 初始化小队
        team = new Team("Zebra", 5);
        // 获取小队初始所在的地块
        currentTile = GetTileAtPosition(transform.position);

        if (currentTile != null)
        {
            Debug.Log("小队初始位置: " + currentTile.name);
        }
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CustomTerrain clickedTile = hit.collider.GetComponent<CustomTerrain>();
                TeamManager clickedTeam = hit.collider.GetComponent<TeamManager>();

                if (Input.GetMouseButtonDown(0)) // 左键点击
                {
                    if (clickedTeam != null && clickedTeam == this)
                    {
                        Select();
                    }
                    else if (isSelected && clickedTile != null)
                    {
                        if (selectedTile == null)
                        {
                            selectedTile = clickedTile;
                            //ShowPathTo(clickedTile);
                        }
                        else if (selectedTile == clickedTile)
                        {
                            StartCoroutine(MoveAlongPath(currentPath));
                            selectedTile = null;
                        }
                    }
                    else if (clickedTile != null)
                    {
                        ShowTileInfo(clickedTile);
                    }
                }
                else if (Input.GetMouseButtonDown(1)) // 右键点击
                {
                    Deselect();
                }
            }
        }
    }

    // 选中小队
    public void Select()
    {
        isSelected = true;
        GameManager.Instance.SelectTeam(this);  // 通知 GameManager 更新 UI
        Debug.Log("小队已选中");
        // 显示小队的 UI 信息
        UIManager.Instance.ShowTeamInfo(team);
    }

    // 取消小队选中
    public void Deselect()
    {
        if (isSelected)  // 只有在选中状态下才执行取消选中操作
        {
            isSelected = false;
            selectedTile = null;
            ClearPreviousHighlights();
            Debug.Log("小队已取消选中");

            // 可能还需要通知 GameManager，更新其他 UI 或状态
            GameManager.Instance.DeselectTeam();  // 确保不会再递归调用 Deselect()
        }
    }

    // 显示路径并计算消耗
    /*public void ShowPathTo(CustomTerrain targetTile)
    {
        float totalCost;
        ClearPreviousHighlights();

        // 调用A*算法，获取路径的坐标列表
        List<CubeCoordinate> pathCoordinates = pathfinder.AStarCubeNavigaction(currentTile.cubeCoordinate, targetTile.cubeCoordinate, out totalCost);

        // 将坐标列表转换为 CustomTerrain 列表
        currentPath = new List<CustomTerrain>();
        foreach (var coordinate in pathCoordinates)
        {
            CustomTerrain tile = HexGrid.Instance.GetTileFromCoordinate(coordinate);
            if (tile != null)
            {
                currentPath.Add(tile);
            }
        }

        // 检查路径是否有效以及移动力是否足够
        if (currentPath != null && totalCost <= maxMovementSpeed)
        {
            HighlightPath(currentPath);
            Debug.Log("路径找到，总消耗: " + totalCost);
            Debug.Log("预计移动回合数: " + Mathf.Ceil(totalCost / team.speed) + " 回合");

            // 显示路径信息
            UIManager.Instance.ShowPathInfo(totalCost, Mathf.Ceil(totalCost / team.speed));
        }
        else
        {
            Debug.Log("目标不可达或超出移动力范围");
        }
    }*/


    void HighlightPath(List<CustomTerrain> path)
    {
        foreach (var tile in path)
        {
            tile.Highlight();
        }
    }

    void ClearPreviousHighlights()
    {
        // 如果路径已经高亮，取消所有高亮
        if (currentPath != null)
        {
            foreach (var tile in currentPath)
            {
                if (tile != null)
                {
                    tile.RemoveHighlight();
                }
            }
            currentPath.Clear();
        }
    }

    IEnumerator MoveAlongPath(List<CustomTerrain> path)
    {
        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("路径为空，无法移动");
            yield break;
        }

        hasMoved = true;
        foreach (var tile in path)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = tile.transform.position;
            float elapsedTime = 0f;
            float moveTime = 1f;

            while (elapsedTime < moveTime)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition;
            currentTile = tile;
        }

        // 更新资源和事件
        CollectResources();
        ResolveEvents();

        // 重置移动状态
        hasMoved = false;
    }

    CustomTerrain GetTileAtPosition(Vector3 position)
    {
        Ray ray = new Ray(position + Vector3.down * 10f, Vector3.up);
        if (Physics.Raycast(ray, out RaycastHit hit))
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
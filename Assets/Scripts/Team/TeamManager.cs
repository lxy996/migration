using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public Team team;
    public CustomTerrain currentTile;
    public HexGrid hexGrid;
    public TeamMovement teamMovement;  // ���ö���С�ӵ��ƶ��ű�
    public Pathfinder pathfinder; // ·��Ѱ·��
    public GameObject teamModel;  // С�ӵ�ģ��

    public int speed;  // ��λ�ƶ���
    public bool isSelected = false; // С���Ƿ�ѡ��
    private bool hasMoved = false; // ��ǰ�غ��Ƿ��Ѿ��ƶ�

    private HexCell startCell = null;   // ��ʼ��
    private HexCell targetCell = null;  // Ŀ���

    private List<CustomTerrain> currentPath = new List<CustomTerrain>(); // ��ǰ·��

    void Start()
    {
        // ��ʼ��С��
        team = new Team("Zebra", 5);
        speed = team.speed;

        // ��ȡС�ӳ�ʼ���ڵĵؿ�
        currentTile = GetTileAtPosition(transform.position);

        if (currentTile != null)
        {
            Debug.Log("С�ӳ�ʼλ��: " + currentTile.name);
        }

        teamMovement = FindObjectOfType<TeamMovement>();  // �ҵ������е� TeamMovement �ű�

    }

    // ѡ��С��
    public void Select()
    {
        isSelected = true;
        UIManager.Instance.ShowTeamInfo(team);
        
        // ��λ��ѡ�к󣬼�����ƶ���Χ
        currentTile = GetTileAtPosition(transform.position);
        startCell = currentTile.GetComponentInParent<HexCell>();
        List<HexCell> movableCells = hexGrid.GetMovableCells(startCell, speed);
        hexGrid.HighlightMovableCells(movableCells);
        
        Debug.Log("������ѡ��");
    }

    // ȡ��С��ѡ��
    public void Deselect()
    {
        isSelected = false;
        targetCell = null;
        UIManager.Instance.HideTeamInfo();
        // ����λȡ��ѡ��ʱ���������
        hexGrid.ClearHighlights();
        Debug.Log("������ȡ��ѡ��");
    }

    // ��ѡ��һ����Ԫ��ʱ
    public void OnCellSelected(HexCell cell)
    {
        if (targetCell == null)
        {
            // ѡ��Ŀ�굥Ԫ��
            targetCell = cell;
            Debug.Log("�趨Ŀ�굥Ԫ��: " + cell.coordinates);
        }
        else if (targetCell == cell)
        {
            // ����ٴε����ͬ��Ԫ��ִ���ƶ�
            ExecuteMove();
        }
        else
        {
            // �����趨Ŀ��
            targetCell = cell;
            Debug.Log("����Ŀ�굥Ԫ��: " + cell.coordinates);
        }
    }

    public void OnRightClick(HexCell targetCell)
    {
        currentTile = GetTileAtPosition(transform.position);
        startCell = currentTile.GetComponentInParent<HexCell>();
        List<HexCell> path = pathfinder.FindPath(startCell, targetCell, hexGrid, speed);

        int turns = pathfinder.CalculateTurns(path, speed);

        // ���� HexGrid ��ʾ·��������·���յ���ʾ�غ���
        hexGrid.ShowPath(path, turns);
    }

    // ִ���ƶ�����
    void ExecuteMove()
    {
        currentTile = GetTileAtPosition(transform.position);
        startCell = currentTile.GetComponentInParent<HexCell>();

        if (currentTile != null && targetCell != null)
        {
            teamMovement.StartMove(teamModel, startCell, targetCell, GameManager.Instance.hexGrid, GameManager.Instance.pathfinder, speed);
            Debug.Log("�ƶ���ʼ");
        }
    }

    CustomTerrain GetTileAtPosition(Vector3 position)
    {
        // ����һ��ֻ�����ε� LayerMask
        LayerMask terrainLayerMask = LayerMask.GetMask("TerrainLayer"); 

        Ray ray = new Ray(position + Vector3.down * 10f, Vector3.up);
        // ʹ�� LayerMask ����������� Collider
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, terrainLayerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            return hit.collider.GetComponent<CustomTerrain>();
        }
        return null;
    }

    public void ResetMove()
    {
        hasMoved = false;  // ����С�ӵ��ƶ�״̬
    }

    public bool HasMoved()
    {
        return hasMoved;
    }

    public void UpdateMemberStates()
    {
        team.UpdateMemberStates(-2, -2); // ÿ���ƶ����ٱ����ȺͿڿʶ�
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
        Debug.Log("��ʾ�ؿ���Ϣ��" + tile.name);
        // �� UI ����ʾ�ؿ���Ϣ
        UIManager.Instance.ShowTileInfo(tile);
    }
}
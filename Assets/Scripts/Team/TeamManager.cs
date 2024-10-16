using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public Team team;
    public CustomTerrain currentTile;
    public Pathfinder pathfinder; // ·��Ѱ·��
    public float maxMovementSpeed = 3f; // С������ƶ���
    public bool isSelected = false; // С���Ƿ�ѡ��
    private bool hasMoved = false; // ��ǰ�غ��Ƿ��Ѿ��ƶ�
    private List<CustomTerrain> currentPath = new List<CustomTerrain>(); // ��ǰ·��
    private CustomTerrain selectedTile = null; // ��¼��ǰѡ�еĵؿ�

    void Start()
    {
        // ��ʼ��С��
        team = new Team("Zebra", 5);
        // ��ȡС�ӳ�ʼ���ڵĵؿ�
        currentTile = GetTileAtPosition(transform.position);

        if (currentTile != null)
        {
            Debug.Log("С�ӳ�ʼλ��: " + currentTile.name);
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

                if (Input.GetMouseButtonDown(0)) // ������
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
                else if (Input.GetMouseButtonDown(1)) // �Ҽ����
                {
                    Deselect();
                }
            }
        }
    }

    // ѡ��С��
    public void Select()
    {
        isSelected = true;
        GameManager.Instance.SelectTeam(this);  // ֪ͨ GameManager ���� UI
        Debug.Log("С����ѡ��");
        // ��ʾС�ӵ� UI ��Ϣ
        UIManager.Instance.ShowTeamInfo(team);
    }

    // ȡ��С��ѡ��
    public void Deselect()
    {
        if (isSelected)  // ֻ����ѡ��״̬�²�ִ��ȡ��ѡ�в���
        {
            isSelected = false;
            selectedTile = null;
            ClearPreviousHighlights();
            Debug.Log("С����ȡ��ѡ��");

            // ���ܻ���Ҫ֪ͨ GameManager���������� UI ��״̬
            GameManager.Instance.DeselectTeam();  // ȷ�������ٵݹ���� Deselect()
        }
    }

    // ��ʾ·������������
    /*public void ShowPathTo(CustomTerrain targetTile)
    {
        float totalCost;
        ClearPreviousHighlights();

        // ����A*�㷨����ȡ·���������б�
        List<CubeCoordinate> pathCoordinates = pathfinder.AStarCubeNavigaction(currentTile.cubeCoordinate, targetTile.cubeCoordinate, out totalCost);

        // �������б�ת��Ϊ CustomTerrain �б�
        currentPath = new List<CustomTerrain>();
        foreach (var coordinate in pathCoordinates)
        {
            CustomTerrain tile = HexGrid.Instance.GetTileFromCoordinate(coordinate);
            if (tile != null)
            {
                currentPath.Add(tile);
            }
        }

        // ���·���Ƿ���Ч�Լ��ƶ����Ƿ��㹻
        if (currentPath != null && totalCost <= maxMovementSpeed)
        {
            HighlightPath(currentPath);
            Debug.Log("·���ҵ���������: " + totalCost);
            Debug.Log("Ԥ���ƶ��غ���: " + Mathf.Ceil(totalCost / team.speed) + " �غ�");

            // ��ʾ·����Ϣ
            UIManager.Instance.ShowPathInfo(totalCost, Mathf.Ceil(totalCost / team.speed));
        }
        else
        {
            Debug.Log("Ŀ�겻�ɴ�򳬳��ƶ�����Χ");
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
        // ���·���Ѿ�������ȡ�����и���
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
            Debug.LogWarning("·��Ϊ�գ��޷��ƶ�");
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

        // ������Դ���¼�
        CollectResources();
        ResolveEvents();

        // �����ƶ�״̬
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
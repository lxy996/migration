using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        StartScreen,
        InGame,
        LevelEnd,
        Victory,
        Restart
    }

    public GameState currentState;
    public Team team;
    public List<TeamManager> teams = new List<TeamManager>();  // ����С��
    public TeamManager selectedTeam;  // ��ǰѡ�е�С��
    public HexCell selectedCell;      // ��ǰѡ�еĵ�Ԫ��
    public HexGrid hexGrid;           // ���õ�ǰ�� HexGrid
    public TeamMovement teamMovement;  // ���ö���С�ӵ��ƶ��ű�
    public Pathfinder pathfinder; // ����·�������㷨

    private HexCell startCell;         // ����С�ӵ���㵥Ԫ��
    private HexCell goalCell;          // ����С�ӵ�Ŀ�굥Ԫ��

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))  // ������������
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // ����˶���
                TeamManager clickedTeam = hit.collider.GetComponent<TeamManager>();
                if (clickedTeam != null)
                {
                    if (selectedTeam != clickedTeam)
                    {
                        SelectTeam(clickedTeam);  // �л�����
                    }
                }
                else
                {
                    // ����˵�Ԫ��
                    HexCell clickedCell = hit.collider.GetComponentInParent<HexCell>();
                    if (clickedCell != null)
                    {
                        SelectCell(clickedCell);  // ѡ�е�Ԫ��
                    }
                }
            }
        }
        // �������Ҽ������ȡ��ѡ��
        if (Input.GetMouseButtonDown(1))  // �Ҽ�ȡ��ѡ��
        {
            if (selectedTeam != null)
            {
                DeselectTeam();  // ����ȡ��ѡ�е��߼�
            }
            else if (selectedCell != null)
            {
                selectedCell = null;  // �����ǰѡ�еĵ�Ԫ��
                Debug.Log("ȡ��ѡ�е�Ԫ��");
            }
        }
    }

    public void SelectTeam(TeamManager team)
    {
        if (selectedTeam != null)
        {
            selectedTeam.Deselect();  // ���֮ǰ��ѡ�е�С�ӣ�ȡ����ѡ��״̬
        }

        selectedTeam = team;
        selectedTeam.Select();  // ֪ͨ�µ�ѡ��С��
    }

    // ѡ�е�Ԫ�񲢸����ƶ�Ŀ��
    public void SelectCell(HexCell cell)
    {
        selectedCell = cell;

        if (selectedTeam != null && selectedTeam.isSelected)
        {
            selectedTeam.OnCellSelected(cell);
        }
    }
    public void DeselectTeam()
    {
        if (selectedTeam != null)
        {
            selectedTeam.Deselect();
            selectedTeam = null;
        }
    }

    public void SaveTeamState()
    {
        // ����С�ӵ���ϸ״̬
        PlayerPrefs.SetInt("TeamMemberCount", team.members.Count);
        for (int i = 0; i < team.members.Count; i++)
        {
            PlayerPrefs.SetInt($"Member{i}_Hunger", team.members[i].hunger);
            PlayerPrefs.SetInt($"Member{i}_Thirst", team.members[i].thirst);
            PlayerPrefs.SetString($"Member{i}_Buffs", string.Join(",", team.members[i].buffs));
            PlayerPrefs.SetString($"Member{i}_Debuffs", string.Join(",", team.members[i].debuffs));
        }
        PlayerPrefs.SetInt("TeamSpeed", team.speed);
        PlayerPrefs.SetInt("TeamStamina", team.stamina);
        PlayerPrefs.SetInt("TeamDefense", team.defense);
        PlayerPrefs.SetInt("TeamTempSpeed", team.tempSpeed);
    }

    public void LoadTeamState()
    {
        // ����С�ӵ���ϸ״̬
        int memberCount = PlayerPrefs.GetInt("TeamMemberCount", 0);
        team.members.Clear();
        for (int i = 0; i < memberCount; i++)
        {
            // ��Ϊ AnimalMember �Ĺ��캯����Ҫ foodType ����������������Ҫ�Ľ�
            FoodType foodType = (FoodType)System.Enum.Parse(typeof(FoodType), PlayerPrefs.GetString($"Member{i}_FoodType", "LongGrass"));
            AnimalMember member = new AnimalMember(foodType);
            member.hunger = PlayerPrefs.GetInt($"Member{i}_Hunger", 0);
            member.thirst = PlayerPrefs.GetInt($"Member{i}_Thirst", 0);
            member.buffs = new List<string>(PlayerPrefs.GetString($"Member{i}_Buffs", "").Split(','));
            member.debuffs = new List<string>(PlayerPrefs.GetString($"Member{i}_Debuffs", "").Split(','));
            team.members.Add(member);
        }
        team.speed = PlayerPrefs.GetInt("TeamSpeed", 0);
        team.stamina = PlayerPrefs.GetInt("TeamStamina", 0);
        team.defense = PlayerPrefs.GetInt("TeamDefense", 0);
        team.tempSpeed = PlayerPrefs.GetInt("TeamTempSpeed", 0);
    }

    void ResetGameState()
    {
        // ����������Ϸ״̬
        PlayerPrefs.DeleteAll();
    }
}
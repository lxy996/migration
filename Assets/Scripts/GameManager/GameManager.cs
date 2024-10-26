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
    public List<TeamManager> teams = new List<TeamManager>();  // 所有小队
    public TeamManager selectedTeam;  // 当前选中的小队
    public HexCell selectedCell;      // 当前选中的单元格
    public HexGrid hexGrid;           // 引用当前的 HexGrid
    public TeamMovement teamMovement;  // 引用动物小队的移动脚本
    public Pathfinder pathfinder; // 引用路径查找算法

    private HexCell startCell;         // 动物小队的起点单元格
    private HexCell goalCell;          // 动物小队的目标单元格

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
        if (Input.GetMouseButtonDown(0))  // 检测鼠标左键点击
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 点击了队伍
                TeamManager clickedTeam = hit.collider.GetComponent<TeamManager>();
                if (clickedTeam != null)
                {
                    if (selectedTeam != clickedTeam)
                    {
                        SelectTeam(clickedTeam);  // 切换队伍
                    }
                }
                else
                {
                    // 点击了单元格
                    HexCell clickedCell = hit.collider.GetComponentInParent<HexCell>();
                    if (clickedCell != null)
                    {
                        SelectCell(clickedCell);  // 选中单元格
                    }
                }
            }
        }
        // 检测鼠标右键点击以取消选中
        if (Input.GetMouseButtonDown(1))  // 右键取消选中
        {
            if (selectedTeam != null)
            {
                DeselectTeam();  // 调用取消选中的逻辑
            }
            else if (selectedCell != null)
            {
                selectedCell = null;  // 清除当前选中的单元格
                Debug.Log("取消选中单元格");
            }
        }
    }

    public void SelectTeam(TeamManager team)
    {
        if (selectedTeam != null)
        {
            selectedTeam.Deselect();  // 如果之前有选中的小队，取消其选中状态
        }

        selectedTeam = team;
        selectedTeam.Select();  // 通知新的选中小队
    }

    // 选中单元格并更新移动目标
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
        // 保存小队的详细状态
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
        // 加载小队的详细状态
        int memberCount = PlayerPrefs.GetInt("TeamMemberCount", 0);
        team.members.Clear();
        for (int i = 0; i < memberCount; i++)
        {
            // 因为 AnimalMember 的构造函数需要 foodType 参数，所以这里需要改进
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
        // 重置所有游戏状态
        PlayerPrefs.DeleteAll();
    }
}
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
    public CustomTerrain selectedTile;  // 当前选中的地块

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

    void Start()
    {
        ChangeState(GameState.StartScreen);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.StartScreen:
                LoadStartScreen();
                break;
            case GameState.InGame:
                LoadInGameScene();
                break;
            case GameState.LevelEnd:
                ShowLevelEndScreen();
                break;
            case GameState.Victory:
                ShowVictoryScreen();
                break;
            case GameState.Restart:
                RestartGame();
                break;
        }
    }

    void LoadStartScreen()
    {
        SceneManager.LoadScene("StartScreen");
    }

    void LoadInGameScene()
    {
        SceneManager.LoadScene("GameScene");
        StartNewTurn();  // 开始游戏后重置小队状态
    }

    void ShowLevelEndScreen()
    {
        SaveTeamState();
        SceneManager.LoadScene("LevelEndScreen");
    }

    void ShowVictoryScreen()
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    void RestartGame()
    {
        ResetGameState();
        ChangeState(GameState.StartScreen);
    }

    public void StartNewTurn()
    {
        foreach (var team in teams)
        {
            team.ResetMove();  // 重置所有小队的移动状态
        }
        selectedTeam = null;  // 重置选择的队伍
        selectedTile = null;  // 重置选中的地块
    }

    public bool CanSelectTeam(TeamManager team)
    {
        // 确保一次只能选中一个队伍
        return selectedTeam == null || team == selectedTeam;
    }

    public void SelectTeam(TeamManager team)
    {
        if (selectedTeam != null)
        {
            selectedTeam.Deselect();  // 取消当前选中
        }
        selectedTeam = team;
        selectedTeam.Select();
    }

    public void DeselectTeam()
    {
        if (selectedTeam != null)
        {
            selectedTeam.Deselect();  // 调用小队的 Deselect 方法
            selectedTeam = null;  // 取消选择
        }
    }

    public void SelectTile(CustomTerrain tile)
    {
        if (selectedTile != null)
        {
            selectedTile.RemoveHighlight();  // 取消当前选中的地块高亮
        }
        selectedTile = tile;
        selectedTile.Highlight();  // 高亮新选中的地块

        // 更新 UI 显示地块信息
        UIManager.Instance.ShowTileInfo(tile);
    }

    public void DeselectTile()
    {
        if (selectedTile != null)
        {
            selectedTile.RemoveHighlight();  // 取消高亮
            selectedTile = null;
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
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        StartScreen,
        InGame,
        LevelEnd,
        Victory,
        Restart
    }

    public GameState currentState;
    public Team team; // �������ǵ�ǰ��Ϸ�е�С����

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
        LoadTeamState();
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
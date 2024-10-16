using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text foodText;
    public Text waterText;
    public Text staminaText;
    public Text pathInfoText;
    public Text tileInfoText;

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

    // 更新小队的食物、饮水和耐力信息
    public void ShowTeamInfo(Team team)
    {
        foreach (AnimalMember member in team.members)
        {
            foodText.text = "Food: " + member.hunger;
            waterText.text = "Water: " + member.thirst;
            staminaText.text = "Stamina: " + team.stamina;
        }
    }
    // 显示路径信息，包括消耗和预计回合数
    public void ShowPathInfo(float totalCost, float estimatedTurns)
    {
        pathInfoText.text = "Path Cost: " + totalCost + "\nEstimated Turns: " + estimatedTurns;
    }

    // 显示地块信息
    public void ShowTileInfo(CustomTerrain tile)
    {
        tileInfoText.text = "Tile: " + tile.name + "\nTerrain Type: " + tile.terrainType;
    }
}

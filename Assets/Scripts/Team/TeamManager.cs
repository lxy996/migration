using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public Team team;
    public CustomTerrain currentTile;

    void Start()
    {
        // 初始化小队，假设每个成员都吃草，成员数量5，初始饱腹度和口渴度为10，速度为3
        team = new Team("Zebra", 5);
    }

    public void UpdateResources(int foodChange, int waterChange)
    {
        team.UpdateMemberStates(foodChange, waterChange);
    }

    public void MoveTo(CustomTerrain targetTile)
    {
        if (targetTile.isPassable)
        {
            currentTile = targetTile;

            // 更新小队成员的饱腹度和口渴值
            UpdateMemberStates();
            ResolveEvents();
            CollectResources();
        }
    }

    public void UpdateMemberStates()
    {
        team.UpdateMemberStates(-2, -2); // 每次移动后减少饱腹度和口渴度
    }

    public void CollectResources()
    {
        team.CollectResources();
    }

    public void ResolveEvents()
    {
        team.ResolveEvents();
    }

    public void ApplyBuffToTeam(string buff)
    {
        team.ApplyBuff(buff);
    }

    public void ApplyDebuffToTeam(string debuff)
    {
        team.ApplyDebuff(debuff);
    }
}

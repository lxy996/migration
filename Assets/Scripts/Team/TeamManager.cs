using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public Team team;
    public CustomTerrain currentTile;

    void Start()
    {
        // ��ʼ��С�ӣ�����ÿ����Ա���Բݣ���Ա����5����ʼ�����ȺͿڿʶ�Ϊ10���ٶ�Ϊ3
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

            // ����С�ӳ�Ա�ı����ȺͿڿ�ֵ
            UpdateMemberStates();
            ResolveEvents();
            CollectResources();
        }
    }

    public void UpdateMemberStates()
    {
        team.UpdateMemberStates(-2, -2); // ÿ���ƶ�����ٱ����ȺͿڿʶ�
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

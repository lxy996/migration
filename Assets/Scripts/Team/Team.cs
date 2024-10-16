using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public List<AnimalMember> members;
    public CustomTerrain currentTile;
    public int speed;
    public int stamina;
    public int defense;
    public int tempSpeed; // 临时速度

    public Team(string animalType, int numberOfMembers)
    {
        members = new List<AnimalMember>();
        FoodType foodType;
        switch (animalType)
        {
            case "Zebra":
                foodType = FoodType.LongGrass;
                speed = 3;
                stamina = 3;
                defense = 4;
                break;
            case "Gnu":
                foodType = FoodType.BaseGrass;
                speed = 3;
                stamina = 4;
                defense = 3;
                break;
            case "Gazelle":
                foodType = FoodType.TenderGrass;
                speed = 4;
                stamina = 2;
                defense = 3;
                break;
            default:
                foodType = FoodType.LongGrass;
                speed = 3;
                stamina = 3;
                defense = 3;
                break;
        }

        for (int i = 0; i < numberOfMembers; i++)
        {
            members.Add(new AnimalMember(foodType));
        }
    }

    // 收集地块资源
    public void CollectResources()
    {
        int totalFood = 0;
        foreach (AnimalMember member in members)
        {
            switch (member.foodType)
            {
                case FoodType.LongGrass:
                    totalFood += currentTile.resources.longGrass;
                    currentTile.resources.longGrass = 0;
                    break;
                case FoodType.BaseGrass:
                    totalFood += currentTile.resources.baseGrass;
                    currentTile.resources.baseGrass = 0;
                    break;
                case FoodType.TenderGrass:
                    totalFood += currentTile.resources.tenderGrass;
                    currentTile.resources.tenderGrass = 0;
                    break;
            }
        }

        int totalWater = currentTile.resources.water;
        int distance = CalculateDistanceFromMainGroup();
        float resourceMultiplier = distance == 1 ? 0.5f : 1.0f;
        totalFood = Mathf.RoundToInt(totalFood * resourceMultiplier);
        totalWater = Mathf.RoundToInt(totalWater * resourceMultiplier);

        DistributeResources(totalFood, ResourceType.Food);
        DistributeResources(totalWater, ResourceType.Water);
    }

    // 分配资源
    private void DistributeResources(int totalAmount, ResourceType type)
    {
        foreach (AnimalMember member in members)
        {
            if (type == ResourceType.Food)
            {
                int neededFood = 10 - member.hunger;
                int givenFood = Mathf.Min(totalAmount, neededFood);
                member.UpdateHunger(givenFood);
                totalAmount -= givenFood;
            }
            else if (type == ResourceType.Water)
            {
                int neededWater = 10 - member.thirst;
                int givenWater = Mathf.Min(totalAmount, neededWater);
                member.UpdateThirst(givenWater);
                totalAmount -= givenWater;
            }

            if (totalAmount <= 0) break;
        }
    }

    // 更新小队成员状态
    public void UpdateMemberStates(int hungerChange, int thirstChange)
    {
        foreach (AnimalMember member in members)
        {
            member.UpdateHunger(hungerChange);
            member.UpdateThirst(thirstChange);
        }
    }

    // 触发事件
    public void ResolveEvents()
    {
        int distance = CalculateDistanceFromMainGroup();
        int diceRoll = Random.Range(1, 21);

        (int rewardThreshold, int neutralThreshold) = GetEventThresholds(distance);

        if (diceRoll <= rewardThreshold)
        {
            TriggerRewardEvent();
        }
        else if (diceRoll <= rewardThreshold + neutralThreshold)
        {
            TriggerNeutralEvent();
        }
        else
        {
            TriggerDangerEvent();
        }
    }

    private (int, int) GetEventThresholds(int distance)
    {
        switch (distance)
        {
            case 1:
                return (5, 11);
            case 2:
                return (7, 7);
            case 3:
                return (9, 4);
            default:
                return (9, 2);
        }
    }

    private void TriggerRewardEvent()
    {
        // 处理奖励事件的逻辑
    }

    private void TriggerNeutralEvent()
    {
        // 处理中立事件的逻辑
    }

    private void TriggerDangerEvent()
    {
        // 处理危险事件的逻辑
    }

    private int CalculateDistanceFromMainGroup()
    {
        // 计算与大部队的距离
        return 2; // 示例返回值
    }

    public int CalculateTurnsToReachTarget(float totalCost)
    {
        return Mathf.CeilToInt(totalCost / speed);
    }

    public void ApplyBuff(string buff)
    {
        // 应用buff的逻辑
    }

    public void ApplyDebuff(string debuff)
    {
        // 应用debuff的逻辑
    }

    private enum ResourceType
    {
        Food,
        Water
    }
}
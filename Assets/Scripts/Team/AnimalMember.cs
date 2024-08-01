using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    LongGrass,
    BaseGrass,
    TenderGrass
}

public class AnimalMember
{
    public int hunger;
    public int thirst;
    public FoodType foodType;
    public List<string> buffs; // 添加 buffs 属性
    public List<string> debuffs; // 添加 debuffs 属性

    public AnimalMember(FoodType foodType)
    {
        this.foodType = foodType;
        hunger = 10; // 初始饱腹度
        thirst = 10; // 初始口渴度
        buffs = new List<string>(); // 初始化 buffs 列表
        debuffs = new List<string>(); // 初始化 debuffs 列表
    }

    public void UpdateHunger(int amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0, 10);
    }

    public void UpdateThirst(int amount)
    {
        thirst = Mathf.Clamp(thirst + amount, 0, 10);
    }
}

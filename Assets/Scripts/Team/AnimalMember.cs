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
    public List<string> buffs; // ��� buffs ����
    public List<string> debuffs; // ��� debuffs ����

    public AnimalMember(FoodType foodType)
    {
        this.foodType = foodType;
        hunger = 10; // ��ʼ������
        thirst = 10; // ��ʼ�ڿʶ�
        buffs = new List<string>(); // ��ʼ�� buffs �б�
        debuffs = new List<string>(); // ��ʼ�� debuffs �б�
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

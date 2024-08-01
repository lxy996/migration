using UnityEngine;

public enum TerrainType
{
    Grass,
    Plains,
    Hills,
    Swamps,
    BiggerRiver,
    River,
    Lakes,
    Mountains
}

public class TerrainResource
{
    public int longGrass; // ����
    public int baseGrass; // �ײ��
    public int tenderGrass; // �۲�
    public int water; // ˮԴ
    public bool isWaterInfinite; // ˮ�Ƿ�����

    public TerrainResource(int longGrass, int baseGrass, int tenderGrass, int water, bool isWaterInfinite = false)
    {
        this.longGrass = longGrass;
        this.baseGrass = baseGrass;
        this.tenderGrass = tenderGrass;
        this.water = water;
        this.isWaterInfinite = isWaterInfinite;
    }
}

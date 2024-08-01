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
    public int longGrass; // 长草
    public int baseGrass; // 底层草
    public int tenderGrass; // 嫩草
    public int water; // 水源
    public bool isWaterInfinite; // 水是否无限

    public TerrainResource(int longGrass, int baseGrass, int tenderGrass, int water, bool isWaterInfinite = false)
    {
        this.longGrass = longGrass;
        this.baseGrass = baseGrass;
        this.tenderGrass = tenderGrass;
        this.water = water;
        this.isWaterInfinite = isWaterInfinite;
    }
}

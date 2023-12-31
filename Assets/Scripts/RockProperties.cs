using Unity.Entities;
using Unity.Mathematics;

public struct RockSpawnerComponent : IComponentData
{
    public float2 MaxFieldDimensions;
    public float2 MinFieldDimensions;

    public float RockSpawnRate;
    public int RockSpawnStartAmount;
    public int RockSpawnAmount;

    public Entity RockPrefab;

    public float2 RockMovementSpeedRange;

    public float Timer;
}

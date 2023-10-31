using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class RockSpawnerMono : MonoBehaviour
{
    public float2 MinFieldDimensions;
    public float2 MaxFieldDimensions;
    public float RockSpawnRate;
    public float2 RockMovementSpeedRange;
    public uint RandomSeed;
    public GameObject RockPrefab;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(MinFieldDimensions.x, MinFieldDimensions.y, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(MaxFieldDimensions.x, MaxFieldDimensions.y, 0));
    }
}

public class RockSpawnerBaker : Baker<RockSpawnerMono>
{
    public override void Bake(RockSpawnerMono authoring)
    {
        var rockSpawnerEntity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(rockSpawnerEntity, new RockSpawnerComponent
        {
            MaxFieldDimensions = authoring.MaxFieldDimensions,
            MinFieldDimensions = authoring.MinFieldDimensions,
            RockSpawnRate = authoring.RockSpawnRate,
            RockPrefab = GetEntity(authoring.RockPrefab, TransformUsageFlags.Dynamic),
            Timer = 0,
            RockMovementSpeedRange = authoring.RockMovementSpeedRange,
        });

        AddComponent(rockSpawnerEntity, new RandomComponent { RandomValue = Random.CreateFromIndex(authoring.RandomSeed) });
    }
}

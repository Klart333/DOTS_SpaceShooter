using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct RockSpawnerAspect : IAspect
{
    public readonly Entity Entity;

    //private readonly RefRO<Transform> transform;

    private readonly RefRW<RockSpawnerComponent> rockSpawnerProperties;
    private readonly RefRW<RandomComponent> randomComponent;

    public Entity RockPrefab => rockSpawnerProperties.ValueRO.RockPrefab;

    public float RockSpawnRate => 1.0f / rockSpawnerProperties.ValueRO.RockSpawnRate;

    private float2 MinField => rockSpawnerProperties.ValueRO.MinFieldDimensions;
    private float2 MaxField => rockSpawnerProperties.ValueRO.MaxFieldDimensions;

    public bool ShouldSpawn(float deltaTime)
    {
        rockSpawnerProperties.ValueRW.Timer += deltaTime;

        if (rockSpawnerProperties.ValueRO.Timer > RockSpawnRate)
        {
            rockSpawnerProperties.ValueRW.Timer = 0;
            return true;
        }

        return false;
    }

    public LocalTransform GetRandomTransform()
    {
        return new LocalTransform
        {
            Position = GetRandomPosition(),
            Rotation = quaternion.identity,
            Scale = GetRandomScale(0.6f, 2.5f)
        };
    }

    private float GetRandomScale(float min, float max)
    {
        return randomComponent.ValueRW.RandomValue.NextFloat(min, max);
    }

    private float3 GetRandomPosition()
    {
        int side = randomComponent.ValueRW.RandomValue.NextInt(0, 4);

        switch (side)
        {
            case 0:

                float2 minCorner = new float2(-MinField.x / 2.0f, -MaxField.y / 2.0f);
                float2 maxCorner = new float2(MinField.x / 2.0f, -MinField.y / 2.0f);
                return new float3(randomComponent.ValueRW.RandomValue.NextFloat2(minCorner, maxCorner), 0);

            case 1:
                float2 minCorner1 = new float2(-MinField.x / 2.0f, MinField.y / 2.0f);
                float2 maxCorner1 = new float2(MinField.x / 2.0f, MaxField.y / 2.0f);
                return new float3(randomComponent.ValueRW.RandomValue.NextFloat2(minCorner1, maxCorner1), 0);

            case 2:
                float2 minCorner2 = new float2(-MaxField.x / 2.0f, -MaxField.y / 2.0f);
                float2 maxCorner2 = new float2(-MinField.x / 2.0f, MaxField.y / 2.0f);
                return new float3(randomComponent.ValueRW.RandomValue.NextFloat2(minCorner2, maxCorner2), 0);

            case 3:
                float2 minCorner3 = new float2(MinField.x / 2.0f, -MaxField.y / 2.0f);
                float2 maxCorner3 = new float2(MaxField.x / 2.0f, MaxField.y / 2.0f);
                return new float3(randomComponent.ValueRW.RandomValue.NextFloat2(minCorner3, maxCorner3), 0);

            default:
                return new float3();
        }

    }

    public float3 GetDirectionToCentre(float3 position)
    {
        return math.rotate(quaternion.AxisAngle(math.forward(), randomComponent.ValueRW.RandomValue.NextFloat(-0.5f, 0.5f)), math.normalize(-position));
    }

    public float GetMovementSpeed(float scale)
    {
        return randomComponent.ValueRO.RandomValue.NextFloat(rockSpawnerProperties.ValueRO.RockMovementSpeedRange.x, rockSpawnerProperties.ValueRO.RockMovementSpeedRange.y) * (1.0f / scale);
    }
}

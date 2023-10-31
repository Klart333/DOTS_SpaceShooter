using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnRockSystem : ISystem
{
    public static int RockAmount = 0;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RockSpawnerComponent>();
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Count Rock amount
        int count = 0;
        foreach (var kvp in SystemAPI.Query<RockTag>())
        {
            count++;
        }
        RockAmount = count;


        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        new SpawnRockJob
        {
            DeltaTime = Time.deltaTime,
            ecb = ecb.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct SpawnRockJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer.ParallelWriter ecb;

    [BurstCompile]
    private void Execute(RockSpawnerAspect rockSpawnerAspect, [EntityIndexInQuery] int sortKey)
    {
        int amountToSpawn = rockSpawnerAspect.GetSpawnAmount();
        if (amountToSpawn > 10 || rockSpawnerAspect.ShouldSpawn(DeltaTime))
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                var rock = ecb.Instantiate(sortKey, rockSpawnerAspect.RockPrefab);

                var transform = rockSpawnerAspect.GetRandomTransform();
                ecb.SetComponent(sortKey, rock, transform);

                ecb.AddComponent(sortKey, rock, new WorldMovementTag());
                ecb.AddComponent(sortKey, rock, new DistanceDeathTag());
                ecb.AddComponent(sortKey, rock, new RockTag());

                ecb.AddComponent(sortKey, rock, new VelocityMovementComponent()
                {
                    Direction = rockSpawnerAspect.GetDirectionToCentre(transform.Position),
                    MovementSpeed = rockSpawnerAspect.GetMovementSpeed(transform.Scale),
                });
            }
        }
    }
}

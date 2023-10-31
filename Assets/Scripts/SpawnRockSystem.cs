using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnRockSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RockSpawnerComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        new SpawnRockJob
        {
            DeltaTime = Time.deltaTime,
            ecb = ecb.CreateCommandBuffer(state.WorldUnmanaged),
        }.Schedule();
    }
}

[BurstCompile]
public partial struct SpawnRockJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ecb;

    [BurstCompile]
    private void Execute(RockSpawnerAspect rockSpawnerAspect)
    {
        if (rockSpawnerAspect.ShouldSpawn(DeltaTime))
        {
            var rock = ecb.Instantiate(rockSpawnerAspect.RockPrefab);

            var transform = rockSpawnerAspect.GetRandomTransform();
            ecb.SetComponent(rock, transform);

            ecb.AddComponent(rock, new WorldMovementTag());
            ecb.AddComponent(rock, new DistanceDeathTag());
            ecb.AddComponent(rock, new VelocityMovementComponent()
            {
                Direction = rockSpawnerAspect.GetDirectionToCentre(transform.Position),
                MovementSpeed = rockSpawnerAspect.GetMovementSpeed(transform.Scale),
            });
        }
    }
}

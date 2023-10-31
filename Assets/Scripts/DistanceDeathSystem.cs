using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct DistanceDeathSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        new DistanceDeathJob
        {
            ecb = ecb.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct DistanceDeathJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;

    [BurstCompile]
    private void Execute(DistanceDeathAspect deathAspect, [EntityIndexInQuery] int sortKey)
    {
        if (deathAspect.ShouldDie())
        {
            ecb.DestroyEntity(sortKey, deathAspect.Entity);
        }
    }
}
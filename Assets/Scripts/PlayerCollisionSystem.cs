using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[BurstCompile]
public partial struct PlayerCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var notEcb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        var ecb = notEcb.CreateCommandBuffer(state.WorldUnmanaged);

        new PlayerCollisionJob
        {
            ecb = ecb.AsParallelWriter(),
        }.ScheduleParallel();

    }
}

[BurstCompile]
public partial struct PlayerCollisionJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;

    [BurstCompile]
    private void Execute(RockAspect rockAspect, [EntityIndexInQuery] int sortKey)
    {
        float radius = rockAspect.RockScale; // I'll ignore the players size, its faster :)
        if (math.abs(rockAspect.LocalTransform.Position.x) < radius && math.abs(rockAspect.LocalTransform.Position.y) < radius)
        {
            ecb.AddComponent(sortKey, rockAspect.Entity, new HitPlayerTag());
        }
    }
}

public struct HitPlayerTag : IComponentData
{

}

[BurstCompile]
public partial struct HitPlayerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var notEcb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        var ecb = notEcb.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var item in SystemAPI.Query<HitPlayerTag>().WithEntityAccess())
        {
            ecb.RemoveComponent(item.Item2, typeof(HitPlayerTag));

            foreach (var playerHopefully in SystemAPI.Query<InputShootingComponent>().WithEntityAccess()) // Not great as now we can only have one inputShooter, also very ugly, but idc
            {
                ecb.AddComponent(playerHopefully.Item2, new HurtTag() { Value = new float4(1, 0, 0, 1) });
            }
        }

    }
}

public struct HurtTag : IComponentData
{
    public float4 Value;
}

public partial struct DisplayHurtSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var notEcb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        var ecb = notEcb.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var item in SystemAPI.Query<HurtTag>().WithEntityAccess())
        {
            float4 color = item.Item1.Value + new float4(0, 0.5f, 0.5f, 0) * Time.deltaTime;
            color = math.clamp(color, 0, 1);
            ecb.AddComponent(item.Item2, new URPMaterialPropertyBaseColor { Value = color });
            ecb.SetComponent(item.Item2, new HurtTag() { Value = color });
        }

    }
}

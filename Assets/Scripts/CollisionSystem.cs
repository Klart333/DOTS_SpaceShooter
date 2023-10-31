using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct CollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var notEcb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        var ecb = notEcb.CreateCommandBuffer(state.WorldUnmanaged);

        int count = 0;
        NativeArray<float3> projectilePositions = new NativeArray<float3>(512, Allocator.TempJob); // Hopefully there's no more than 256 projectiles... Could not find a better solution
        foreach (var projectile in SystemAPI.Query<VelocityMovementAspect>().WithAll<ProjectileTag>())
        {
            projectilePositions[count++] = projectile.LocalTransform.Position;
            if (count == 256)
            {
                break;
            }
        }

        new ProjectileCollisionJob
        {
            ecb = ecb.AsParallelWriter(),
            projectilePositions = projectilePositions,
        }.ScheduleParallel();

    }
}

[BurstCompile]
public partial struct ProjectileCollisionJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;

    [ReadOnly, DeallocateOnJobCompletion]
    public NativeArray<float3> projectilePositions;

    [BurstCompile]
    private void Execute(RockAspect rockAspect, [EntityIndexInQuery] int sortKey)
    {
        for (int i = 0; i < projectilePositions.Length; i++)
        {
            if (projectilePositions[i].Equals(float3.zero))
            {
                break;
            }

            float3 diff = rockAspect.LocalTransform.Position - projectilePositions[i];
            float sqrMagnitude = diff.x * diff.x + diff.y * diff.y;

            float radius = rockAspect.RockScale + 0.075f; // Half the projectiles radius 
            if (sqrMagnitude < radius * radius)
            {
                ecb.DestroyEntity(sortKey, rockAspect.Entity);
            }
        }
    }
}

public readonly partial struct RockAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<RockTag> _ref;

    private readonly RefRO<LocalTransform> localTransform;

    public LocalTransform LocalTransform => localTransform.ValueRO;

    public float RockScale => localTransform.ValueRO.Scale / 2.0f;
}
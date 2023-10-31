using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct InputShootingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InputShootingComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (!InputManager.Instance.GetFire) return;

        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        var direction = math.normalize(InputManager.Instance.CurrentDirection);

        new InputShootingJob
        {
            direction = new float3(direction, 0),
            ecb = ecb.CreateCommandBuffer(state.WorldUnmanaged),
        }.Schedule();
    }
}


[BurstCompile]
public partial struct InputShootingJob : IJobEntity
{
    public EntityCommandBuffer ecb;
    public float3 direction;

    [BurstCompile]
    private void Execute(InputShootingAspect shootingAspect)
    {
        var projectile = ecb.Instantiate(shootingAspect.ProjectilePrefab);

        var transform = new LocalTransform
        {
            Position = direction / 2.0f,
            Rotation = quaternion.identity,
            Scale = shootingAspect.Scale,
        };

        ecb.SetComponent(projectile, transform);

        ecb.AddComponent(projectile, new WorldMovementTag());
        ecb.AddComponent(projectile, new DistanceDeathTag());
        ecb.AddComponent(projectile, new ProjectileTag());

        ecb.AddComponent(projectile, new VelocityMovementComponent()
        {
            Direction = direction,
            MovementSpeed = shootingAspect.ProjectileSpeed,
        });
    }
}

using Unity.Entities;
using Unity.Transforms;

public struct InputShootingComponent : IComponentData
{
    public Entity ProjectilePrefab;
    public float ProjectileSpeed;
    public float Scale;
}

public readonly partial struct InputShootingAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<InputShootingComponent> inputShootingComponent;

    public Entity ProjectilePrefab => inputShootingComponent.ValueRO.ProjectilePrefab;
    public float ProjectileSpeed => inputShootingComponent.ValueRO.ProjectileSpeed;
    public float Scale => inputShootingComponent.ValueRO.Scale;
}

public struct ProjectileTag : IComponentData
{

}

public struct RockTag : IComponentData
{

}
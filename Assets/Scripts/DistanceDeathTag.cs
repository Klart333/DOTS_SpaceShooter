using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public struct DistanceDeathTag : IComponentData
{

}

public readonly partial struct DistanceDeathAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<DistanceDeathTag> rockMovement;

    private readonly RefRW<LocalTransform> localTransform;

    public bool ShouldDie()
    {
        return localTransform.ValueRO.Position.x * localTransform.ValueRO.Position.x + localTransform.ValueRO.Position.y * localTransform.ValueRO.Position.y > 300;
    }
}

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct VelocityMovementComponent : IComponentData
{
    public float MovementSpeed;
    public float3 Direction;
}

public readonly partial struct VelocityMovementAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<VelocityMovementComponent> rockMovement;

    private readonly RefRW<LocalTransform> localTransform;

    public void Move(float deltaTime)
    {
        localTransform.ValueRW.Position += rockMovement.ValueRO.Direction * rockMovement.ValueRO.MovementSpeed * deltaTime;
    }
}

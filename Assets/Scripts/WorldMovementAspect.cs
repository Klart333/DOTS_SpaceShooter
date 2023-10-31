using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct WorldMovementAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<WorldMovementTag> _ref;

    private readonly RefRW<LocalTransform> localTransform;

    public void Move(float3 movement)
    {
        localTransform.ValueRW.Position += movement;
    }
}

using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(SpawnRockSystem))]
public partial struct VelocityMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = Time.deltaTime;
        new VelocityMovementJob
        {
            deltaTime = deltaTime,
        }.ScheduleParallel();
    }
}


[BurstCompile]
public partial struct VelocityMovementJob : IJobEntity
{
    public float deltaTime;

    [BurstCompile]
    private void Execute(VelocityMovementAspect rockAspect)
    {
        rockAspect.Move(deltaTime);
    }
}

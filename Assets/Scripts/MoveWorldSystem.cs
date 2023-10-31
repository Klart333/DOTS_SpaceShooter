using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public partial struct MoveWorldSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RockSpawnerComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        Vector2 input = InputManager.Instance.GetMovement;
        if (input.sqrMagnitude < 0.1f) return;

        float3 movement = new float3(input, 0) * Time.deltaTime;

        new MoveWorldJob
        {
            movement = movement,
        }.ScheduleParallel();
    }
}


[BurstCompile]
public partial struct MoveWorldJob : IJobEntity
{
    public float3 movement;

    [BurstCompile]
    private void Execute(WorldMovementAspect worldMovementAspect)
    {
        worldMovementAspect.Move(-movement);
    }
}

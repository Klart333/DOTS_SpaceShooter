using Unity.Entities;
using UnityEngine;

public class InputShootingMono : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;
    public float Scale = 0.15f;
}

public class InputShootingBaker : Baker<InputShootingMono>
{
    public override void Bake(InputShootingMono authoring)
    {
        var shooter = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(shooter, new InputShootingComponent
        {
            ProjectileSpeed = authoring.ProjectileSpeed,
            ProjectilePrefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
            Scale = authoring.Scale
        });

        //AddComponent(shooter, new RandomComponent { RandomValue = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed) });
    }
}

using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(TransformSystemGroup))]
[UpdateAfter(typeof(LocalToWorldSystem))]
partial class SphereMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var box = SystemAPI.GetSingletonEntity<Box>();
        var boxLocalToWorld = SystemAPI.GetComponent<LocalToWorld>(box);

        Entities.WithAll<Sphere>().ForEach((Entity entity, TransformAspect transform, in Sphere sphere) =>
        {
            var dir = boxLocalToWorld.Position - transform.WorldPosition;
            dir = math.normalize(dir);
            dir.y = 0f;//transform.WorldPosition.y;
            transform.WorldPosition += dir * sphere.Speed * SystemAPI.Time.DeltaTime;
        }).ScheduleParallel();
    }
}

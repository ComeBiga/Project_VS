using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Collections;
using Collider = Unity.Physics.Collider;

[BurstCompile]
[UpdateInGroup(typeof(TransformSystemGroup))]
partial struct BoxMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var h = UnityEngine.Input.GetAxis("Horizontal");
        var v = UnityEngine.Input.GetAxis("Vertical");

        var deltaTime = SystemAPI.Time.DeltaTime;
        var forward = new float3(0, 0, 1f);
        var right = new float3(1f, 0, 0);

        var dir = forward * v + right * h;

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach(var (transform, box) in SystemAPI.Query<TransformAspect, Box>().WithAll<Box>())
        {
            transform.WorldPosition += dir * box.Speed * deltaTime;
            transform.WorldRotation = quaternion.LookRotationSafe(dir, new float3(0, 1f, 0));

            var entity = SphereCast(transform.WorldPosition, transform.WorldPosition, .49f);

            if(entity != Entity.Null)
            {
                if(SystemAPI.HasComponent<Sphere>(entity))
                    ecb.DestroyEntity(entity);
            }

            // ColliderCastHit hit = new ColliderCastHit();
            // if(physicsCollider.Value.Value.BoxCast(transform.WorldPosition, quaternion.identity, 100f, new float3(0, 0, 1f), .5f, out hit, new CollisionFilter()))
            // {
            //     if(SystemAPI.HasComponent<Sphere>(hit.Entity))
            //         ecb.DestroyEntity(hit.Entity);
            // }
        }
    }

    public unsafe Entity SphereCast(float3 RayFrom, float3 RayTo, float radius)
    {
        EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();

        EntityQuery singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);

        var collisionWorld = singletonQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

        singletonQuery.Dispose();

        var filter = new CollisionFilter()
        {
            BelongsTo = ~0u,
            CollidesWith = ~0u,
            GroupIndex = 0
        };

        //SphereGeometry sphereGeometry = new SphereGeometry() { Center = float3.zero, Radius = radius };
        //BlobAssetReference<Collider> sphereCollider = SphereCollider.Create(sphereGeometry, filter);

        BoxGeometry boxGeometry = new BoxGeometry() { Center = float3.zero, Size = .4f , Orientation = quaternion.identity, BevelRadius = .05f};
        BlobAssetReference<Collider> boxCollider = BoxCollider.Create(boxGeometry, filter);

        ColliderCastInput input = new ColliderCastInput()
        {
            Collider = (Collider*)boxCollider.GetUnsafePtr(),
            Orientation = quaternion.identity,
            Start = RayFrom,
            End = RayTo
        };

        ColliderCastHit hit = new ColliderCastHit();
        bool haveHit = collisionWorld.CastCollider(input, out hit);
        if(haveHit)
        {
            return hit.Entity;
        }

        //sphereCollider.Dispose();
        boxCollider.Dispose();

        return Entity.Null;
    }
}



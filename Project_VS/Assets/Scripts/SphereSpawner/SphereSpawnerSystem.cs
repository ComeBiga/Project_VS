using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct SphereSpawnerSystem : ISystem
{
    ComponentLookup<WorldTransform> m_worldTransformLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        m_worldTransformLookup = state.GetComponentLookup<WorldTransform>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var spawner = SystemAPI.GetSingleton<SphereSpawner>();

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var vehicles = CollectionHelper.CreateNativeArray<Entity>(spawner.SphereCount, Allocator.Temp);
        ecb.Instantiate(spawner.SpherePrefab, vehicles);

        m_worldTransformLookup.Update(ref state);

        //var transformLookup = state.GetComponentLookup<WorldTransform>();
        var spawnerEntity = SystemAPI.GetSingletonEntity<SphereSpawner>();
        var spawnerTransform = m_worldTransformLookup[spawnerEntity];
        // var sphereTransform = LocalTransform.FromPosition(spawnerTransform.Position);


        foreach(var vehicle in vehicles)
        {
            var randomPosition = Random.CreateFromIndex((uint)vehicle.Index%100).NextFloat3() * 20f;
            randomPosition.y = .5f;
            var sphereTransform = LocalTransform.FromPosition(randomPosition);
            sphereTransform.Scale = m_worldTransformLookup[spawner.SpherePrefab].Scale;

            ecb.SetComponent(vehicle, sphereTransform);
        }

        state.Enabled = false;
    }
}

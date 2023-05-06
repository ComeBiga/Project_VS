using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

//[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(TransformSystemGroup))]
[UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
partial class MainEntityCameraSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if(MainGameObjectCamera.instance != null && SystemAPI.HasSingleton<MainEntityCamera>())
        {
            Entity mainEntityCameraEntity = SystemAPI.GetSingletonEntity<MainEntityCamera>();
            LocalToWorld targetLocalToWorld = SystemAPI.GetComponent<LocalToWorld>(mainEntityCameraEntity);
            MainEntityCamera mainEntityCamera = SystemAPI.GetComponent<MainEntityCamera>(mainEntityCameraEntity);

            float3 targetPosition = targetLocalToWorld.Position;
            float3 offset = new float3(mainEntityCamera.offsetX, mainEntityCamera.offsetY, mainEntityCamera.offsetZ);

            //UnityEngine.Vector3 mainGameObjectCameraPosition = MainGameObjectCamera.instance.transform.position;
            // float3 forward = targetPosition - new float3(mainGameObjectCameraPosition.x, mainGameObjectCameraPosition.y, mainGameObjectCameraPosition.z);
            float3 forward = -offset;
            quaternion rotation = quaternion.LookRotationSafe(forward, new float3(0, 1f, 0));

            MainGameObjectCamera.instance.transform.SetPositionAndRotation(targetPosition + offset, rotation);
            // MainGameObjectCamera.instance.transform.SetPositionAndRotation(targetPosition + offset, rotation);
        }
    }
}

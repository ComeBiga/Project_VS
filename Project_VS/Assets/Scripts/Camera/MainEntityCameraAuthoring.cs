using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class MainEntityCameraAuthoring : MonoBehaviour
{
    public Vector3 offset;

    class MainEntityCameraBaker : Baker<MainEntityCameraAuthoring>
    {
        public override void Bake(MainEntityCameraAuthoring authoring)
        {
            AddComponent<MainEntityCamera>(new MainEntityCamera() 
            {
                offsetX = authoring.offset.x,
                offsetY = authoring.offset.y,
                offsetZ = authoring.offset.z
            });
        }
    }
}

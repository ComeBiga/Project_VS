using Unity.Entities;

[System.Serializable]
public struct MainEntityCamera : IComponentData
{
    public float offsetX;
    public float offsetY;
    public float offsetZ;
}

using Unity.Entities;

public struct SphereSpawner : IComponentData
{
    public Entity SpherePrefab;
    public int SphereCount;
}

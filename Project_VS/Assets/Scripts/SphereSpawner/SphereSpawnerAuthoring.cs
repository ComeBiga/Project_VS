using Unity.Entities;
using UnityEngine;

public class SphereSpawnerAuthoring : MonoBehaviour
{
    public GameObject spherePrefab;
    public int sphereCount;

    public class SphereSpawnerBaker : Baker<SphereSpawnerAuthoring>
    {
        public override void Bake(SphereSpawnerAuthoring authoring)
        {
            AddComponent<SphereSpawner>(new SphereSpawner() 
            { 
                SpherePrefab = GetEntity(authoring.spherePrefab),
                SphereCount = authoring.sphereCount
            });
        }
    }
}

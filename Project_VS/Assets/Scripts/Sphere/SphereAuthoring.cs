using Unity.Entities;
using UnityEngine;

public class SphereAuthoring : MonoBehaviour
{
    public float speed = 1f;

    public class SphereBaker : Baker<SphereAuthoring>
    {
        public override void Bake(SphereAuthoring authoring)
        {
            AddComponent<Sphere>(new Sphere() { Speed = authoring.speed });
        }
    }
}

public struct Sphere : IComponentData
{
    public float Speed;
}

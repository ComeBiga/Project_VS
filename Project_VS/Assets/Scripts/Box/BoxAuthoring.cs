using UnityEngine;
using Unity.Entities;

public class BoxAuthoring : MonoBehaviour
{
    public float speed = 3f;

    public class BoxBaker : Baker<BoxAuthoring>
    {
        public override void Bake(BoxAuthoring authoring)
        {
            AddComponent(new Box(){ Speed = authoring.speed });
        }
    }
}

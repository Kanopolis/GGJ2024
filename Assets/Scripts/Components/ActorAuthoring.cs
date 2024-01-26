using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ActorAuthoring : MonoBehaviour
{
    public class Baker : Baker<ActorAuthoring>
    {
        public override void Bake(ActorAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Actor());
        }
    }
}

public struct Actor : IComponentData
{

}
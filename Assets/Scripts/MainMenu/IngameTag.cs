using Unity.Entities;
using UnityEngine;

public class IngameTag : MonoBehaviour
{
    public class Baker : Baker<IngameTag>
    {
        public override void Bake(IngameTag _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new IngameTag_Data());
        }
    }
}


public struct IngameTag_Data : IComponentData
{

}
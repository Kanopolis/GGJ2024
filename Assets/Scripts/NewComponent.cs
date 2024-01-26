using Unity.Entities;
using UnityEngine;

public class NewComponent : MonoBehaviour
{
    public class Baker : Baker<NewComponent>
    {
        public override void Bake(NewComponent _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new NewComponent_Data());
        }
    }
}

public struct NewComponent_Data : IComponentData
{
    
}
using Unity.Entities;
using UnityEngine;

public class DialogSelectionTag : MonoBehaviour
{
    public class Baker : Baker<DialogSelectionTag>
    {
        public override void Bake(DialogSelectionTag _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new DialogSelectionTag_Data());
        }
    }
}

public struct DialogSelectionTag_Data : IComponentData
{

}
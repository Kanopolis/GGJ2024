using Unity.Entities;
using UnityEngine;

public class MainMenuTag : MonoBehaviour
{
    public class Baker : Baker<MainMenuTag>
    {
        public override void Bake(MainMenuTag _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MainMenuTag_Data());
        }
    }
}

public struct MainMenuTag_Data : IComponentData
{

}
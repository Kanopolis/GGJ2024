using Unity.Entities;
using UnityEngine;

public class PlayerBulletAuthoring : MonoBehaviour
{
    public class Baker : Baker<PlayerBulletAuthoring>
    {
        public override void Bake(PlayerBulletAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerBullet());
        }
    }
}

public struct PlayerBullet : IComponentData
{
    
}
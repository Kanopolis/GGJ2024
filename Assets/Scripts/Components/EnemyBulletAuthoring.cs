using Unity.Entities;
using UnityEngine;

public class EnemyBulletAuthoring : MonoBehaviour
{
    public class Baker : Baker<EnemyBulletAuthoring>
    {
        public override void Bake(EnemyBulletAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyBullet());
        }
    }
}

public struct EnemyBullet : IComponentData
{
    
}
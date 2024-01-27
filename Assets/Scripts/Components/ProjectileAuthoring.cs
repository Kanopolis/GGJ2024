using Unity.Entities;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
    [SerializeField]
    private float m_MaxLifeTime;

    public class Baker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Projectile()
            {
                MaxLifeTime = _authoring.m_MaxLifeTime
            });
        }
    }
}

public struct Projectile : IComponentData
{
    public float MaxLifeTime;
    public float LifeTime;
    public int Damage;
}
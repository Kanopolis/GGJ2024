using Unity.Entities;
using UnityEngine;

public class PlayerPeriodicShootAuthoring : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ProjectilePrefab;
    [SerializeField]
    private float m_ProjectileSpeed;
    [SerializeField]
    private float m_ProjectileLifetime;
    [SerializeField]
    private int m_ProjectileCount;
    [SerializeField]
    private int m_AttackDamage;
    [SerializeField]
    private float m_AttackCooldown;

    public class Baker : Baker<PlayerPeriodicShootAuthoring>
    {
        public override void Bake(PlayerPeriodicShootAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerPeriodicShoot()
            {
                ProjectileEntity = GetEntity(_authoring.m_ProjectilePrefab, TransformUsageFlags.Dynamic),
                ProjectileSpeed = _authoring.m_ProjectileSpeed,
                ProjectileLifeTime = _authoring.m_ProjectileLifetime,
                Damage = _authoring.m_AttackDamage,
                ProjectileCount = _authoring.m_ProjectileCount,
                MaxCooldown = _authoring.m_AttackCooldown
            });
        }
    }
}

public struct PlayerPeriodicShoot : IComponentData
{
    public Entity ProjectileEntity;
    public float ProjectileSpeed;
    public float ProjectileLifeTime;

    public float MaxCooldown;
    public float CurrentCooldown;

    public int ProjectileCount;
    public int Damage;
}
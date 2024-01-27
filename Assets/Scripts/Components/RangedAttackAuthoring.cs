using Unity.Entities;
using UnityEngine;

public class RangedAttackAuthoring : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ProjectilePrefab;
    [SerializeField]
    private float m_ProjectileSpeed;
    [SerializeField]
    private float m_ProjectileLifetime;
    [SerializeField]
    private int m_AttackDamage;
    [SerializeField]
    private float m_AttackRange;
    [SerializeField]
    private float m_AttackCooldown;

    public class Baker : Baker<RangedAttackAuthoring>
    {
        public override void Bake(RangedAttackAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RangedAttack()
            {
                ProjectileEntity = GetEntity(_authoring.m_ProjectilePrefab, TransformUsageFlags.Dynamic),
                ProjectileSpeed = _authoring.m_ProjectileSpeed,
                ProjectileLifeTime = _authoring.m_ProjectileLifetime,
                Damage = _authoring.m_AttackDamage,
                Range = _authoring.m_AttackRange,
                MaxCooldown = _authoring.m_AttackCooldown
            });
        }
    }
}

public struct RangedAttack : IComponentData
{
    public Entity ProjectileEntity;
    public float ProjectileSpeed;
    public float ProjectileLifeTime;

    public float MaxCooldown;
    public float CurrentCooldown;

    public int Damage;
    public float Range;
}
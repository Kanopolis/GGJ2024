using Unity.Entities;
using UnityEngine;

public class MeleeAttackAuthoring : MonoBehaviour
{
    [SerializeField]
    private int m_AttackDamage;
    [SerializeField]
    private float m_AttackRange;
    [SerializeField]
    private float m_AttackCooldown;

    public class Baker : Baker<MeleeAttackAuthoring>
    {
        public override void Bake(MeleeAttackAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MeleeAttack()
            {
                Damage = _authoring.m_AttackDamage,
                Range = _authoring.m_AttackRange,
                MaxCooldown = _authoring.m_AttackCooldown
            });
        }
    }
}

public struct MeleeAttack : IComponentData
{
    public float MaxCooldown;
    public float CurrentCooldown;

    public int Damage;
    public float Range;
}
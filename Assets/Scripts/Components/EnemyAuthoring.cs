using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    [SerializeField]
    private float m_PreferredDistanceToTarget;

    public class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Enemy()
            {
                PreferredDistanceToTarget = _authoring.m_PreferredDistanceToTarget
            });
        }
    }
}

public struct Enemy : IComponentData
{
    public float PreferredDistanceToTarget;
}
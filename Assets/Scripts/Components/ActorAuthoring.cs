using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ActorAuthoring : MonoBehaviour
{
    [SerializeField]
    private int m_MaxHP;

    public class Baker : Baker<ActorAuthoring>
    {
        public override void Bake(ActorAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Actor()
            {
                MaxHitPoints = _authoring.m_MaxHP,
                CurrentHitPoints = _authoring.m_MaxHP
            });
        }
    }
}

public struct Actor : IComponentData
{
    public int MaxHitPoints;
    public int CurrentHitPoints;
}
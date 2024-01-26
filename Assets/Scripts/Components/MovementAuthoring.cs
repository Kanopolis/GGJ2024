using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MovementAuthoring : MonoBehaviour
{
    [SerializeField]
    private float m_MoveSpeed;

    public class Baker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Movement()
            {
                MovementSpeed = _authoring.m_MoveSpeed
            });
        }
    }
}

public struct Movement : IComponentData
{
    public float3 MovementDirection;
    public float MovementSpeed;
}
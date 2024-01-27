using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MovementAuthoring : MonoBehaviour
{
    [SerializeField]
    private float m_MaxMoveSpeed;
    [SerializeField]
    private float m_Accelleration;

    public class Baker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Movement()
            {
                MaxMovementSpeed = _authoring.m_MaxMoveSpeed,
                Accelleration = _authoring.m_Accelleration
            });
        }
    }
}

public struct Movement : IComponentData
{
    public float3 MovementDirection;
    public float MaxMovementSpeed;
    public float CurrentMoveSpeed;
    public float Accelleration;
}
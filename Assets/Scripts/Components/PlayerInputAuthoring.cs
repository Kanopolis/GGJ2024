using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerInputAuthoring : MonoBehaviour
{
    public class Baker : Baker<PlayerInputAuthoring>
    {
        public override void Bake(PlayerInputAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerInput());
        }
    }
}

public struct PlayerInput : IComponentData
{
    public float2 MovementInput;
    public float2 ShootInput;
}
using Unity.Burst;
using Unity.Entities;

public partial struct InputSystem_Player : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
        _state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        
    }

    [BurstCompile]
    public partial struct InputSystem_PlayerJob : IJobEntity
    {
        public void Execute()
        {
            
        }
    }
}
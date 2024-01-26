using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct MovementSystem : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
        _state.RequireForUpdate<Actor>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        MovementSystemJob moveJob = new MovementSystemJob()
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        moveJob.Schedule();
    }

    [BurstCompile]
    public partial struct MovementSystemJob : IJobEntity
    {
        public float DeltaTime;

        public void Execute(in Movement movement, ref LocalTransform lTrans)
        {
            lTrans.Position += movement.MovementDirection * movement.MovementSpeed * DeltaTime;
        }
    }
}
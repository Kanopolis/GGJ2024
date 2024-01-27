using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;

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

        public void Execute(ref Movement movement, ref LocalTransform lTrans)
        {
            if (movement.Accelleration > 0)
            {
                if (math.lengthsq(movement.MovementDirection) > 0f)
                    movement.CurrentMoveSpeed += movement.Accelleration * DeltaTime;
                else
                    movement.CurrentMoveSpeed -= movement.Accelleration * DeltaTime;

                movement.CurrentMoveSpeed = math.clamp(movement.CurrentMoveSpeed, 0f, movement.MaxMovementSpeed);
            }
            else
            {
                movement.CurrentMoveSpeed = movement.MaxMovementSpeed;
            }

            lTrans.Position += movement.MovementDirection * movement.CurrentMoveSpeed * DeltaTime;
        }
    }
}
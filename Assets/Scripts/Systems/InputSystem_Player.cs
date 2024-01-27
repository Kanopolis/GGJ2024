using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class InputSystem_Player : SystemBase
{
    private PlayerInputs playerInputs;

    protected override void OnCreate()
    {
        RequireForUpdate<Player>();

        playerInputs = new PlayerInputs();
    }

    protected override void OnStartRunning()
    {
        playerInputs?.Enable();
    }

    protected override void OnStopRunning()
    {
        playerInputs?.Disable();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        Vector2 movementInput = playerInputs.Combat.Movement.ReadValue<Vector2>();
        Vector2 shootInput = playerInputs.Combat.Shooting.ReadValue<Vector2>();

        InputSystem_PlayerJob inputJob = new InputSystem_PlayerJob()
        {
            MoveInput = movementInput,
            ShootInput = shootInput
        };
        Dependency = inputJob.Schedule(Dependency);
    }

    [BurstCompile]
    [WithAll(typeof(Player))]
    public partial struct InputSystem_PlayerJob : IJobEntity
    {
        public float2 MoveInput;
        public float2 ShootInput;

        public void Execute(ref Movement movement, ref PlayerInput input)
        {
            input.MovementInput = MoveInput;
            input.ShootInput = ShootInput;

            movement.MovementDirection = new float3(MoveInput.x, 0, MoveInput.y);
        }
    }
}
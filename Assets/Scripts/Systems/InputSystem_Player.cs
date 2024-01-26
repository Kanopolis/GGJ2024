using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class InputSystem_Player : SystemBase
{
    private PlayerInputs playerInputs;

    protected override void OnCreate()
    {
        RequireForUpdate<Player>();

        UnityEngine.Debug.Log($"Create!");
        playerInputs = new PlayerInputs();
    }

    protected override void OnStartRunning()
    {
        UnityEngine.Debug.Log($"Enable!");
        playerInputs?.Enable();
    }

    protected override void OnStopRunning()
    {
        UnityEngine.Debug.Log($"Disable!");
        playerInputs?.Disable();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        Vector2 horizontalInput = playerInputs.Combat.Movement.ReadValue<Vector2>();
        UnityEngine.Debug.Log(Mouse.current.leftButton.wasPressedThisFrame);
        UnityEngine.Debug.Log($"Current Inputs: {horizontalInput}");

        InputSystem_PlayerJob inputJob = new InputSystem_PlayerJob()
        {
            HIn = horizontalInput.x,
            VIn = horizontalInput.y
        };
        inputJob.Schedule();
    }

    [BurstCompile]
    [WithAll(typeof(Player))]
    public partial struct InputSystem_PlayerJob : IJobEntity
    {
        public float HIn;
        public float VIn;

        public void Execute(ref Movement movement)
        {
            movement.MovementDirection = math.normalizesafe(new float3(HIn, 0, VIn));
        }
    }
}
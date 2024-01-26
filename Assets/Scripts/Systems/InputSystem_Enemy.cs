using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct InputSystem_Enemy : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
        _state.RequireForUpdate<Enemy>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        NativeList<float3> playerPositions = new NativeList<float3>(Allocator.TempJob);
        foreach ((RefRO<Player> player, RefRO<LocalTransform> lTrans) in SystemAPI.Query<RefRO<Player>, RefRO<LocalTransform>>())
        {
            playerPositions.Add(lTrans.ValueRO.Position);
        }

        InputSystem_EnemyJob enemyMoveDataJob = new InputSystem_EnemyJob()
        {
            PlayerPositions = playerPositions
        };
        enemyMoveDataJob.Schedule();
    }

    [BurstCompile]
    public partial struct InputSystem_EnemyJob : IJobEntity
    {
        public NativeList<float3> PlayerPositions;

        public void Execute(ref Movement movement, in LocalTransform lTrans, in Enemy enemy)
        {
            float3 closestPlayerPosition = float3.zero;
            float closestDistanceSqr = float.MaxValue;
            for (int i = 0; i < PlayerPositions.Length; i++)
            {
                float currentDistance = math.distancesq(PlayerPositions[i], lTrans.Position);
                if (currentDistance < closestDistanceSqr)
                {
                    closestDistanceSqr = currentDistance;
                    closestPlayerPosition = PlayerPositions[i];
                }
            }


            UnityEngine.Debug.Log($"Current Distance Sqr: {closestDistanceSqr}");
            if (closestDistanceSqr > enemy.PreferredDistanceToTarget * enemy.PreferredDistanceToTarget)
            {
                UnityEngine.Debug.Log($"Moving from: {lTrans.Position} in direction {closestPlayerPosition - lTrans.Position}");
                movement.MovementDirection = math.normalizesafe(closestPlayerPosition - lTrans.Position);
            }
            else
                movement.MovementDirection = float3.zero;
        }
    }
}
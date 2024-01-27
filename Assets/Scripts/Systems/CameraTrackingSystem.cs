using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct CameraTrackingSystem : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        foreach ((RefRO<Player> player, RefRO<LocalTransform> lTrans) in SystemAPI.Query<RefRO<Player>, RefRO<LocalTransform>>().WithNone<Projectile, Enemy>())
        {
            CameraTracker.mainCameraTracker.TrackPosition(lTrans.ValueRO.Position);
        }
    }

    [BurstCompile]
    public partial struct CameraTrackingSystemJob : IJobEntity
    {
        public void Execute()
        {

        }
    }
}
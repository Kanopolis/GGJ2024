using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct CameraTrackingSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        foreach (RefRO<LocalTransform> lTrans in SystemAPI.Query<RefRO<LocalTransform>>())
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
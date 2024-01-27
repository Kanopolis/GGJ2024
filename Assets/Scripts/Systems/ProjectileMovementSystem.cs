using Unity.Burst;
using Unity.Entities;

public partial class ProjectileMovementSystem : SystemBase
{
    protected override void OnCreate()
    {

    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        EntityCommandBuffer buffer = new EntityCommandBuffer(WorldUpdateAllocator);

        Entities.ForEach((Entity ent, ref Projectile proj) =>
        {
            proj.LifeTime += SystemAPI.Time.DeltaTime;
            if(proj.LifeTime >= proj.MaxLifeTime)
                buffer.DestroyEntity(ent);
        }).Run();

        buffer.Playback(EntityManager);
    }

    [BurstCompile]
    public partial struct ProjectileMovementSystemJob : IJobEntity
    {
        public void Execute()
        {
            
        }
    }
}
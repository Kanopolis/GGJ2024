using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(MovementSystem))]
public partial struct PlayerPeriodicShootSystem : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
        _state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        EntityCommandBuffer buffer = new EntityCommandBuffer(_state.WorldUpdateAllocator);
        PlayerPeriodicShootSystemJob attackJob = new PlayerPeriodicShootSystemJob()
        {
            EntityBuffer = buffer,
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        _state.Dependency.Complete();
        attackJob.Run();
        buffer.Playback(_state.EntityManager);
    }

    [BurstCompile]
    public partial struct PlayerPeriodicShootSystemJob : IJobEntity
    {
        public EntityCommandBuffer EntityBuffer;
        public float DeltaTime;

        public void Execute(ref PlayerPeriodicShoot periodicShoot, in LocalTransform lTrans)
        {
            if (periodicShoot.CurrentCooldown > 0)
            {
                periodicShoot.CurrentCooldown -= DeltaTime;
            }
            else
            {
                //Attack!
                //Spawn Bullet
                float angleIncrement = (2 * math.PI) / periodicShoot.ProjectileCount;
                for (int i = 0; i < periodicShoot.ProjectileCount; i++)
                {
                    float angle = angleIncrement * i;
                    Entity newBullet = EntityBuffer.Instantiate(periodicShoot.ProjectileEntity);
                    float3 moveDir = new float3(math.cos(angle), 0, math.sin(angle));
                    EntityBuffer.SetComponent<Movement>(newBullet, new Movement()
                    {
                        MaxMovementSpeed = periodicShoot.ProjectileSpeed,
                        MovementDirection = moveDir
                    });
                    EntityBuffer.SetComponent<LocalTransform>(newBullet, new LocalTransform()
                    {
                        Position = lTrans.Position,
                        Rotation = quaternion.LookRotation(moveDir, lTrans.Up()),
                        Scale = 1
                    });
                    EntityBuffer.SetComponent<Projectile>(newBullet, new Projectile()
                    {
                        MaxLifeTime = periodicShoot.ProjectileLifeTime,
                        Damage = periodicShoot.Damage
                    });
                }

                periodicShoot.CurrentCooldown += periodicShoot.MaxCooldown;
            }
        }
    }
}
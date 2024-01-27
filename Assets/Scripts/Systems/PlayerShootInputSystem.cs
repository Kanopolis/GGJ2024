using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(InputSystem_Player))]
[UpdateAfter(typeof(MovementSystem))]
public partial struct PlayerShootInputSystem : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
        _state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        EntityCommandBuffer buffer = new EntityCommandBuffer(_state.WorldUpdateAllocator);
        PlayerShootInputSystemJob attackJob = new PlayerShootInputSystemJob()
        {
            EntityBuffer = buffer,
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        _state.Dependency.Complete();
        attackJob.Run();
        buffer.Playback(_state.EntityManager);
    }

    [BurstCompile]
    public partial struct PlayerShootInputSystemJob : IJobEntity
    {
        public EntityCommandBuffer EntityBuffer;
        public float DeltaTime;

        public void Execute(ref PlayerShootInput inputShoot, in PlayerInput input, in LocalTransform lTrans)
        {
            if (inputShoot.CurrentCooldown > 0)
            {
                inputShoot.CurrentCooldown -= DeltaTime;
            }
            else
            {
                //Attack!
                //Spawn Bullet
                if(math.lengthsq(input.ShootInput) > 0)
                {
                    Entity newBullet = EntityBuffer.Instantiate(inputShoot.ProjectileEntity);
                    float3 moveDir = math.normalizesafe(new float3(input.ShootInput.x, 0, input.ShootInput.y));
                    EntityBuffer.SetComponent<Movement>(newBullet, new Movement()
                    {
                        MaxMovementSpeed = inputShoot.ProjectileSpeed,
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
                        MaxLifeTime = inputShoot.ProjectileLifeTime,
                        Damage = inputShoot.Damage
                    });

                    inputShoot.CurrentCooldown += inputShoot.MaxCooldown;
                }
            }
        }
    }
}
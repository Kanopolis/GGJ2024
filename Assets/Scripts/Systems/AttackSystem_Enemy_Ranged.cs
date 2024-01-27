using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using static AttackSystem_Enemy_Melee;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct AttackSystem_Enemy_Ranged : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        NativeList<SPlayerData> playerData = new NativeList<SPlayerData>(Allocator.TempJob);
        foreach ((RefRO<Player> player, RefRW<Actor> actor, RefRO<LocalTransform> lTrans) in SystemAPI.Query<RefRO<Player>, RefRW<Actor>, RefRO<LocalTransform>>())
        {
            playerData.Add(new SPlayerData()
            {
                CurrentPositon = lTrans.ValueRO.Position,
                CurrentData = actor
            });
        }

        EntityCommandBuffer buffer = new EntityCommandBuffer(_state.WorldUpdateAllocator);
        AttackSystem_Enemy_RangedJob attackJob = new AttackSystem_Enemy_RangedJob()
        {
            EntityBuffer = buffer,
            PlayerData = playerData,
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        attackJob.Run();
        buffer.Playback(_state.EntityManager);
    }

    [BurstCompile]
    public partial struct AttackSystem_Enemy_RangedJob : IJobEntity
    {
        public EntityCommandBuffer EntityBuffer;
        public NativeList<SPlayerData> PlayerData;
        public float DeltaTime;

        public void Execute(ref RangedAttack rangedAtk, in LocalTransform lTrans)
        {
            if (rangedAtk.CurrentCooldown > 0)
            {
                rangedAtk.CurrentCooldown -= DeltaTime;
            }
            else
            {
                //Attack!
                bool hasHit = false;
                float closestDistance = float.MaxValue;
                float3 closestPlayerPosition = float3.zero;
                for (int i = 0; i < PlayerData.Length; i++)
                {
                    //Check for Radius / Direction
                    float currentDistance = math.distancesq(PlayerData[i].CurrentPositon, lTrans.Position);
                    if (currentDistance <= rangedAtk.Range * rangedAtk.Range)
                    {
                        if(currentDistance < closestDistance)
                        {
                            //Found Player!
                            closestDistance = currentDistance;
                            closestPlayerPosition = PlayerData[i].CurrentPositon;
                            hasHit = true;
                        }
                    }
                }

                if (hasHit)
                {
                    //Spawn Bullet
                    Entity newBullet = EntityBuffer.Instantiate(rangedAtk.ProjectileEntity);
                    float3 moveDir = math.normalizesafe(closestPlayerPosition - lTrans.Position);
                    EntityBuffer.SetComponent<Movement>(newBullet, new Movement()
                    {
                        MaxMovementSpeed = rangedAtk.ProjectileSpeed,
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
                        MaxLifeTime = rangedAtk.ProjectileLifeTime,
                        Damage = rangedAtk.Damage
                    });

                    rangedAtk.CurrentCooldown += rangedAtk.MaxCooldown;
                }
            }
        }
    }
}
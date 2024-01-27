using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct AttackSystem_Enemy_Melee : ISystem
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

        AttackSystem_Enemy_MeleeJob attackJob = new AttackSystem_Enemy_MeleeJob()
        {
            PlayerData = playerData,
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        attackJob.Schedule();
    }

    [BurstCompile]
    public partial struct AttackSystem_Enemy_MeleeJob : IJobEntity
    {
        public NativeList<SPlayerData> PlayerData;
        public float DeltaTime;

        public void Execute(ref MeleeAttack atk, in LocalTransform lTrans)
        {
            if (atk.CurrentCooldown > 0)
            {
                atk.CurrentCooldown -= DeltaTime;
            }
            else
            {
                //Attack!
                bool hasHit = false;
                for (int i = 0; i < PlayerData.Length; i++)
                {
                    //Check for Radius / Direction
                    if (math.distancesq(PlayerData[i].CurrentPositon, lTrans.Position) <= atk.Range * atk.Range)
                    {
                        //Hurt Player!
                        PlayerData[i].CurrentData.ValueRW.CurrentHitPoints -= atk.Damage;
                        hasHit = true;

                        if (PlayerData[i].CurrentData.ValueRO.CurrentHitPoints <= 0)
                        {
                            //Player Death!
                        }
                    }

                    if (hasHit)
                        atk.CurrentCooldown += atk.MaxCooldown;
                }
            }
        }
    }

    public struct SPlayerData
    {
        public float3 CurrentPositon;
        public RefRW<Actor> CurrentData;
    }
}
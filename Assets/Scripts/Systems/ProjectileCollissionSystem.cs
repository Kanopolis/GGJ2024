using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct ProjectileCollissionSystem : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
        _state.RequireForUpdate<SimulationSingleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        EntityCommandBuffer buffer = new EntityCommandBuffer(_state.WorldUpdateAllocator);
        ProjectileCollissionSystemJob collissionJob = new ProjectileCollissionSystemJob()
        {
            EntityBuffer = buffer,
            ProjectileGroupLookup = SystemAPI.GetComponentLookup<Projectile>(),
            PlayerGroupLookup = SystemAPI.GetComponentLookup<Player>(),
            PlayerBulletGroupLookup = SystemAPI.GetComponentLookup<PlayerBullet>(),
            EnemyGroupLookup = SystemAPI.GetComponentLookup<Enemy>(),
            EnemyBulletGroupLookup = SystemAPI.GetComponentLookup<EnemyBullet>(),
            ActorGroupLookup = SystemAPI.GetComponentLookup<Actor>()
        };
        JobHandle handle = collissionJob.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), _state.Dependency);
        handle.Complete();
        buffer.Playback(_state.EntityManager);
    }

    [BurstCompile]
    public partial struct ProjectileCollissionSystemJob : ITriggerEventsJob
    {
        public EntityCommandBuffer EntityBuffer;
        public ComponentLookup<Projectile> ProjectileGroupLookup;
        [ReadOnly] public ComponentLookup<Player> PlayerGroupLookup;
        [ReadOnly] public ComponentLookup<PlayerBullet> PlayerBulletGroupLookup;
        [ReadOnly] public ComponentLookup<Enemy> EnemyGroupLookup;
        [ReadOnly] public ComponentLookup<EnemyBullet> EnemyBulletGroupLookup;
        public ComponentLookup<Actor> ActorGroupLookup;

        public void Execute(TriggerEvent _triggerEvent)
        {
            Entity hitEnt = _triggerEvent.EntityB;
            Entity projectileEnt = _triggerEvent.EntityA;
            bool isValid = false;
            bool hitPlayer = false;

            if (ProjectileGroupLookup.HasComponent(_triggerEvent.EntityA))
            {
                if (PlayerBulletGroupLookup.HasComponent(_triggerEvent.EntityA) &&
                    EnemyGroupLookup.HasComponent(_triggerEvent.EntityB))
                {
                    //Player Shooting Enemy
                    isValid = true;
                }
                else if (EnemyBulletGroupLookup.HasComponent(_triggerEvent.EntityA) &&
                    PlayerGroupLookup.HasComponent(_triggerEvent.EntityB))
                {
                    //Enemy Shooting Player
                    isValid = true;
                    hitPlayer = true;
                }
            }
            else if (ProjectileGroupLookup.HasComponent(_triggerEvent.EntityB))
            {
                hitEnt = _triggerEvent.EntityA;
                projectileEnt = _triggerEvent.EntityB;

                if (PlayerBulletGroupLookup.HasComponent(_triggerEvent.EntityB) &&
                    EnemyGroupLookup.HasComponent(_triggerEvent.EntityA))
                {
                    //Player Shooting Enemy
                    isValid = true;
                }
                else if (EnemyBulletGroupLookup.HasComponent(_triggerEvent.EntityB) &&
                    PlayerGroupLookup.HasComponent(_triggerEvent.EntityA))
                {
                    //Enemy Shooting Player
                    isValid = true;
                    hitPlayer = true;
                }
            }

            if (isValid)
            {
                Projectile projectile = ProjectileGroupLookup[projectileEnt];
                Actor player = ActorGroupLookup[hitEnt];
                player.CurrentHitPoints -= projectile.Damage;

                if (player.CurrentHitPoints <= 0)
                {
                    if (hitPlayer)
                        CombatManager.CallPlayerDeath();
                    else
                        EntityBuffer.DestroyEntity(hitEnt);
                }

                ActorGroupLookup[hitEnt] = player;

                EntityBuffer.DestroyEntity(projectileEnt);
            }
        }
    }
}
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
            ActorGroupLookup= SystemAPI.GetComponentLookup<Actor>()
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
        public ComponentLookup<Actor> ActorGroupLookup;

        public void Execute(TriggerEvent _triggerEvent)
        {
            Entity playerEnt = _triggerEvent.EntityB;
            Entity projectileEnt = _triggerEvent.EntityA;
            bool isValid = false;

            if (ProjectileGroupLookup.HasComponent(_triggerEvent.EntityA) &&
                PlayerGroupLookup.HasComponent(_triggerEvent.EntityB))
            {
                isValid = true;
            }
            else if (ProjectileGroupLookup.HasComponent(_triggerEvent.EntityB) &&
                 PlayerGroupLookup.HasComponent(_triggerEvent.EntityA))
            {
                playerEnt = _triggerEvent.EntityA;
                projectileEnt = _triggerEvent.EntityB;
                isValid = true;
            }

            if(isValid)
            {
                Projectile projectile = ProjectileGroupLookup[projectileEnt];
                Actor player = ActorGroupLookup[playerEnt];
                player.CurrentHitPoints -= projectile.Damage;

                if (player.CurrentHitPoints <= 0)
                {
                    CombatManager.CallPlayerDeath();
                }

                ActorGroupLookup[playerEnt] = player;

                EntityBuffer.DestroyEntity(projectileEnt);
            }
        }
    }
}
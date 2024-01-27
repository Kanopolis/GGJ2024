using Unity.Burst;
using Unity.Entities;
using static EnemySpawnerAuthoring;

public partial class HPCheckSystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        ComponentLookup<Player> playerGroupLookup = SystemAPI.GetComponentLookup<Player>();
        ComponentLookup<Enemy> enemyGroupLookup = SystemAPI.GetComponentLookup<Enemy>();

        EnemySpawner spawner = SystemAPI.GetSingleton<EnemySpawner>();

        EntityCommandBuffer entityBuffer = new EntityCommandBuffer(WorldUpdateAllocator);
        Entities.ForEach((Entity ent, in Actor actor) =>
        {
            if(actor.CurrentHitPoints <= 0)
            {
                if (playerGroupLookup.HasComponent(ent))
                {
                    CombatManager.CallPlayerDeath();
                }
                else if (enemyGroupLookup.HasComponent(ent))
                {
                    spawner.CurrentSlaynEnemiesCount++;
                    entityBuffer.DestroyEntity(ent);

                    if(spawner.CurrentSlaynEnemiesCount >= spawner.AllEnemiesCount)
                        CombatManager.CallAllEnemiesDeath();
                }
            }
        }).Run();

        SystemAPI.SetSingleton<EnemySpawner>(spawner);

        entityBuffer.Playback(EntityManager);
    }
}
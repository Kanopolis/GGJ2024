using Unity.Burst;
using Unity.Entities;
using static EnemySpawnerAuthoring;

public partial class UpdateUISystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<EnemySpawner>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        foreach ((RefRO<Actor> actor, RefRO<Player> player) in SystemAPI.Query<RefRO<Actor>, RefRO<Player>>().WithNone<Projectile>())
        {
            GUI_HPSlider.SetupSlider(actor.ValueRO.MaxHitPoints);
            GUI_HPSlider.SetNewValue(actor.ValueRO.CurrentHitPoints);
        }

        EnemySpawner spawner = SystemAPI.GetSingleton<EnemySpawner>();
        GUI_ProgressSlider.SetupSlider(spawner.AllEnemiesCount);
        GUI_ProgressSlider.SetNewValue(spawner.CurrentSlaynEnemiesCount);
    }
}
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static EnemySpawnerAuthoring;

public partial struct WaveSpawnSystem : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
        
    }

    public void OnUpdate(ref SystemState _state)
    {
        EntityCommandBuffer entBuffer = new EntityCommandBuffer(_state.WorldUpdateAllocator);
        Random rnd = new Random((uint)System.DateTime.UtcNow.Ticks);

        WaveSpawnSystemJob spawnJob = new WaveSpawnSystemJob()
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            EntityBuffer = entBuffer,
            Rnd = rnd
        };
        spawnJob.Run();
        entBuffer.Playback(_state.EntityManager);
    }

    [BurstCompile]
    public partial struct WaveSpawnSystemJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer EntityBuffer;
        public Random Rnd;

        public void Execute(ref EnemySpawner spawner, DynamicBuffer<EnemySpawnerBufferElement> spawnBuffer)
        {
            if(spawnBuffer.Length > 0)
            {
                if (spawner.Timer > 0)
                {
                    spawner.Timer -= DeltaTime;
                }
                else
                {
                    Entity newEnemy = EntityBuffer.Instantiate(spawnBuffer[0].EnemyPrefab);
                    float3 enemyPosition = Rnd.NextFloat3(-60, 60);
                    enemyPosition.y = 3;

                    EntityBuffer.SetComponent<LocalTransform>(newEnemy, new LocalTransform()
                    {
                        Position = enemyPosition,
                        Rotation = quaternion.identity,
                        Scale = 1
                    });

                    spawner.SpawnCounter++;
                    if (spawner.SpawnCounter >= spawnBuffer[0].SpawnCount)
                    {
                        //Next Wave
                        spawnBuffer.RemoveAt(0);
                        spawner.Timer += spawner.TimeBetweenWaves;
                        spawner.SpawnCounter = 0;
                    }
                    else
                    {
                        //Next Spawn
                        spawner.Timer += spawner.TimeBetweenSpawns;
                    }
                }
            }
            else
            {
                //Level is done spawning!
                CombatManager.CallSpawnEnd();
            }
        }
    }
}
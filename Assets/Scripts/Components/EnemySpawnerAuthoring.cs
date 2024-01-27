using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    [SerializeField]
    public float m_TimeBetweenWaves;
    [SerializeField]
    public float m_TimeBetweenSpawns;
    [SerializeField]
    private SEnemyWaveSettings[] m_Waves;

    public class Baker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemySpawner()
            {
                TimeBetweenWaves = _authoring.m_TimeBetweenWaves,
                TimeBetweenSpawns = _authoring.m_TimeBetweenSpawns
            });

            DynamicBuffer<EnemySpawnerBufferElement> spawnDataBuffer = AddBuffer<EnemySpawnerBufferElement>(entity);
            for (int i = 0; i < _authoring.m_Waves.Length; i++)
            {
                spawnDataBuffer.Add(new EnemySpawnerBufferElement()
                {
                    EnemyPrefab= GetEntity(_authoring.m_Waves[i].EnemyType, TransformUsageFlags.Dynamic),
                    SpawnCount = _authoring.m_Waves[i].SpawnCount
                });
            }
        }
    }

    public struct EnemySpawner : IComponentData
    {
        public float TimeBetweenWaves;
        public float TimeBetweenSpawns;
        public float Timer;
        public float SpawnCounter;
    }

    [System.Serializable]
    public struct SEnemyWaveSettings
    {
        public GameObject EnemyType;
        public int SpawnCount;
    }
}
using Unity.Entities;

[InternalBufferCapacity(5)]
public struct EnemySpawnerBufferElement : IBufferElementData
{
	public Entity EnemyPrefab;
	public int SpawnCount;
}

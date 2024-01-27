using Unity.Entities;

[InternalBufferCapacity(5)]
public struct DialogBuffer : IBufferElementData
{
	public long Value;
}

[InternalBufferCapacity(20)]
public struct DialogChoicesEnemyBuffer : IBufferElementData
{
    public long Value;
}

[InternalBufferCapacity(20)]
public struct DialogChoicesLocationBuffer : IBufferElementData
{
    public long Value;
}

[InternalBufferCapacity(20)]
public struct DialogChoicesShaderBuffer : IBufferElementData
{
    public long Value;
}

[InternalBufferCapacity(20)]
public struct DialogChoicesMusicBuffer : IBufferElementData
{
    public long Value;
}

[InternalBufferCapacity(4)]
public struct DialogChoicesEnemyReturnBuffer : IBufferElementData
{
    public long Value;
}

[InternalBufferCapacity(4)]
public struct DialogChoicesLocationReturnBuffer : IBufferElementData
{
    public long Value;
}

[InternalBufferCapacity(4)]
public struct DialogChoicesShaderReturnBuffer : IBufferElementData
{
    public long Value;
}

[InternalBufferCapacity(4)]
public struct DialogChoicesMusicReturnBuffer : IBufferElementData
{
    public long Value;
}

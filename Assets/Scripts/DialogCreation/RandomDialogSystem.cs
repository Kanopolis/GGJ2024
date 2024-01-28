using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public partial struct RandomDialogSystem : ISystem
{
    public void OnCreate(ref SystemState _state)
    {
        _state.RequireForUpdate<DialogReturnValue_Data>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        _state.Enabled = false;
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        var random = Random.CreateFromIndex((uint)UnityEngine.Random.Range(0,1000));

        RandomDialogSystemJob job = new()
        {
            MyRandom = random,
            ecbParallel = ecb.AsParallelWriter(),
        };
        job.Schedule();

        _state.Dependency.Complete();

        ecb.Playback(_state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public partial struct RandomDialogSystemJob : IJobEntity
    {
        public Random MyRandom;
        internal EntityCommandBuffer.ParallelWriter ecbParallel;

        public readonly void Execute(
            in Entity _entity,
            [ChunkIndexInQuery] int sortKey,
            ref DynamicBuffer<DialogBuffer> _dialogBuffer,
            ref DynamicBuffer<DialogChoicesEnemyBuffer> _dialogChoices1Buffer,
            ref DynamicBuffer<DialogChoicesLocationBuffer> _dialogChoices2Buffer,
            ref DynamicBuffer<DialogChoicesShaderBuffer> _dialogChoices3Buffer,
            ref DynamicBuffer<DialogChoicesMusicBuffer> _dialogChoices4Buffer,
            ref DynamicBuffer<DialogChoicesEnemyReturnBuffer> _dialogChoices1ReturnBuffer,
            ref DynamicBuffer<DialogChoicesLocationReturnBuffer> _dialogChoices2ReturnBuffer,
            ref DynamicBuffer<DialogChoicesShaderReturnBuffer> _dialogChoices3ReturnBuffer,
            ref DynamicBuffer<DialogChoicesMusicReturnBuffer> _dialogChoices4ReturnBuffer,
            ref DialogReturnValue_Data _dialogReturnValue_Data)
        {
            _dialogReturnValue_Data.DialogEntryKey = _dialogBuffer[MyRandom.NextInt(_dialogBuffer.Length)].Value;

            for (int i = 0; i < 4; i++)
            {
                var NextChoice1Id = MyRandom.NextInt(_dialogChoices1Buffer.Length - 1);
                var NextChoice2Id = MyRandom.NextInt(_dialogChoices2Buffer.Length - 1);
                var NextChoice3Id = MyRandom.NextInt(_dialogChoices3Buffer.Length - 1);
                var NextChoice4Id = MyRandom.NextInt(_dialogChoices4Buffer.Length - 1);

                _dialogChoices1ReturnBuffer[i] = new() { Value = _dialogChoices1Buffer[NextChoice1Id].Value };
                _dialogChoices2ReturnBuffer[i] = new() { Value = _dialogChoices2Buffer[NextChoice2Id].Value };
                _dialogChoices3ReturnBuffer[i] = new() { Value = _dialogChoices3Buffer[NextChoice3Id].Value };
                _dialogChoices4ReturnBuffer[i] = new() { Value = _dialogChoices4Buffer[NextChoice4Id].Value };

                _dialogChoices1Buffer.RemoveAt(NextChoice1Id);
                _dialogChoices2Buffer.RemoveAt(NextChoice2Id);
                _dialogChoices3Buffer.RemoveAt(NextChoice3Id);
                _dialogChoices4Buffer.RemoveAt(NextChoice4Id);
            }
            UnityEngine.Debug.LogError(sortKey + " " + _entity.Index);
            ecbParallel.AddComponent<AfterRandomDialogGenerationTag>(sortKey, _entity);
        }
    }
}
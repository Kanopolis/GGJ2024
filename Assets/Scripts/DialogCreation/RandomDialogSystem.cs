using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct RandomDialogSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState _state)
    {
        _state.Enabled = false;
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        RandomDialogSystemJob job = new()
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
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
        public float DeltaTime;
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
            var random = Unity.Mathematics.Random.CreateFromIndex((uint)DeltaTime);
            _dialogReturnValue_Data.DialogEntryKey = _dialogBuffer[random.NextInt(_dialogBuffer.Length - 1)].Value;

            for (int i = 0; i < 4; i++)
            {
                var NextChoice1Id = random.NextInt(_dialogChoices1Buffer.Length - 1);
                var NextChoice2Id = random.NextInt(_dialogChoices2Buffer.Length - 1);
                var NextChoice3Id = random.NextInt(_dialogChoices3Buffer.Length - 1);
                var NextChoice4Id = random.NextInt(_dialogChoices4Buffer.Length - 1);

                _dialogChoices1ReturnBuffer[i] = new() { Value = _dialogChoices1Buffer[NextChoice1Id].Value };
                _dialogChoices2ReturnBuffer[i] = new() { Value = _dialogChoices2Buffer[NextChoice2Id].Value };
                _dialogChoices3ReturnBuffer[i] = new() { Value = _dialogChoices3Buffer[NextChoice3Id].Value };
                _dialogChoices4ReturnBuffer[i] = new() { Value = _dialogChoices4Buffer[NextChoice4Id].Value };

                _dialogChoices1Buffer.RemoveAt(NextChoice1Id);
                _dialogChoices2Buffer.RemoveAt(NextChoice2Id);
                _dialogChoices3Buffer.RemoveAt(NextChoice3Id);
                _dialogChoices4Buffer.RemoveAt(NextChoice4Id);
            }
            ecbParallel.AddComponent<AfterRandomDialogGenerationTag>(sortKey, _entity);
        }
    }
}
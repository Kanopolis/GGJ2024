using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

public partial class DialogChoiceSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<DialogSelectionTag_Data>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        EntityCommandBuffer ecb = new(Allocator.Temp);
        foreach (var item in SystemAPI.Query<DynamicBuffer<PlayerUIInputBuffer>>())
        {
            var bufferEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerUIInputBuffer>()).GetSingletonEntity();
            var player1Buffer = item[0];
            var player2Buffer = item[1];
            var player3Buffer = item[2];
            var player4Buffer = item[3];

            HandlePlayer(player1Buffer, 0);
            HandlePlayer(player2Buffer, 1);
            HandlePlayer(player3Buffer, 2);
            HandlePlayer(player4Buffer, 3);

            ecb.RemoveComponent<PlayerUIInputBuffer>(bufferEntity);
            DynamicBuffer<PlayerUIInputBuffer> newBuffer = ecb.AddBuffer<PlayerUIInputBuffer>(bufferEntity);
            newBuffer.Length = 4;
            for (int i = 0; i < newBuffer.Length; i++)
            {
                newBuffer[i] = new PlayerUIInputBuffer();
            }
        }
        ecb.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
    }

    private void HandlePlayer(PlayerUIInputBuffer _input, int _playerId)
    {
        if (_input.Choice1Clicked)
        {
            DialogCanvasHandler.SelectDialog(_playerId, 0);
        }
        else if(_input.Choice2Clicked)
        {
            DialogCanvasHandler.SelectDialog(_playerId, 1);
        }
        else if (_input.Choice3Clicked)
        {
            DialogCanvasHandler.SelectDialog(_playerId, 2);
        }
        else if (_input.Choice4Clicked)
        {
            DialogCanvasHandler.SelectDialog(_playerId, 3);
        }
        else if (_input.ConfirmClicked)
        {
            DialogCanvasHandler.LockDialog(_playerId);
        }
    }

    [BurstCompile]
    public partial struct DialogChoiceSystemJob : IJobEntity
    {
        public void Execute()
        {
            
        }
    }
}
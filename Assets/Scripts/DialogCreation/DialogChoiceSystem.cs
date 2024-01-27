using Unity.Burst;
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
        foreach (var item in SystemAPI.Query<DynamicBuffer<PlayerUIInputBuffer>>())
        {
            var player1Buffer = item[0];
            var player2Buffer = item[1];
            var player3Buffer = item[2];
            var player4Buffer = item[3];

            HandlePlayer(player1Buffer, 0);
            HandlePlayer(player2Buffer, 1);
            HandlePlayer(player3Buffer, 2);
            HandlePlayer(player4Buffer, 3);
        }
    }

    private void HandlePlayer(PlayerUIInputBuffer _input, int _playerId)
    {

    }

    [BurstCompile]
    public partial struct DialogChoiceSystemJob : IJobEntity
    {
        public void Execute()
        {
            
        }
    }
}
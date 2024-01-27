using Unity.Burst;
using Unity.Entities;

public partial class MainMenuSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<>
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
    }

    private static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    [BurstCompile]
    public partial struct MainMenuSystemJob : IJobEntity
    {
        public void Execute(in DynamicBuffer<PlayerUIInputBuffer> _playerUIInputBuffer)
        {
            var player1Buffer = _playerUIInputBuffer[0];

            if (player1Buffer.Choice1Clicked)
            {

            }
            else if (player1Buffer.Choice2Clicked)
            {

            }
            else if (player1Buffer.Choice3Clicked)
            {

            }
            else if (player1Buffer.Choice4Clicked)
            {
                ExitGame();
            }
        }
    }
}
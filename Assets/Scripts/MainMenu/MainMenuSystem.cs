using Unity.Burst;
using Unity.Entities;

public partial class MainMenuSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<MainMenuTag>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        UnityEngine.Debug.LogError("Update");

        var job = new MainMenuSystemJob();
        job.Schedule();
    }

    [BurstCompile]
    public partial struct MainMenuSystemJob : IJobEntity
    {
        public void Execute(in DynamicBuffer<PlayerUIInputBuffer> _playerUIInputsBuffer)
        {
            var player1Buffer = _playerUIInputsBuffer[0];

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

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }
    }
}
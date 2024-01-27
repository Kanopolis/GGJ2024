using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

[BurstCompile]
public partial class MainMenuSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<MainMenuTag_Data>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        EntityCommandBuffer ecb = new(Allocator.Temp);
        bool startGame = false;
        foreach (var item in SystemAPI.Query<DynamicBuffer<PlayerUIInputBuffer>>())
        {
            var player1Buffer = item[0];

            var bufferEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerUIInputBuffer>()).GetSingletonEntity();
            if (player1Buffer.Choice1Clicked && PlayerManager.PlayerNumber >= 2)
            {
                ecb.RemoveComponent<MainMenuTag_Data>(bufferEntity);
                ecb.AddComponent<DialogSelectionTag_Data>(bufferEntity);
                startGame = true;
            }
            else if (player1Buffer.Choice2Clicked)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            }
            else if (player1Buffer.Choice3Clicked)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            }
            else if (player1Buffer.Choice4Clicked)
            {
                ExitGame();
            }
            ecb.RemoveComponent<PlayerUIInputBuffer>(bufferEntity);
            DynamicBuffer<PlayerUIInputBuffer> newBuffer = ecb.AddBuffer<PlayerUIInputBuffer>(bufferEntity);
            newBuffer.Length = 4;
            for (int i = 0; i < newBuffer.Length; i++)
            {
                newBuffer[i] = new PlayerUIInputBuffer();
            }
        }
        ecb.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);

        if (startGame)
        {
            PlayerInputManager.instance.DisableJoining();
            StartGame();
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
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
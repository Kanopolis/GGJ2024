using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputComponent : MonoBehaviour
{
    private static Dictionary<int, UnityEngine.InputSystem.PlayerInput> m_AvailablePlayerInputs = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        m_AvailablePlayerInputs = new();
    }

    public static void RegisterPlayer(int _playerIndex, UnityEngine.InputSystem.PlayerInput _playerInput)
    {
        m_AvailablePlayerInputs.Add(_playerIndex, _playerInput);
        _playerInput.onActionTriggered += x => InputActionTriggered(_playerIndex, x);
        //EnableMainMenuForAll();
        EnableMainMenu(0);
    }

    public static void EnableMainMenuForAll()
    {
        for (int i = 0; i < m_AvailablePlayerInputs.Count; i++)
        {
            EnableMainMenu(m_AvailablePlayerInputs[i].playerIndex);
        }
    }
    public static void EnableCombatForAll()
    {
        for (int i = 0; i < m_AvailablePlayerInputs.Count; i++)
        {
            EnableCombat(m_AvailablePlayerInputs[i].playerIndex);
        }
    }

    public static void EnableMainMenu(int _playerIndex)
    {
        m_AvailablePlayerInputs[_playerIndex].actions.FindActionMap("MainMenu").Enable();
    }

    public static void EnableCombat(int _playerIndex)
    {
        m_AvailablePlayerInputs[_playerIndex].actions.FindActionMap("Combat").Enable();
    }

    public static void EndGame()
    {
        foreach (var item in m_AvailablePlayerInputs)
        {
            item.Value.DeactivateInput();
            Destroy(item.Value.gameObject);
        }
    }

    private static void InputActionTriggered(int _playerIndex, InputAction.CallbackContext _context)
    {
        if (!_context.performed)
            return;

        if (_context.action.name.StartsWith("Choice"))
        {

            EntityCommandBuffer ecb = new(Allocator.Temp);
            var bufferEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerUIInputBuffer>()).GetSingletonEntity();
            var playerUIInputBuffer = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerUIInputBuffer>()).GetSingletonBuffer<PlayerUIInputBuffer>();
            ecb.RemoveComponent<PlayerUIInputBuffer>(bufferEntity);
            PlayerUIInputBuffer bufferToWrite;
            switch (_context.action.name)
            {
                case "Choice1":
                    bufferToWrite = new PlayerUIInputBuffer()
                    {
                        Choice1Clicked = true,
                        Choice2Clicked = false,
                        Choice3Clicked = false,
                        Choice4Clicked = false,
                    };
                    break;
                case "Choice2":
                    bufferToWrite = new PlayerUIInputBuffer()
                    {
                        Choice1Clicked = false,
                        Choice2Clicked = true,
                        Choice3Clicked = false,
                        Choice4Clicked = false,
                    };
                    break;
                case "Choice3":
                    bufferToWrite = new PlayerUIInputBuffer()
                    {
                        Choice1Clicked = false,
                        Choice2Clicked = false,
                        Choice3Clicked = true,
                        Choice4Clicked = false,
                    };
                    break;
                case "Choice4":
                    bufferToWrite = new PlayerUIInputBuffer()
                    {
                        Choice1Clicked = false,
                        Choice2Clicked = false,
                        Choice3Clicked = false,
                        Choice4Clicked = true,
                    };
                    break;
                default:
                    bufferToWrite = new PlayerUIInputBuffer()
                    {
                        Choice1Clicked = false,
                        Choice2Clicked = false,
                        Choice3Clicked = false,
                        Choice4Clicked = false,
                    };
                    break;
            }

            DynamicBuffer<PlayerUIInputBuffer> newBuffer = ecb.AddBuffer<PlayerUIInputBuffer>(bufferEntity);
            newBuffer.Length = 4;
            for (int i = 0; i < playerUIInputBuffer.Length; i++)
            {
                if(i == _playerIndex)
                {
                    newBuffer[i] = bufferToWrite;
                }
                else
                {
                    newBuffer[i] = playerUIInputBuffer[i];
                }
            }
            ecb.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
        }
    }

    public class Baker : Baker<PlayerInputComponent>
    {
        public override void Bake(PlayerInputComponent _authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            var playerCombatInputBuffer = AddBuffer<PlayerCombatInputBuffer>(entity);
            var playerUIInputBuffer = AddBuffer<PlayerUIInputBuffer>(entity);

            for (int i = 0; i < 4; i++)
            {
                playerCombatInputBuffer.Add(new());
                playerUIInputBuffer.Add(new());
            }
        }
    }
}

[InternalBufferCapacity(4)]
public struct PlayerCombatInputBuffer : IBufferElementData
{
    public float2 MovementInput;
    public float2 ShootingDirectionInput;
}

[InternalBufferCapacity(4)]
public struct PlayerUIInputBuffer : IBufferElementData
{
    public bool Choice1Clicked;
    public bool Choice2Clicked;
    public bool Choice3Clicked;
    public bool Choice4Clicked;
}
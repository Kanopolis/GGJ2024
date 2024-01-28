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

    private void Awake()
    {
        foreach (var item in m_AvailablePlayerInputs)
        {
            item.Value.onActionTriggered += x => InputActionTriggered(item.Key, x);
        }
    }

    public static void RegisterPlayer(int _playerIndex, UnityEngine.InputSystem.PlayerInput _playerInput)
    {
        m_AvailablePlayerInputs.Add(_playerIndex, _playerInput);
        _playerInput.onActionTriggered += x => InputActionTriggered(_playerIndex, x);
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

    public static void DiableMainMenuForAll()
    {
        for (int i = 0; i < m_AvailablePlayerInputs.Count; i++)
        {
            m_AvailablePlayerInputs[m_AvailablePlayerInputs[i].playerIndex].actions.FindActionMap("MainMenu").Disable();
        }
    }

    public static void EndGame()
    {
        m_AvailablePlayerInputs = new();
        foreach (var item in m_AvailablePlayerInputs)
        {
            item.Value.DeactivateInput();
            Destroy(item.Value.gameObject);
        }
    }

    private static void InputActionTriggered(int _playerIndex, InputAction.CallbackContext _context)
    {
        if (_context.canceled)
        {
            if (_context.action.name == "Movement")
            {
                InputActionMovementTriggered(_playerIndex, float2.zero);
            }
            else if (_context.action.name == "Shooting")
            {
                InputActionShootingTriggered(_playerIndex, float2.zero);
            }
        }

        if (!_context.performed)
            return;
        if (_context.action.name == "RegisterPlayer")
        {
            InputActionStartTriggered(_playerIndex);
        }
        else if (_context.action.name.StartsWith("Choice"))
        {
            InputActionChoiceTriggered(_playerIndex, _context.action.name);
        }
        else if (_context.action.name == "Movement")
        {
            InputActionMovementTriggered(_playerIndex, _context.ReadValue<Vector2>());
        }
        else if (_context.action.name == "Shooting")
        {
            InputActionShootingTriggered(_playerIndex, _context.ReadValue<Vector2>());
        }
    }

    private static void InputActionStartTriggered(int _playerIndex)
    {
        var bufferEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerUIInputBuffer>()).GetSingletonEntity();
        var playerUIInputBuffer = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerUIInputBuffer>()).GetSingletonBuffer<PlayerUIInputBuffer>();

        var currentPlayerBuffer = playerUIInputBuffer[_playerIndex];
        EntityCommandBuffer ecb = new(Allocator.Temp);
        ecb.RemoveComponent<PlayerUIInputBuffer>(bufferEntity);
        PlayerUIInputBuffer bufferToWrite = new()
        {
            ConfirmClicked = true,
            Choice1Clicked = currentPlayerBuffer.Choice1Clicked,
            Choice2Clicked = currentPlayerBuffer.Choice2Clicked,
            Choice3Clicked = currentPlayerBuffer.Choice3Clicked,
            Choice4Clicked = currentPlayerBuffer.Choice4Clicked
        };

        DynamicBuffer<PlayerUIInputBuffer> newBuffer = ecb.AddBuffer<PlayerUIInputBuffer>(bufferEntity);
        newBuffer.Length = 4;
        for (int i = 0; i < playerUIInputBuffer.Length; i++)
        {
            if (i == _playerIndex)
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

    private static void InputActionChoiceTriggered(int _playerIndex, string _actionName)
    {
        EntityCommandBuffer ecb = new(Allocator.Temp);
        var bufferEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerUIInputBuffer>()).GetSingletonEntity();
        var playerUIInputBuffer = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerUIInputBuffer>()).GetSingletonBuffer<PlayerUIInputBuffer>();
        ecb.RemoveComponent<PlayerUIInputBuffer>(bufferEntity);
        PlayerUIInputBuffer bufferToWrite;
        switch (_actionName)
        {
            case "Choice1":
                bufferToWrite = new PlayerUIInputBuffer()
                {
                    ConfirmClicked = playerUIInputBuffer[_playerIndex].ConfirmClicked,
                    Choice1Clicked = true,
                    Choice2Clicked = false,
                    Choice3Clicked = false,
                    Choice4Clicked = false,
                };
                break;
            case "Choice2":
                bufferToWrite = new PlayerUIInputBuffer()
                {
                    ConfirmClicked = playerUIInputBuffer[_playerIndex].ConfirmClicked,
                    Choice1Clicked = false,
                    Choice2Clicked = true,
                    Choice3Clicked = false,
                    Choice4Clicked = false,
                };
                break;
            case "Choice3":
                bufferToWrite = new PlayerUIInputBuffer()
                {
                    ConfirmClicked = playerUIInputBuffer[_playerIndex].ConfirmClicked,
                    Choice1Clicked = false,
                    Choice2Clicked = false,
                    Choice3Clicked = true,
                    Choice4Clicked = false,
                };
                break;
            case "Choice4":
                bufferToWrite = new PlayerUIInputBuffer()
                {
                    ConfirmClicked = playerUIInputBuffer[_playerIndex].ConfirmClicked,
                    Choice1Clicked = false,
                    Choice2Clicked = false,
                    Choice3Clicked = false,
                    Choice4Clicked = true,
                };
                break;
            default:
                bufferToWrite = new PlayerUIInputBuffer()
                {
                    ConfirmClicked = playerUIInputBuffer[_playerIndex].ConfirmClicked,
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
            if (i == _playerIndex)
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

    private static void InputActionMovementTriggered(int _playerIndex, float2 _newValue)
    {
        var bufferEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerCombatInputBuffer>()).GetSingletonEntity();
        var playerCombatInputBuffer = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerCombatInputBuffer>()).GetSingletonBuffer<PlayerCombatInputBuffer>();

        var currentPlayerBuffer = playerCombatInputBuffer[_playerIndex];
        EntityCommandBuffer ecb = new(Allocator.Temp);
        ecb.RemoveComponent<PlayerCombatInputBuffer>(bufferEntity);
        PlayerCombatInputBuffer bufferToWrite = new()
        {
            MovementInput = _newValue,
            ShootingDirectionInput = currentPlayerBuffer.ShootingDirectionInput,
        };

        DynamicBuffer<PlayerCombatInputBuffer> newBuffer = ecb.AddBuffer<PlayerCombatInputBuffer>(bufferEntity);
        newBuffer.Length = 4;
        for (int i = 0; i < playerCombatInputBuffer.Length; i++)
        {
            if (i == _playerIndex)
            {
                newBuffer[i] = bufferToWrite;
            }
            else
            {
                newBuffer[i] = playerCombatInputBuffer[i];
            }
        }
        ecb.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
    }

    private static void InputActionShootingTriggered(int _playerIndex, float2 _newValue)
    {
        var bufferEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerCombatInputBuffer>()).GetSingletonEntity();
        var playerCombatInputBuffer = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<PlayerCombatInputBuffer>()).GetSingletonBuffer<PlayerCombatInputBuffer>();

        var currentPlayerBuffer = playerCombatInputBuffer[_playerIndex];
        EntityCommandBuffer ecb = new(Allocator.Temp);
        ecb.RemoveComponent<PlayerCombatInputBuffer>(bufferEntity);
        PlayerCombatInputBuffer bufferToWrite = new()
        {
            MovementInput = currentPlayerBuffer.MovementInput,
            ShootingDirectionInput = _newValue,
        };

        DynamicBuffer<PlayerCombatInputBuffer> newBuffer = ecb.AddBuffer<PlayerCombatInputBuffer>(bufferEntity);
        newBuffer.Length = 4;
        for (int i = 0; i < playerCombatInputBuffer.Length; i++)
        {
            if (i == _playerIndex)
            {
                newBuffer[i] = bufferToWrite;
            }
            else
            {
                newBuffer[i] = playerCombatInputBuffer[i];
            }
        }
        ecb.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
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
    public bool ConfirmClicked;
    public bool Choice1Clicked;
    public bool Choice2Clicked;
    public bool Choice3Clicked;
    public bool Choice4Clicked;
}
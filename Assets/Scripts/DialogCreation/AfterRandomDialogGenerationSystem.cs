using System;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

public struct AfterRandomDialogGenerationTag : IComponentData { }

[BurstCompile]
public partial class AfterRandomDialogGenerationSystem : SystemBase
{
    private static Action m_OnDialogsCreated;
    public static bool m_IsCreated = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        m_OnDialogsCreated = null;
        m_IsCreated = false;
    }

    public static void RegisterDialogCreatedReceiver(Action _onDialogCreated)
    {
        if (m_IsCreated)
        {
            _onDialogCreated?.Invoke();
        }
        else
        {
            m_OnDialogsCreated = _onDialogCreated;
        }
    }

    [BurstCompile]
    protected override void OnCreate()
    {
        RequireForUpdate<AfterRandomDialogGenerationTag>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        m_OnDialogsCreated?.Invoke();
        m_IsCreated = true;
        Enabled = false;
    }
}
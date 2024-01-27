using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class DialogCanvasHandler : MonoBehaviour
{
    public static DialogCanvasHandler Instance;

    [SerializeField]
    private List<DialogChoice> m_DialogChoices;

    [SerializeField]
    private LocalizedStringTable m_DialogTable;
    [SerializeField]
    private LocalizedStringTable m_DialogChoicesTable;
    [SerializeField]
    private LocalizedStringTable m_DialogChoiceTypesTable;

    [SerializeField]
    private LocalizeStringEvent m_FinalDialogLoca;

    private System.Random rng = new();

    private void Awake()
    {
        DialogCanvasHandler instance = this;
        AfterRandomDialogGenerationSystem.RegisterDialogCreatedReceiver(OnDialogCreated);
    }

    private void OnDialogCreated()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entity = entityManager.CreateEntityQuery(typeof(DialogReturnValue_Data)).GetSingletonEntity();
        GenerateDialog(entityManager, entity);
        GenerateChoices(entityManager, entity);
    }

    private void GenerateDialog(EntityManager _entityManager, Entity _entity)
    {
        var dialogEntry = _entityManager.GetComponentData<DialogReturnValue_Data>(_entity).DialogEntryKey;
        m_DialogTable.GetTableAsync().Completed += x =>
        {
            m_FinalDialogLoca.SetEntry(x.Result.GetEntry(dialogEntry).Key);
        };
    }

    private void GenerateChoices(EntityManager _entityManager, Entity _entity)
    {
        var dialogChoicesEnemyBuffer = _entityManager.GetBuffer<DialogChoicesEnemyReturnBuffer>(_entity);
        var dialogChoicesLocationBuffer = _entityManager.GetBuffer<DialogChoicesLocationReturnBuffer>(_entity);
        var dialogChoicesShaderBuffer = _entityManager.GetBuffer<DialogChoicesShaderReturnBuffer>(_entity);
        var dialogChoicesMusicBuffer = _entityManager.GetBuffer<DialogChoicesMusicReturnBuffer>(_entity);

        long[][] dialogChoicesKeys = new long[4][];
        dialogChoicesKeys[0] = new long[4];
        dialogChoicesKeys[1] = new long[4];
        dialogChoicesKeys[2] = new long[4];
        dialogChoicesKeys[3] = new long[4];

        for (int i = 0; i < dialogChoicesEnemyBuffer.Length; i++)
        {
            dialogChoicesKeys[0][i] = dialogChoicesEnemyBuffer[i].Value;
        }
        for (int i = 0; i < dialogChoicesEnemyBuffer.Length; i++)
        {
            dialogChoicesKeys[1][i] = dialogChoicesLocationBuffer[i].Value;
        }
        for (int i = 0; i < dialogChoicesEnemyBuffer.Length; i++)
        {
            dialogChoicesKeys[2][i] = dialogChoicesShaderBuffer[i].Value;
        }
        for (int i = 0; i < dialogChoicesEnemyBuffer.Length; i++)
        {
            dialogChoicesKeys[3][i] = dialogChoicesMusicBuffer[i].Value;
        }

        ShuffelPlayerCardType();

        m_DialogChoicesTable.GetTableAsync().Completed += x =>
        {
            string dialogChoiceTypesLocaKey = "";
            string[] dialogChoicesLocaKey;
            for (int i = m_DialogChoices.Count - 1; i >= 0; i--)
            {
                dialogChoicesLocaKey = new string[4];
                for (int j = 0; j < dialogChoicesKeys[i].Length; j++)
                {
                    dialogChoicesLocaKey[j] = x.Result.GetEntry(dialogChoicesKeys[i][j]).Key;
                    if (j == 0)
                    {
                        dialogChoiceTypesLocaKey = dialogChoicesLocaKey[0].Split('_')[0];
                    }
                }
                m_DialogChoices[i].Setup(dialogChoicesLocaKey, dialogChoiceTypesLocaKey);
            }
        };
    }

    private void ShuffelPlayerCardType()
    {
        int n = m_DialogChoices.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            DialogChoice choice = m_DialogChoices[k];
            m_DialogChoices[k] = m_DialogChoices[n];
            m_DialogChoices[n] = choice;
        }
    }

    public static void BuildFinalDialog()
    {
        Instance.BuildFinalDialog_Internal();
    }

    private void BuildFinalDialog_Internal()
    {
        //m_FinalDialogLoca.text = "Final Dialog";
        Invoke(nameof(StartBattle), 3f);
    }

    private void StartBattle()
    {
        //m_FinalDialogLoca.text = "Switch to battle scene";
    }
}

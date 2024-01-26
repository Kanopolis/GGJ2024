using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class DialogChoicesDatabase : MonoBehaviour
{
    [SerializeField]
    private StringTableCollection m_DialogChoiceTable;

    private static List<string> m_PossibleDialogChoice1EntryKeys;
    private static List<string> m_PossibleDialogChoice2EntryKeys;
    private static List<string> m_PossibleDialogChoice3EntryKeys;
    private static List<string> m_PossibleDialogChoice4EntryKeys;

    private void Start()
    {
        foreach (var item in (m_DialogChoiceTable.GetTable("en") as StringTable).Values)
        {
            if (item.Key.StartsWith("Choice1"))
            {
                m_PossibleDialogChoice1EntryKeys.Add(item.Key);
            }
            else if (item.Key.StartsWith("Choice2"))
            {
                m_PossibleDialogChoice2EntryKeys.Add(item.Key);
            }
            else if (item.Key.StartsWith("Choice3"))
            {
                m_PossibleDialogChoice3EntryKeys.Add(item.Key);
            }
            else if (item.Key.StartsWith("Choice4"))
            {
                m_PossibleDialogChoice4EntryKeys.Add(item.Key);
            }
        }
    }

    public class Baker : Baker<DialogChoicesDatabase>
    {
        public override void Bake(DialogChoicesDatabase authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        }
    }
}

public struct DialogDatabase_Data : IComponentData
{
    public readonly NativeArray<FixedString512Bytes> m_PossibleDialogEntryKeys;
    public readonly NativeArray<FixedString512Bytes> m_PossibleDialogChoice1EntryKeys;
    public readonly NativeArray<FixedString512Bytes> m_PossibleDialogChoice2EntryKeys;
    public readonly NativeArray<FixedString512Bytes> m_PossibleDialogChoice3EntryKeys;
    public readonly NativeArray<FixedString512Bytes> m_PossibleDialogChoice4EntryKeys;

}
using Unity.Entities;
#if UNITY_EDITOR
using UnityEditor.Localization;
#endif
using UnityEngine;
using UnityEngine.Localization.Tables;

public class DialogDatabase : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private StringTableCollection m_DialogTable;

    [SerializeField]
    private StringTableCollection m_DialogChoiceTable;

    public class Baker : Baker<DialogDatabase>
    {
        public override void Bake(DialogDatabase authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            var BufferDialogs = AddBuffer<DialogBuffer>(entity);
            var BufferDialogChoices1 = AddBuffer<DialogChoicesEnemyBuffer>(entity);
            var BufferDialogChoices2 = AddBuffer<DialogChoicesLocationBuffer>(entity);
            var BufferDialogChoices3 = AddBuffer<DialogChoicesShaderBuffer>(entity);
            var BufferDialogChoices4 = AddBuffer<DialogChoicesMusicBuffer>(entity);
            var BufferDialogChoicesReturn1 = AddBuffer<DialogChoicesEnemyReturnBuffer>(entity);
            var BufferDialogChoicesReturn2 = AddBuffer<DialogChoicesLocationReturnBuffer>(entity);
            var BufferDialogChoicesReturn3 = AddBuffer<DialogChoicesShaderReturnBuffer>(entity);
            var BufferDialogChoicesReturn4 = AddBuffer<DialogChoicesMusicReturnBuffer>(entity);

            foreach (var item in (authoring.m_DialogTable.GetTable("en") as StringTable).Values)
            {
                BufferDialogs.Add(new DialogBuffer() { Value = item.KeyId });
            }
            foreach (var item in (authoring.m_DialogChoiceTable.GetTable("en") as StringTable).Values)
            {
                if (item.Key.StartsWith("Enemy"))
                {
                    BufferDialogChoices1.Add(new DialogChoicesEnemyBuffer() { Value = item.KeyId });
                }
                else if (item.Key.StartsWith("Location"))
                {
                    BufferDialogChoices2.Add(new DialogChoicesLocationBuffer() { Value = item.KeyId });
                }
                else if (item.Key.StartsWith("Shader"))
                {
                    BufferDialogChoices3.Add(new DialogChoicesShaderBuffer() { Value = item.KeyId });
                }
                else if (item.Key.StartsWith("Music"))
                {
                    BufferDialogChoices4.Add(new DialogChoicesMusicBuffer() { Value = item.KeyId });
                }
            }

            for (var i = 0; i < 4; i++)
            {
                BufferDialogChoicesReturn1.Add(new());
            }
            for (var i = 0; i < 4; i++)
            {
                BufferDialogChoicesReturn2.Add(new());
            }
            for (var i = 0; i < 4; i++)
            {
                BufferDialogChoicesReturn3.Add(new());
            }
            for (var i = 0; i < 4; i++)
            {
                BufferDialogChoicesReturn4.Add(new());
            }

            AddComponent<DialogReturnValue_Data>(entity);
        }
    }
#endif
}

public struct DialogReturnValue_Data : IComponentData
{
    public bool Created;
    public long DialogEntryKey;
}
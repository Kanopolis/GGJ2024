using System;
using TMPro;
using UnityEngine;

public class DialogCanvasHandler : MonoBehaviour
{
    public static DialogCanvasHandler Instance;

    [SerializeField]
    private DialogChoicesHandler DialogChoicesHandler;

    [SerializeField]
    private TextMeshProUGUI m_FinalDialog;

    private void Awake()
    {
        DialogCanvasHandler instance = this;
    }

    private void Start()
    {
        GenerateDialog();
        GenerateChoices();
    }

    private void GenerateDialog()
    {
        m_FinalDialog.text = "GeneratedPreFinalDialog";
    }

    private void GenerateChoices()
    {
    }

    public static void BuildFinalDialog()
    {
        Instance.BuildFinalDialog_Internal();
    }

    private void BuildFinalDialog_Internal()
    {
        m_FinalDialog.text = "Final Dialog";
        Invoke(nameof(StartBattle), 3f);
    }

    private void StartBattle()
    {
        m_FinalDialog.text = "Switch to battle scene";
    }
}

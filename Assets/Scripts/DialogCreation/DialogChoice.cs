using System;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class DialogChoice : MonoBehaviour
{
    [SerializeField]
    private DialogCard[] m_DialogCards;

    public string ChoiceType { get; private set; }
    public string[] ChoiceString { get; private set; }

    private int m_CurrentSelectedCard = -1;

    public void Setup(string[] _dialogChoicesLocaKeys, string _dialogChoiceTypesLocaKey)
    {
        ChoiceType = _dialogChoiceTypesLocaKey;
        ChoiceString = new string[m_DialogCards.Length];
        for (int i = 0; i < m_DialogCards.Length; i++)
        {
            ChoiceString[i] = m_DialogCards[i].SetupAndGetText(_dialogChoiceTypesLocaKey, _dialogChoicesLocaKeys[i]);
        }
    }

    internal void Select(int selectedId)
    {
        if (m_CurrentSelectedCard != -1)
        {
            m_DialogCards[m_CurrentSelectedCard].Select(false);
        }
        m_CurrentSelectedCard = selectedId;
        m_DialogCards[selectedId].Select(true);
    }

    internal void Lock()
    {
        if (m_CurrentSelectedCard != -1)
        {
            m_DialogCards[m_CurrentSelectedCard].Lock();
        }
    }
}

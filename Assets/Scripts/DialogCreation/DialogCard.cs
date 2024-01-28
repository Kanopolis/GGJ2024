using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class DialogCard : MonoBehaviour
{
    [SerializeField]
    private LocalizedString m_SelectedLoca, m_LockedLoca;

    [SerializeField]
    private LocalizeStringEvent m_StateLoca;

    [SerializeField]
    private LocalizeStringEvent m_CardTypeLoca;
    [SerializeField]
    private LocalizeStringEvent m_CardTextLoca;

    private void Awake()
    {
        m_StateLoca.gameObject.SetActive(false);
    }

    public string SetupAndGetText(string _cardTypeLocaEntryKey, string _cardTextLocaEntryKey)
    {
        m_CardTypeLoca.SetEntry(_cardTypeLocaEntryKey);
        m_CardTextLoca.SetEntry(_cardTextLocaEntryKey);

        return m_CardTextLoca.GetComponent<TextMeshProUGUI>().text;
    }

    internal void Select(bool _active)
    {
        m_StateLoca.gameObject.SetActive(_active);
        m_StateLoca.StringReference = m_SelectedLoca;
    }

    internal void Lock()
    {
        m_StateLoca.StringReference = m_LockedLoca;
    }
}

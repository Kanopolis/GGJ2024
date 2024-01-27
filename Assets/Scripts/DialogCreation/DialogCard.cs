using UnityEngine;
using UnityEngine.Localization.Components;

public class DialogCard : MonoBehaviour
{
    [SerializeField]
    private LocalizeStringEvent m_CardTypeLoca;
    [SerializeField]
    private LocalizeStringEvent m_CardTextLoca;

    public void Setup(string _cardTypeLocaEntryKey, string _cardTextLocaEntryKey)
    {
        m_CardTypeLoca.SetEntry(_cardTypeLocaEntryKey);
        m_CardTextLoca.SetEntry(_cardTextLocaEntryKey);
    }
}

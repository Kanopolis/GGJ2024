using UnityEngine;
using UnityEngine.Localization;

public class DialogChoice : MonoBehaviour
{
    [SerializeField]
    private DialogCard[] m_DialogCards;

    public void Setup(string[] _dialogChoicesLocaKeys, string _dialogChoiceTypesLocaKey)
    {
        for (int i = 0; i < m_DialogCards.Length; i++)
        {
            m_DialogCards[i].Setup(_dialogChoiceTypesLocaKey, _dialogChoicesLocaKeys[i]);
        }
    }
}

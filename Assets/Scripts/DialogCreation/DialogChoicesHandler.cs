using UnityEngine;
using UnityEngine.Localization;

public class DialogChoicesHandler : MonoBehaviour
{
    [SerializeField]
    private LocalizedString[] m_BattleDialogs;
    [SerializeField]
    private LocalizedString m_EndDialog;

    private void OnEnable()
    {
        switch (GameManager.CurrentGameStateID)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }
}

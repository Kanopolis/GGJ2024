using UnityEngine;
using UnityEngine.UI;

public class PlayerRegistrationScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ReadyTextObject;
    [SerializeField]
    private Image m_PlayerSprite;

    private void Start()
    {
        m_ReadyTextObject.SetActive(false);
        m_PlayerSprite.gameObject.SetActive(false);
    }

    public void PlayerJoined(Color color)
    {
        m_ReadyTextObject.SetActive(true);
        m_PlayerSprite.color = color;
        m_PlayerSprite.gameObject.SetActive(true);
    }
}

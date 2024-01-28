using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager m_Instance;

    [SerializeField]
    private PlayerRegistrationScreen[] m_RegistrationScreens;

    private static int m_PlayerNumber;
    public static int PlayerNumber => m_PlayerNumber;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        m_PlayerNumber = 0;
    }

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(m_Instance);
        }
        m_Instance = this;
        DontDestroyOnLoad(this);
    }

    public void OnPlayerJoined()
    {
        m_RegistrationScreens[m_PlayerNumber].PlayerJoined(GameManager.PlayerColors[m_PlayerNumber]);
        ++m_PlayerNumber;
    }

    public void OnPlayerLeft()
    {
        Debug.LogError("PlayerLeft");
    }

    public static void EndGame()
    {
        Destroy(m_Instance.gameObject);
    }
}

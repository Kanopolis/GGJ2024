using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private PlayerRegistrationScreen[] m_RegistrationScreens;

    private static int m_PlayerNumber;
    public static int PlayerNumber => m_PlayerNumber;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        m_PlayerNumber = 0;
    }

    public void OnPlayerJoined()
    {
        m_RegistrationScreens[m_PlayerNumber++].PlayerJoined();
    }

    public void OnPlayerLeft()
    {
        Debug.LogError("PlayerLeft");
    }
}

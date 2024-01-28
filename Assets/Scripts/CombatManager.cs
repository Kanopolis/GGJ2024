using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    private static CombatManager instance;

    [SerializeField]
    private LocalizeStringEvent m_EndLocaEvent;

    [SerializeField]
    private LocalizedString m_WinLoca, m_LoseLoca;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static void CallPlayerDeath()
    {
        Debug.Log("Player is dead! Combat is over...");
        instance.Lose();
    }

    private void Lose()
    {
        m_EndLocaEvent.StringReference = m_LoseLoca;
        m_EndLocaEvent.gameObject.SetActive(true);
        Invoke(nameof(BackToMainMenu), 3f);
    }

    public static void CallAllEnemiesDeath()
    {
        instance.Won();
    }

    private void Won()
    {
        m_EndLocaEvent.StringReference = m_WinLoca;
        m_EndLocaEvent.gameObject.SetActive(true);
        Invoke(nameof(BackToMainMenu), 3f);
    }

    private void BackToMainMenu()
    {
        GameManager.EndGame();
    }

    public static void CallSpawnEnd()
    {
        Debug.Log("Spawning is done!");
    }
}
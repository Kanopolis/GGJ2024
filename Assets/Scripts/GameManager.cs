using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int m_CurrentGameStateID = 0;
    public static int CurrentGameStateID => m_CurrentGameStateID;
}

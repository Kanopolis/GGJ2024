using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Color[] m_PlayerColors;

    public static Color[] PlayerColors;

    private void Awake()
    {
        PlayerColors = m_PlayerColors;
    }
}

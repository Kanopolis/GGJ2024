using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.InputSystem.PlayerInput m_PlayerInput;
    private int m_PlayerIndex => m_PlayerInput.playerIndex;

    private void Start()
    {
        PlayerInputComponent.RegisterPlayer(m_PlayerIndex, m_PlayerInput);
    }
}

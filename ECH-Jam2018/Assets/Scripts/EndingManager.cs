namespace GameJam
{
    using System;
    using UnityEngine;

    public sealed class EndingManager : MonoBehaviour
    {
        [SerializeField]
        GameController m_gameController;
        [SerializeField]
        GameState m_gameState;

        void OnEnable()
        {
            m_gameController.GameOver();
        }
        void OnDisable()
        {
        }

    }
}

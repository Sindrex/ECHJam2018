namespace GameJam
{
    using System;
    using UnityEngine;

    public sealed class GameplayManager : MonoBehaviour
    {
        [SerializeField]
        GameController m_gameController;
        [SerializeField]
        GameState m_gameState;

        void OnEnable()
        {
            m_gameState.IndoorChanged += AdvancePhase;
            m_gameState.StoppedTalking += TalkToSelfIfOver;
        }

        void OnDisable()
        {
            m_gameState.IndoorChanged -= AdvancePhase;
            m_gameState.StoppedTalking -= TalkToSelfIfOver;
        }

        [NonSerialized]
        bool m_talkedToSelf = false;
        void TalkToSelfIfOver()
        {
            if (m_talkedToSelf) return;
            if (m_gameState.IsPhaseOver())
            {
                m_talkedToSelf = true;
                m_gameController.TalkToSelf();
            }
        }

        void AdvancePhase()
        {
            // The player interacted with the door to go out
            //if (m_gameState.ActiveHouse.IsHome && !m_gameState.IsIndoor) m_gameController.TalkToSelf();

            // The player is back home and talked to everybody
            if(m_gameState.ActiveHouse.IsHome && m_gameState.IsPhaseOver() && m_gameState.IsIndoor)
            {
                m_gameController.AdvancePhase();
            }
        }
    }
}

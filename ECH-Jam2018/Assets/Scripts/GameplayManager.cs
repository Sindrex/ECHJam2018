namespace GameJam
{
    using UnityEngine;

    public sealed class GameplayManager : MonoBehaviour
    {
        [SerializeField]
        GameController m_gameController;
        [SerializeField]
        GameState m_gameState;

        void OnEnable()
        {
            m_gameState.IndoorChanged += TalkToSelf;
        }
        void OnDisable()
        {
            m_gameState.IndoorChanged -= TalkToSelf;
        }

        void TalkToSelf()
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

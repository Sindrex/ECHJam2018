namespace GameJam
{
    using UnityEngine;

    public sealed class IntroductionManager : MonoBehaviour
    {
        [SerializeField]
        GameController m_gameController;
        [SerializeField]
        GameState m_gameState;

        void OnEnable()
        {
            m_gameState.IndoorChanged += ExitHouseIfComplete;
            m_gameController.TalkToSelf();
        }
        void OnDisable()
        {
            m_gameState.IndoorChanged -= ExitHouseIfComplete;
        }

        void ExitHouseIfComplete()
        {
            if (!m_gameState.IsPhaseOver()) return;
            m_gameController.ExitHouse();
            m_gameController.AdvancePhase();
        }
    }
}

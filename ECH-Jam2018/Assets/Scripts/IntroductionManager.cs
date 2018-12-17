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
            m_gameState.StoppedTalking += AdvancePhase;
            m_gameController.TalkToSelf();
        }


        void OnDisable()
        {
            m_gameState.StoppedTalking -= AdvancePhase;
        }

        void AdvancePhase()
        {
            if (!m_gameState.IsPhaseOver()) return;
            m_gameController.AdvancePhase();
        }
    }
}

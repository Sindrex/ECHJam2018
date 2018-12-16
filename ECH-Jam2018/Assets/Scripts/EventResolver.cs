namespace GameJam
{
    using UnityEngine;

    public sealed class EventResolver : MonoBehaviour
    {
        [SerializeField]
        GameState m_gameState;
        [SerializeField]
        GameController m_gameController;
        [SerializeField]
        SoundController m_soundController;

        void OnEnable()
        {
            m_gameState.EventHappened += ResolveEvent;
        }

        public void ForceEvent(string currentEvent)
        {
            ResolveEvent(currentEvent);
        }
        void ResolveEvent(string currentEvent)
        {
            if (currentEvent.StartsWith("SFX("))
            {
                var soundName = currentEvent.Substring(4, currentEvent.Length - 5);
                m_soundController.playAudio(soundName);
            }
            //else if (currentEvent == "ENTER_HOUSE")
            //{
            //    m_gameController.EnterHome();
            //}
            //else if (currentEvent == "EXIT_HOUSE")
            //{
            //    m_gameController.ExitHome();
            //}
            else
            {
                Debug.LogWarningFormat("Unknown event {0}", currentEvent);
            }
        }
    }
}

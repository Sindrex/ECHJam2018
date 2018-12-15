namespace GameJam
{
    using UnityEngine;

    public sealed class EventResolver : MonoBehaviour
    {
        [SerializeField]
        GameState m_gameState;
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
            //else if (eventName == "SUCCESS")
            //{
            //    // Do something
            //}
            else
            {
                Debug.LogWarningFormat("Unknown event {0}", currentEvent);
            }
        }
    }
}

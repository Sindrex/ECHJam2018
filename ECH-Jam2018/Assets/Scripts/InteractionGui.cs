namespace GameJam
{
    using System;
    using TMPro;
    using UnityEngine;

    public sealed class InteractionGui : MonoBehaviour
    {
        [SerializeField]
        CanvasGroup m_canvas;
        [SerializeField]
        TextMeshProUGUI m_interaction;
        [SerializeField]
        float m_duration = 0.7f;
        [SerializeField]
        GameState m_gameState;

        public bool IsAnimating
        {
            get { return enabled; }
            private set { enabled = value; }
        }

        [NonSerialized]
        float m_startAlpha = 0f;
        [NonSerialized]
        float m_targetAlpha = 0f;

        public void Show(string interaction)
        {
            m_interaction.text = interaction;
            m_startAlpha = m_canvas.alpha;
            m_targetAlpha = 1f;
        }
        public void Hide()
        {
            m_startAlpha = m_canvas.alpha;
            m_targetAlpha = 0f;
        }

        void Fade()
        {
            float delta = Time.deltaTime * (m_targetAlpha - m_startAlpha) / m_duration;
            m_canvas.alpha += delta;
        }

        void Update()
        {
            Fade();
        }

        // Self manage
        void OnEnable()
        {
            m_gameState.ActiveCharacterChanged += ShowTalkInteractionIfInRange;
            m_gameState.StartedTalking += Hide;
            m_gameState.StoppedTalking += ShowTalkInteractionIfInRange;

            m_gameState.ActiveHouseChanged += ShowHouseInteractionIfInRange;
        }

        void OnDisable()
        {
            m_gameState.ActiveCharacterChanged -= ShowTalkInteractionIfInRange;
            m_gameState.StartedTalking -= Hide;
            m_gameState.StoppedTalking -= ShowTalkInteractionIfInRange;

            m_gameState.ActiveHouseChanged -= ShowHouseInteractionIfInRange;
        }

        void ShowTalkInteractionIfInRange()
        {
            if (m_gameState.IgnoreInput) Hide();
            else if (m_gameState.ActiveCharacter != null) Show("Talk to " + m_gameState.ActiveCharacter.Name); 
            else Hide();
        }

        void ShowHouseInteractionIfInRange()
        {
            if (m_gameState.IgnoreInput) Hide();
            else if (m_gameState.ActiveHouse != null)
            {
                if (m_gameState.ActiveHouse.IsHome) Show("Enter home");
                else Show("Knock the door");
            }
            else Hide();
        }
    }
}

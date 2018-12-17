﻿namespace GameJam
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
        GameController m_gameController;
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
            m_gameController.EnablingChanged += EnablingChanged;
            m_gameState.ActiveCharacterChanged += ShowIfInRange;
            m_gameState.StartedTalking += Hide;
            m_gameState.StoppedTalking += ShowIfInRange;

            m_gameState.IndoorChanged += ShowIfInRange;
            m_gameState.ActiveHouseChanged += ShowIfInRange;
        }

        void OnDisable()
        {
            m_gameController.EnablingChanged -= EnablingChanged;
            m_gameState.ActiveCharacterChanged -= ShowIfInRange;
            m_gameState.StartedTalking -= Hide;
            m_gameState.StoppedTalking -= ShowIfInRange;

            m_gameState.IndoorChanged -= ShowIfInRange;
            m_gameState.ActiveHouseChanged -= ShowIfInRange;
        }

        void EnablingChanged()
        {
            if (m_gameState.IgnoreInput || m_gameController.TemporarilyDisabled) Hide();
        }

        void ShowIfInRange()
        {
            if (m_gameState.IsIndoor) ShowIndoor();
            else ShowOutdoor();
        }

        void ShowIndoor()
        {
            if (m_gameState.IgnoreInput || m_gameController.TemporarilyDisabled) Hide();
            else if (m_gameState.ActiveCharacter != null) Show("Talk");
            else if (m_gameState.ActiveHouse != null) Show(GetHouseInteraction());
            else Hide();
        }
        void ShowOutdoor()
        {
            if (m_gameState.IgnoreInput || m_gameController.TemporarilyDisabled) Hide();
            else if (m_gameState.ActiveHouse != null) Show(GetHouseInteraction());
            else if (m_gameState.ActiveCharacter != null) Show("Talk");
            else Hide();
        }

        string GetHouseInteraction()
        {
            if (m_gameState.ActiveHouse.IsHome)
            {
                if(m_gameState.IsIndoor) return "Exit home";
                else return "Enter home";
            }
            else return "Knock the door";
        }
    }
}

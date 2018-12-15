namespace GameJam
{
    using System;
    using System.Collections;
    using UnityEngine;

    public sealed class GameOverGui : MonoBehaviour
    {
        [SerializeField]
        float m_duration = 0.7f;

        [SerializeField]
        CanvasGroup m_canvas;
        [SerializeField]
        CanvasGroup m_textCanvas;

        public bool IsAnimating
        {
            get { return enabled; }
            private set { enabled = value; }
        }

        [NonSerialized]
        float m_startAlpha = 0f;
        [NonSerialized]
        float m_targetAlpha = 0f;
        [NonSerialized]
        float m_startTextAlpha = 0f;
        [NonSerialized]
        float m_targetTextAlpha = 0f;

        public IEnumerator ShowAsync()
        {
            gameObject.SetActive(true);
            IsAnimating = true;

            m_startAlpha = 0f;
            m_targetAlpha = 1f;
            while (IsFading)
            {
                yield return null;
            }

            m_startTextAlpha = 0f;
            m_targetTextAlpha = 1f;
            while (IsFadingText)
            {
                yield return null;
            }

            IsAnimating = false;
        }

        bool IsFadingText
        {
            get { return !Mathf.Approximately(m_textCanvas.alpha, m_targetTextAlpha); }
        }
        bool IsFading
        {
            get { return !Mathf.Approximately(m_canvas.alpha, m_targetAlpha); }
        }
        void Fade()
        {
            float delta = Time.deltaTime * (m_targetAlpha - m_startAlpha) / m_duration;
            m_canvas.alpha += delta;
        }
        void FadeText()
        {
            float delta = Time.deltaTime * (m_targetTextAlpha - m_startTextAlpha) / m_duration;
            m_textCanvas.alpha += delta;
        }
        void Update()
        {
            if (IsFading) Fade();
            if (IsFadingText) FadeText();
        }
    }
}

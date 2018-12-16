namespace GameJam
{
    using System;
    using System.Collections;
    using UnityEngine;

    public sealed class Fader : MonoBehaviour
    {
        [SerializeField]
        float m_duration = 0.7f;

        [SerializeField]
        CanvasGroup m_canvas;

        public bool IsAnimating
        {
            get { return enabled; }
            private set { enabled = value; }
        }

        [NonSerialized]
        float m_startAlpha = 0f;
        [NonSerialized]
        float m_targetAlpha = 0f;

        public IEnumerator ShowAsync()
        {
            gameObject.SetActive(true);

            m_startAlpha = m_canvas.alpha;
            m_targetAlpha = 1f;
            IsAnimating = true;
            yield return StartCoroutine(FadeAsync());
        }
        public IEnumerator HideAsync()
        {
            m_startAlpha = m_canvas.alpha;
            m_targetAlpha = 0f;
            IsAnimating = true;
            yield return StartCoroutine(FadeAsync());
            gameObject.SetActive(false);
        }

        IEnumerator FadeAsync()
        {
            while (IsFading)
            {
                yield return null;
            }
            IsAnimating = false;
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
        void Update()
        {
            if (IsFading) Fade();
        }
    }
}

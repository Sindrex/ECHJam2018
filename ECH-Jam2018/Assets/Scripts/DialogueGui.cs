namespace GameJam
{
    using System;
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class DialogueGui : MonoBehaviour
    {
        [SerializeField]
        CloseUp m_closeup;
        [SerializeField]
        TextMeshProUGUI m_name;
        [SerializeField]
        TextMeshProUGUI m_words;
        [SerializeField]
        GameObject m_endOfLine;
        [SerializeField]
        CanvasGroup m_canvas;
        [SerializeField]
        float m_duration = 0.7f;

        public bool IsAnimating
        {
            get { return enabled; }
            private set { enabled = value; }
        }

        [NonSerialized]
        float m_startAlpha = 0f;
        [NonSerialized]
        float m_targetAlpha = 1f;

        [NonSerialized]
        float m_currentLength = 0;
        [NonSerialized]
        string m_targetText;

        public IEnumerator ShowLineAsync(CloseUp closeup, string name, string line)
        {
            m_endOfLine.SetActive(false);
            gameObject.SetActive(true);

            if(closeup != null)
            {
                m_closeup.gameObject.SetActive(true);
                m_closeup.CopyFrom(closeup);
            }
            else
            {
                m_closeup.gameObject.SetActive(false);
            }
            m_name.text = name;

            m_words.text = "";
            m_targetText = line;
            m_currentLength = 0;

            m_startAlpha = 0f;
            m_targetAlpha = 1f;
            IsAnimating = true;
            while(IsFading || IsWritingLetters)
            {
                yield return null;
            }
            m_endOfLine.SetActive(true);
            IsAnimating = false;
        }

        public IEnumerator HideAsync()
        {
            m_startAlpha = 1f;
            m_targetAlpha = 0f;
            IsAnimating = true;
            while (IsFading)
            {
                yield return null;
            }
            IsAnimating = false;
            gameObject.SetActive(false);
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
        bool IsWritingLetters
        {
            get { return (int)m_currentLength != m_targetText.Length; }
        }
        void WriteLetters()
        {
            float deltaLength = Time.deltaTime * ((float)m_targetText.Length) / m_duration;
            m_currentLength += deltaLength;
            m_currentLength = Mathf.Clamp(m_currentLength, 0, m_targetText.Length);
            m_words.text = m_targetText.Substring(0, (int)m_currentLength);

            if ((int)m_currentLength == m_targetText.Length)
            {
                m_currentLength = (int)m_currentLength;
            }
        }
        void Update()
        {
            if (IsFading) Fade();
            if (IsWritingLetters) WriteLetters();
        }
    }
}

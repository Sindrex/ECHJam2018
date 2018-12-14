namespace GameJam
{
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

        public IEnumerator ShowLineAsync(CloseUp closeup, string name, string line)
        {
            m_endOfLine.SetActive(false);
            gameObject.SetActive(true);

            m_closeup.CopyFrom(closeup);
            m_name.text = name;
            m_words.text = line;
            yield return null;
            m_endOfLine.SetActive(true);
        }

        public IEnumerator HideAsync()
        {
            yield return null;
            gameObject.SetActive(false);
        }
    }
}

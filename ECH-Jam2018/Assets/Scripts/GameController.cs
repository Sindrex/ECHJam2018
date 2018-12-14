namespace GameJam
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// This should be the only class ever modifying the GameState
    /// Uses coroutine to allow animations
    /// </summary>
    public sealed class GameController : MonoBehaviour
    {
        [SerializeField]
        DialogueGui m_dialogueGui;
        [SerializeField]
        CharacterManager m_characterManager;
        [SerializeField]
        GameState m_gameState;

        public void TalkTo(string name)
        {
            StartCoroutine(TalkToAsync(name));
        }
        public void AdvanceDialogue()
        {
            StartCoroutine(AdvanceDialogueAsync());
        }

        public IEnumerator TalkToAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) yield break;

            m_gameState.ActiveDialogue = Dialogue.FromAsset(name);
            yield return StartCoroutine(AdvanceDialogueAsync());
        }

        public IEnumerator AdvanceDialogueAsync()
        {
            string line = m_gameState.AdvanceOneLine();
            string speakerName = m_characterManager.GetSpeakerName(line);
            // The default speaker is the one we are talking to
            if (string.IsNullOrEmpty(speakerName)) speakerName = m_gameState.ActiveDialogue.DefaultSpeaker;
            else
            {
                // Strip speaker's name and ": " from the line
                line = line.Substring(speakerName.Length + 2);
            }

            var closeUp = CloseUp.FromAsset(speakerName);
            yield return StartCoroutine(m_dialogueGui.ShowLine(closeUp, speakerName, line));
        }

        /// <summary>
        /// Load test dialogues. To be removed
        /// </summary>
        void Update()
        {
            string name = "";
            if (Input.GetKeyDown(KeyCode.Alpha0)) name = "Adam";
            else if (Input.GetKeyDown(KeyCode.Alpha1)) name = "Bob";

            if (!string.IsNullOrEmpty(name))
            {
                TalkTo(name);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space)) AdvanceDialogue();
        }
    }
}

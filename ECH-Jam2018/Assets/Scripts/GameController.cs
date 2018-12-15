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
        [SerializeField]
        Character m_player;
        [SerializeField]
        SoundController m_soundController;

        void Start()
        {
            m_soundController.playAudio("BGM");
        }

        public void StopTalking()
        {
            StartCoroutine(StopTalkingAsync());
        }
        public IEnumerator StopTalkingAsync()
        {
            if (m_dialogueGui.IsAnimating) yield break;
            m_gameState.ActiveCharacter.ResumeAnimations();
            m_gameState.ActiveDialogue = null;
            yield return StartCoroutine(m_dialogueGui.HideAsync());
        }
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
            if (m_dialogueGui.IsAnimating) yield break;

            m_gameState.ActiveDialogue = Dialogue.FromAsset(name);
            m_gameState.ActiveCharacter.PauseAnimations();
            m_gameState.ActiveCharacter.Face(m_player.transform);
            yield return StartCoroutine(AdvanceDialogueAsync());
        }

        public IEnumerator AdvanceDialogueAsync()
        {
            if (m_dialogueGui.IsAnimating) yield break;

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
            yield return StartCoroutine(m_dialogueGui.ShowLineAsync(closeUp, speakerName, line));
        }

        void Update()
        {
            float distance;
            var character = m_characterManager.GetClosest(m_player.transform.position, out distance);
            if (character != null && distance < 3f) m_gameState.ActiveCharacter = character;
            else m_gameState.ActiveCharacter = null;

            if (Input.GetKeyDown(KeyCode.X))
            {
                // A dialogue is currently open
                if (m_gameState.ActiveDialogue != null)
                {
                    // But we reached the last line, stop it
                    if (m_gameState.IsDialogueOver) StopTalking();
                    // There are more lines, go on
                    else AdvanceDialogue();
                }
                // No dialogue currently open, start one with the closest character
                else if (m_gameState.ActiveCharacter != null) TalkTo(m_gameState.ActiveCharacter.Name);
            }
            else
            {
                // There is no one to interact with
                // TODO: Add unpleasant sound
            }
        }
    }
}

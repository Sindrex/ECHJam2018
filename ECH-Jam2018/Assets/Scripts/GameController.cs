namespace GameJam
{
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// This should be the only class ever modifying the GameState
    /// Uses coroutine to allow animations
    /// </summary>
    public sealed class GameController : MonoBehaviour
    {
        [SerializeField]
        int m_numberOfDialoguesBeforeGameOver;
        public int NumberOfDialoguesBeforeGameOver
        {
            get { return m_numberOfDialoguesBeforeGameOver; }
            set { m_numberOfDialoguesBeforeGameOver = value; }
        }

        [SerializeField]
        DialogueGui m_dialogueGui;
        [SerializeField]
        CharacterManager m_characterManager;
        [SerializeField]
        HouseManager m_houseManager;
        [SerializeField]
        GameState m_gameState;
        [SerializeField]
        Character m_player;
        [SerializeField]
        SoundController m_soundController;
        [SerializeField]
        GameOverGui m_gameOverGui;

        void Start()
        {
            m_soundController.playAudio("BGM");
            m_gameState.GameIsOver += Disable;
        }

        void Disable()
        {
            enabled = false;
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
            if(m_gameState.GetCompletedDialogues() >= m_numberOfDialoguesBeforeGameOver)
            {
                m_gameState.OnGameIsOver();
                yield return StartCoroutine(m_gameOverGui.ShowAsync());
            }
            else
            {
                m_gameState.IgnoreInput = false;
            }
            // Needs to be last or IgnoreInput would have an inconsistent state
            m_gameState.OnStoppedTalking();
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

            m_gameState.OnEventHappened("SFX(START_DIALOGUE)");
            m_gameState.IgnoreInput = true;
            m_gameState.ActiveDialogue = Dialogue.FromAsset(name);
            m_gameState.ActiveCharacter.PauseAnimations();
            m_gameState.ActiveCharacter.Face(m_player.transform);
            m_gameState.OnStartedTalking();
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

            var house = m_houseManager.GetClosest(m_player.transform.position, out distance);
            if (house != null && distance < 3f) m_gameState.ActiveHouse = house;
            else m_gameState.ActiveHouse = null;

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
                // No character in range, play SFX
                else m_gameState.OnEventHappened("SFX(WRONG)");
            }
        }
    }
}

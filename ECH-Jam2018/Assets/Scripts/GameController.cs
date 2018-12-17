namespace GameJam
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

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
        public Character Player
        {
            get { return m_player; }
        }
        [SerializeField]
        GameOverGui m_gameOverGui;
        [NonSerialized]
        ColliderManager m_colliderManager;

        void Awake()
        {
#if !UNITY_EDITOR
            SceneManager.LoadScene("Level", LoadSceneMode.Additive);
#endif
        }

        void Start()
        {
            m_colliderManager = FindObjectOfType<ColliderManager>();
            if (m_colliderManager == null) throw new Exception("Can't find ColliderManager");
            m_gameState.Phase = GamePhase.Introduction;
            EnterHouse();
        }

        public void StopTalking()
        {
            StartCoroutine(StopTalkingAsync());
        }
        public IEnumerator StopTalkingAsync()
        {
            if (m_dialogueGui.IsAnimating) yield break;
            yield return StartCoroutine(m_dialogueGui.HideAsync());
            m_gameState.ActiveCharacter.ResumeAnimations();
            m_gameState.ActiveDialogue = null;
            m_gameState.IgnoreInput = false;
            // Needs to be last or IgnoreInput would have an inconsistent state
            m_gameState.OnStoppedTalking();
        }
        public void TalkToActiveCharacter()
        {
            StartCoroutine(TalkToActiveCharacterAsync());
        }
        public void AdvanceDialogue()
        {
            StartCoroutine(AdvanceDialogueAsync());
        }

        public IEnumerator TalkToActiveCharacterAsync()
        {
            if (m_gameState.ActiveCharacter == null) yield break;
            if (m_dialogueGui.IsAnimating) yield break;

            m_gameState.OnEventHappened("SFX(START_DIALOGUE)");
            m_gameState.IgnoreInput = true;
            m_gameState.ActiveDialogue = Dialogue.FromAsset(m_gameState.Phase, m_gameState.ActiveCharacter.Name);
            m_gameState.ActiveCharacter.PauseAnimations();
            m_gameState.ActiveCharacter.Face(m_player.transform);
            m_player.Face(m_gameState.ActiveCharacter.transform);
            m_gameState.OnStartedTalking();
            yield return StartCoroutine(AdvanceDialogueAsync());
        }

        public IEnumerator AdvanceDialogueAsync()
        {
            if (m_dialogueGui.IsAnimating) yield break;

            string line = m_gameState.AdvanceOneLine();
            string speakerName = m_characterManager.GetSpeakerName(line);
            if (line.StartsWith("#"))
            {
                int index = line.IndexOf(": ", 1);
                if (index < 0) throw new Exception(string.Format("There should be a column followed by a space (: ) after the hash (#) on line {0}, in dialogue {1}", line, m_gameState.ActiveDialogue.Name));
                speakerName = line.Substring(1, index - 1);
                line = line.Substring(speakerName.Length + 3);
            }
            else
            {
                // The default speaker is the one we are talking to
                if (string.IsNullOrEmpty(speakerName)) speakerName = m_gameState.ActiveDialogue.DefaultSpeaker;
                else
                {
                    // Strip speaker's name and ": " from the line
                    line = line.Substring(speakerName.Length + 2);
                }

            }
            var closeUp = CloseUp.FromAsset(speakerName);
            yield return StartCoroutine(m_dialogueGui.ShowLineAsync(closeUp, speakerName, line));
        }

        public void InteractWithHouse()
        {
            if (m_gameState.ActiveHouse.IsHome)
            {
                if (m_gameState.IsIndoor) ExitHouse();
                else EnterHouse();
            }
            else KnockDoor();
        }

        public void AdvancePhase()
        {
            switch (m_gameState.Phase)
            {
                case GamePhase.Introduction:
                    m_gameState.Phase = GamePhase.Gameplay;
                    break;
                case GamePhase.Gameplay:
                default:
                    m_gameState.Phase = GamePhase.Ending;
                    break;
            }
            //if (m_gameState.IgnoreInput) m_gameState.IgnoreInput = false;
        }
        public void ExitHouse()
        {
            m_gameState.IsIndoor = false;
            m_colliderManager.disbleIndoors();
        }
        public void EnterHouse()
        {
            m_gameState.IsIndoor = true;
            m_colliderManager.enableIndoors();
            //if (m_gameState.IsPhaseOver(m_gameState.Phase))
            //{
            //    AdvancePhase();
            //    //m_gameState.OnGameIsOver();
            //    // Start ending cutscene
            //    //StartCoroutine(m_gameOverGui.ShowAsync());
            //}
            //else
            //{
            //    m_gameState.ActiveCharacter = m_player;
            //    TalkToActiveCharacter();
            //}
        }

        public void GameOver()
        {
            m_gameState.IgnoreInput = true;
            m_gameState.OnGameIsOver();
            //Start ending cutscene
            //StartCoroutine(m_gameOverGui.ShowAsync());
        }

        public void TalkToSelf()
        {
            m_gameState.ActiveCharacter = m_player;
            TalkToActiveCharacter();
        }
        public IEnumerator TalkToSelfAsync()
        {
            m_gameState.ActiveCharacter = m_player;
            yield return StartCoroutine(TalkToActiveCharacterAsync());
        }

        public void KnockDoor()
        {
            m_player.Face(m_gameState.ActiveHouse.transform);
            m_gameState.IgnoreInput = true;
            m_gameState.OnDoorKnocked();
            m_gameState.OnEventHappened("SFX(KNOCK)");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
                Application.OpenURL(webplayerQuitURL);
#else
                Application.Quit();
#endif
            }

            // Only look for the closest character/house if not in a dialogue
            if (m_gameState.ActiveDialogue == null)
            {
                if (m_gameState.IsIndoor) LookForClosestOutdoor();
                else LookForClosestOutdoor();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                if (m_gameState.IsIndoor) InteractIndoor();
                else InteractOutdoor();
            }
        }

        // Indoor, give priority to characters
        void LookForClosestIndoor()
        {
            float distance;
            var character = m_characterManager.GetClosest(m_player.transform.position, out distance);
            if (character != null && distance < 3f && Dialogue.Exists(m_gameState.Phase, character.Name))
            {
                m_gameState.ActiveCharacter = character;
            }
            else
            {
                m_gameState.ActiveCharacter = null;

                // Only check houses interaction if no character is available
                var house = m_houseManager.GetClosest(m_player.transform.position, out distance);
                if (house != null && distance < 3f) m_gameState.ActiveHouse = house;
                else m_gameState.ActiveHouse = null;
            }
        }
        // Outdoor, give priority to houses
        void LookForClosestOutdoor()
        {
            float distance;
            var house = m_houseManager.GetClosest(m_player.transform.position, out distance);
            if (house != null && distance < 3f) m_gameState.ActiveHouse = house;
            else m_gameState.ActiveHouse = null;

            var character = m_characterManager.GetClosest(m_player.transform.position, out distance);
            if (character != null && distance < 3f && Dialogue.Exists(m_gameState.Phase, character.Name)) m_gameState.ActiveCharacter = character;
            else m_gameState.ActiveCharacter = null;
        }

        public void InteractIndoor()
        {
            if (TemporarilyDisabled) return;

            // A dialogue is currently open
            if (m_gameState.ActiveDialogue != null)
            {
                // But we reached the last line, stop it
                if (m_gameState.IsDialogueOver) StopTalking();
                // There are more lines, go on
                else AdvanceDialogue();
            }
            // No dialogue currently open, but there's someone in range, let's talk to him
            else if (m_gameState.ActiveCharacter != null) TalkToActiveCharacter();
            // No character in range, but there's a house
            else if (m_gameState.ActiveHouse != null) InteractWithHouse();
            // Nothing to interact with, here. Play SFX for invalid actions
            else m_gameState.OnEventHappened("SFX(WRONG)");
        }


        public void InteractOutdoor()
        {
            if (TemporarilyDisabled) return;

            // A dialogue is currently open
            if (m_gameState.ActiveDialogue != null)
            {
                // But we reached the last line, stop it
                if (m_gameState.IsDialogueOver) StopTalking();
                // There are more lines, go on
                else AdvanceDialogue();
            }
            // No dialogue currently open, but there's a house
            else if (m_gameState.ActiveHouse != null) InteractWithHouse();
            // No house, but there's someone in range, let's talk to him
            else if (m_gameState.ActiveCharacter != null) TalkToActiveCharacter();
            // Nothing to interact with, here. Play SFX for invalid actions
            else m_gameState.OnEventHappened("SFX(WRONG)");
        }

        [NonSerialized]
        bool m_temporarilyDisabled;
        public bool TemporarilyDisabled
        {
            get { return m_temporarilyDisabled; }
            set {
                m_temporarilyDisabled = value;
                OnEnablingChanged();
            }
        }

        [NonSerialized]
        Action m_enablingChanged;
        public event Action EnablingChanged
        {
            add { m_enablingChanged += value; }
            remove { m_enablingChanged -= value; }
        }
        void OnEnablingChanged()
        {
            if (m_enablingChanged != null) m_enablingChanged();
        }
    }
}

namespace GameJam
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// State of the game
    /// Only the GameController should modify this
    /// ScriptableObject for easy referencing
    /// </summary>
    [CreateAssetMenu(fileName = "GameState", menuName = "GameJam/Game State")]
    public sealed class GameState : ScriptableObject
    {
        [NonSerialized]
        bool m_ignoreInput = false;
        public bool IgnoreInput
        {
            get { return m_ignoreInput; }
            set { m_ignoreInput = value; }
        }

        [NonSerialized]
        Action m_gameIsOver;
        public event Action GameIsOver
        {
            add { m_gameIsOver += value; }
            remove { m_gameIsOver -= value; }
        }
        public void OnGameIsOver()
        {
            if (m_gameIsOver != null) m_gameIsOver();
        }

        [SerializeField]
        DialogueManager m_dialogueManager;

        [NonSerialized]
        Action m_activeCharacterChanged;
        public event Action ActiveCharacterChanged
        {
            add { m_activeCharacterChanged += value; }
            remove { m_activeCharacterChanged -= value; }
        }
        void OnActiveCharacterChanged()
        {
            if (m_activeCharacterChanged != null) m_activeCharacterChanged();
        }

        [NonSerialized]
        Character m_activeCharacter;
        public Character ActiveCharacter
        {
            get { return m_activeCharacter; }
            set {
                if (m_activeCharacter == value) return;
                m_activeCharacter = value;
                OnActiveCharacterChanged();
            }
        }

        [NonSerialized]
        Dictionary<string, int> m_cursorByDialogueName;
        void OnEnable()
        {
            if (m_cursorByDialogueName == null)
            {
                m_cursorByDialogueName = new Dictionary<string, int>();
            }
        }

        [NonSerialized]
        Dialogue m_activeDialogue;
        public Dialogue ActiveDialogue
        {
            get { return m_activeDialogue; }
            set {
                if (m_activeDialogue == value) return;
                m_activeDialogue = value;
                if (m_activeDialogue == null) return;

                // If it's a new dialogue, add cursor at the beginning
                int cursor = 0;
                if (!m_cursorByDialogueName.TryGetValue(m_activeDialogue.Name, out cursor))
                {
                    m_cursorByDialogueName.Add(m_activeDialogue.Name, 0);
                }
            }
        }

        public int GetCompletedDialogues()
        {
            int count = 0;
            foreach (var dialogue in m_dialogueManager)
            {
                if (IsOver(dialogue)) count++;
            }
            return count;
        }

        public bool IsDialogueOver
        {
            get
            {
                return IsOver(ActiveDialogue);
            }
        }
        public bool IsOver(Dialogue dialogue)
        {
            if (dialogue == null) return false;
            int cursor = 0;
            m_cursorByDialogueName.TryGetValue(dialogue.Name, out cursor);
            return cursor == dialogue.Lines.Count;
        }

        public string AdvanceOneLine()
        {
            int index = m_cursorByDialogueName[ActiveDialogue.Name];
            if (index < ActiveDialogue.Lines.Count)
            {
                // Advance cursor
                m_cursorByDialogueName[ActiveDialogue.Name]++;
                var line = ActiveDialogue.Lines[index];
                // If it's an event, go to the next one
                if (IsEvent(line)) line = AdvanceOneLine();
                return line;
            }
            return ActiveDialogue.FinalWords;
        }

        bool IsEvent(string line)
        {
            if (!line.StartsWith("@")) return false;
            OnEventHappened(line.Substring(1));
            return true;
        }

        [NonSerialized]
        Action<string> m_eventHappened;
        public event Action<string> EventHappened
        {
            add { m_eventHappened += value; }
            remove { m_eventHappened -= value; }
        }
        void OnEventHappened(string eventName)
        {
            if (m_eventHappened != null) m_eventHappened(eventName);
        }
    }
}

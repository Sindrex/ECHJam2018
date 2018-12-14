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

        public bool IsDialogueOver
        {
            get
            {
                return m_cursorByDialogueName[ActiveDialogue.Name] == ActiveDialogue.Lines.Count;
            }
        }

        public string AdvanceOneLine(bool peek = false)
        {
            int index = m_cursorByDialogueName[ActiveDialogue.Name];
            if (index < ActiveDialogue.Lines.Count)
            {
                // Advance cursor
                if (!peek) m_cursorByDialogueName[ActiveDialogue.Name]++;
                return ActiveDialogue.Lines[index];
            }
            return ActiveDialogue.FinalWords;
        }
    }
}

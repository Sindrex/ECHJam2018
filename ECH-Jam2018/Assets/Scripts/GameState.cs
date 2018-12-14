namespace GameJam
{
    using System;
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
        int m_cursor;

        [NonSerialized]
        Dialogue m_activeDialogue;
        public Dialogue ActiveDialogue
        {
            get { return m_activeDialogue; }
            set {
                if (m_activeDialogue == value) return;
                m_activeDialogue = value;
                m_cursor = 0;
            }
        }

        public string AdvanceOneLine(bool peek = false)
        {
            int index = m_cursor;
            if (index < ActiveDialogue.Lines.Count)
            {
                if (!peek) m_cursor++;
                return ActiveDialogue.Lines[index];
            }
            return ActiveDialogue.FinalWords;
        }
    }
}

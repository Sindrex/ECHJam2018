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
        [SerializeField]
        DialogueManager m_dialogueManager;

        [NonSerialized]
        GamePhase m_phase = (GamePhase)(-1);
        public GamePhase Phase
        {
            get { return m_phase; }
            set {
                if (m_phase == value) return;
                m_phase = value;
                OnPhaseChanged();
            }
        }

        [NonSerialized]
        bool m_ignoreInput = false;
        public bool IgnoreInput
        {
            get { return m_ignoreInput; }
            set {
                if (m_ignoreInput == value) return;
                m_ignoreInput = value;
                OnIgnoreInputChanged();
            }
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
        House m_activeHouse;
        public House ActiveHouse
        {
            get { return m_activeHouse; }
            set
            {
                if (m_activeHouse == value) return;
                m_activeHouse = value;
                OnActiveHouseChanged();
            }
        }

        [NonSerialized]
        bool m_isIndoor = true;
        public bool IsIndoor
        {
            get { return m_isIndoor; }
            set {
                if (m_isIndoor == value) return;
                m_isIndoor = value;
                OnIndoorChanged();
            }
        }

        [NonSerialized]
        Dictionary<GamePhase, Dictionary<string, int>> m_cursorByDialogueNameByPhase;
        void OnEnable()
        {
            if (m_cursorByDialogueNameByPhase == null)
            {
                m_cursorByDialogueNameByPhase = new Dictionary<GamePhase, Dictionary<string, int>>();
                m_cursorByDialogueNameByPhase.Add(GamePhase.Introduction, new Dictionary<string, int>());
                m_cursorByDialogueNameByPhase.Add(GamePhase.Gameplay, new Dictionary<string, int>());
                m_cursorByDialogueNameByPhase.Add(GamePhase.Ending, new Dictionary<string, int>());
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
                if (m_activeDialogue != null)
                {
                    // If it's a new dialogue, add cursor at the beginning
                    int cursor = 0;
                    var dictionary = m_cursorByDialogueNameByPhase[Phase];
                    if (!dictionary.TryGetValue(m_activeDialogue.Name, out cursor))
                    {
                        dictionary.Add(m_activeDialogue.Name, 0);
                    }
                }
                OnActiveDialogueChanged();
            }
        }

        public bool IsPhaseOver()
        {
            return IsPhaseOver(Phase);
        }
        public bool IsPhaseOver(GamePhase phase)
        {
            int dialoguesCount = m_dialogueManager.GetDialoguesCount(phase);
            for (int i = 0; i < dialoguesCount; i++)
            {
                var dialogue = m_dialogueManager.GetDialogue(phase, i);
                if (dialogue.IsMandatory && !IsOver(phase, dialogue)) return false;
            }
            return true;
        }

        public bool IsDialogueOver
        {
            get
            {
                return IsOver(Phase, ActiveDialogue);
            }
        }
        public bool IsOver(GamePhase phase, Dialogue dialogue)
        {
            if (dialogue == null) return false;
            int cursor = 0;
            var dictionary = m_cursorByDialogueNameByPhase[phase];
            dictionary.TryGetValue(dialogue.Name, out cursor);
            return cursor == dialogue.Lines.Count;
        }

        public string AdvanceOneLine()
        {
            string result = "";
            var dictionary = m_cursorByDialogueNameByPhase[Phase];
            int index = dictionary[ActiveDialogue.Name];
            if (index < ActiveDialogue.Lines.Count)
            {
                // Advance cursor
                dictionary[ActiveDialogue.Name]++;
                var line = ActiveDialogue.Lines[index];
                // If it's an event, go to the next one
                if (IsEvent(line)) line = AdvanceOneLine();
                result = line;
            }
            else result = ActiveDialogue.FinalWords;
            OnChanged();
            return result;
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
        public void OnEventHappened(string eventName)
        {
            if (m_eventHappened != null) m_eventHappened(eventName);
            OnChanged();
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
            OnChanged();
        }
        [NonSerialized]
        Action m_startedTalking;
        public event Action StartedTalking
        {
            add { m_startedTalking += value; }
            remove { m_startedTalking -= value; }
        }
        public void OnStartedTalking()
        {
            if (m_startedTalking != null) m_startedTalking();
            OnChanged();
        }

        [NonSerialized]
        Action m_stoppedTalking;
        public event Action StoppedTalking
        {
            add { m_stoppedTalking += value; }
            remove { m_stoppedTalking -= value; }
        }
        public void OnStoppedTalking()
        {
            if (m_stoppedTalking != null) m_stoppedTalking();
            OnChanged();
        }

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
            OnChanged();
        }

        [NonSerialized]
        Action m_activeHouseChanged;
        public event Action ActiveHouseChanged
        {
            add { m_activeHouseChanged += value; }
            remove { m_activeHouseChanged -= value; }
        }
        void OnActiveHouseChanged()
        {
            if (m_activeHouseChanged != null) m_activeHouseChanged();
            OnChanged();
        }

        [NonSerialized]
        Action m_phaseChanged;
        public event Action PhaseChanged
        {
            add { m_phaseChanged += value; }
            remove { m_phaseChanged -= value; }
        }
        void OnPhaseChanged()
        {
            if (m_phaseChanged != null) m_phaseChanged();
            OnChanged();
        }

        [NonSerialized]
        Action m_ignoreInputChanged;
        public event Action IgnoreInputChanged
        {
            add { m_ignoreInputChanged += value; }
            remove { m_ignoreInputChanged -= value; }
        }
        void OnIgnoreInputChanged()
        {
            if (m_ignoreInputChanged != null) m_ignoreInputChanged();
            OnChanged();
        }

        [NonSerialized]
        Action m_activeDialogueChanged;
        public event Action ActiveDialogueChanged
        {
            add { m_activeDialogueChanged += value; }
            remove { m_activeDialogueChanged -= value; }
        }
        void OnActiveDialogueChanged()
        {
            if (m_activeDialogueChanged != null) m_activeDialogueChanged();
            OnChanged();
        }

        [NonSerialized]
        Action m_changed;
        public event Action Changed
        {
            add { m_changed += value; }
            remove { m_changed -= value; }
        }
        void OnChanged()
        {
            if (m_changed != null) m_changed();
        }

        [NonSerialized]
        Action m_indoorChanged;
        public event Action IndoorChanged
        {
            add { m_indoorChanged += value; }
            remove { m_indoorChanged -= value; }
        }
        void OnIndoorChanged()
        {
            if (m_indoorChanged != null) m_indoorChanged();
            OnChanged();
        }

        [NonSerialized]
        Action m_doorKnocked;
        public event Action DoorKnocked
        {
            add { m_doorKnocked += value; }
            remove { m_doorKnocked -= value; }
        }
        public void OnDoorKnocked()
        {
            if (m_doorKnocked != null) m_doorKnocked();
        }
    }
}

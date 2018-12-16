namespace GameJam
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public sealed class Dialogue
    {
        [NonSerialized]
        string m_name;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        [SerializeField]
        string m_defaultSpeaker;
        public string DefaultSpeaker
        {
            get { return m_defaultSpeaker; }
            set { m_defaultSpeaker = value; }
        }

        [SerializeField]
        List<string> m_lines;
        public List<string> Lines
        {
            get { return m_lines; }
            set { m_lines = value; }
        }

        [SerializeField]
        string m_finalWords;
        public string FinalWords
        {
            get { return m_finalWords; }
            set { m_finalWords = value; }
        }

        [SerializeField]
        bool m_isMandatory = true;
        public bool IsMandatory
        {
            get { return m_isMandatory; }
            set { m_isMandatory = value; }
        }

        public static bool Exists(GamePhase phase, string dialogueName)
        {
            var asset = Resources.Load<TextAsset>(string.Format("Dialogue/{0}/{1}", phase, dialogueName));
            return asset != null;
        }
        public static Dialogue FromAsset(GamePhase phase, string dialogueName)
        {
            var asset = Resources.Load<TextAsset>(string.Format("Dialogue/{0}/{1}", phase, dialogueName));
            var json = asset.text;
            return FromJson(dialogueName, json);
        }
        public static Dialogue FromJson(string dialogueName, string json)
        {
            var dialogue = JsonUtility.FromJson<Dialogue>(json);
            dialogue.Name = dialogueName;
            return dialogue;
        }
    }
}

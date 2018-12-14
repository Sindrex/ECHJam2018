namespace GameJam
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public sealed class Dialogue
    {
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

        public static Dialogue FromAsset(string dialogueName)
        {
            var asset = Resources.Load<TextAsset>("Dialogue/" + dialogueName);
            var json = asset.text;
            return JsonUtility.FromJson<Dialogue>(json);
        }
    }
}

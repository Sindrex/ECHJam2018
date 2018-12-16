namespace GameJam
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "DialogueManager", menuName = "GameJam/Dialogue Manager")]
    public sealed class DialogueManager : ScriptableObject, IEnumerable<Dialogue>
    {
        [NonSerialized]
        Dictionary<string, Dialogue> m_dialogueByName;

        //[NonSerialized]
        //Dictionary<GamePhase, Dictionary<string, Dialogue>> m_dialogueByName;
        //[NonSerialized]
        //Dictionary<string, Dialogue> m_dialogueByName;
        //[NonSerialized]
        //Dictionary<string, Dialogue> m_dialogueByName;

        void OnEnable()
        {
            if (m_dialogueByName != null) return;
            var dialogueAssets = Resources.LoadAll<TextAsset>("Dialogue");
            m_dialogueByName = new Dictionary<string, Dialogue>();
            foreach (var asset in dialogueAssets)
            {
                var dialogue = Dialogue.FromJson(asset.name, asset.text);
                m_dialogueByName.Add(dialogue.Name, dialogue);
            }
        }

        public int Count
        {
            get { return m_dialogueByName.Count; }
        }

        public Dialogue GetDialogue(string name)
        {
            return m_dialogueByName[name];
        }

        IEnumerator<Dialogue> IEnumerable<Dialogue>.GetEnumerator()
        {
            foreach (var dialogue in m_dialogueByName.Values)
            {
                yield return dialogue;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var dialogue in m_dialogueByName.Values)
            {
                yield return dialogue;
            }
        }
    }
}

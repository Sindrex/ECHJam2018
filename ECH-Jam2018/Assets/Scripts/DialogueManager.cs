namespace GameJam
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "DialogueManager", menuName = "GameJam/Dialogue Manager")]
    public sealed class DialogueManager : ScriptableObject
    {
        //[NonSerialized]
        //Dictionary<string, Dialogue> m_dialogueByName;

        [NonSerialized]
        Dictionary<GamePhase, List<Dialogue>> m_dialoguesByPhase;
        void OnEnable()
        {
            if (m_dialoguesByPhase != null) return;
            Dictionary<GamePhase, TextAsset[]> allAssets = new Dictionary<GamePhase, TextAsset[]>();
            allAssets.Add(GamePhase.Introduction, Resources.LoadAll<TextAsset>("Dialogue/Introduction"));
            allAssets.Add(GamePhase.Gameplay, Resources.LoadAll<TextAsset>("Dialogue/Gameplay"));
            allAssets.Add(GamePhase.Ending, Resources.LoadAll<TextAsset>("Dialogue/Ending"));

            //m_dialogueByName = new Dictionary<string, Dialogue>();
            m_dialoguesByPhase = new Dictionary<GamePhase, List<Dialogue>>();
            foreach (var phaseToAssets in allAssets)
            {
                var phase = phaseToAssets.Key;
                var assets = phaseToAssets.Value;
                m_dialoguesByPhase.Add(phase, new List<Dialogue>());
                foreach (var asset in assets)
                {
                    var dialogue = Dialogue.FromJson(asset.name, asset.text);
                    m_dialoguesByPhase[phase].Add(dialogue);
                    //m_dialogueByName.Add(dialogue.Name, dialogue);
                }
            }
        }


        public Dialogue GetDialogue(GamePhase phase, int index)
        {
            return m_dialoguesByPhase[phase][index];
        }
        public int GetDialoguesCount(GamePhase phase)
        {
            return m_dialoguesByPhase[phase].Count;
        }

        public int GetDialoguesCount()
        {
            int count = 0;
            foreach (var list in m_dialoguesByPhase.Values)
            {
                count += list.Count;
            }
            return count;
        }

        public IEnumerator<Dialogue> GetDialogues(GamePhase phase)
        {
            return m_dialoguesByPhase[phase].GetEnumerator();
        }
    }
}

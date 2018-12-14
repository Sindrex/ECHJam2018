namespace GameJam
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Characters related utilities like:
    ///  - Access character by name
    ///  - Extract character name from a dialogue "Bob: I used to work for..." -> "Bob"
    /// </summary>
    public sealed class CharacterManager : MonoBehaviour
    {
        [NonSerialized]
        Dictionary<string, Character> m_charactersByName;

        void Awake()
        {
            m_charactersByName = new Dictionary<string, Character>();
            Character[] characters = GetComponentsInChildren<Character>();
            foreach (var character in characters)
            {
                if (character == null) continue;
                m_charactersByName.Add(character.name, character);
            }
        }

        public string GetSpeakerName(string line)
        {
            foreach (var character in m_charactersByName.Values)
            {
                if(line.StartsWith(character.Name + ": "))
                {
                    return character.Name;
                }
            }
            return "";
        }

        public Character GetCharacter(string name)
        {
            return m_charactersByName[name];
        }
    }
}

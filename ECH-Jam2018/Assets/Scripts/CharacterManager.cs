namespace GameJam
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Characters related utilities like:
    ///  - Access character by name
    ///  - Find closest character by position
    ///  - Extract character name from a dialogue "Bob: I used to work for..." -> "Bob"
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterManager", menuName = "GameJam/Character Manager")]
    public sealed class CharacterManager : ScriptableObject
    {
        [NonSerialized]
        Dictionary<string, Character> m_charactersByName;

        void OnEnable()
        {
            if(m_charactersByName == null) m_charactersByName = new Dictionary<string, Character>();
            //Character[] characters = GetComponentsInChildren<Character>();
            //foreach (var character in characters)
            //{
            //    if (character == null) continue;
            //    m_charactersByName.Add(character.Name, character);
            //}
        }

        public void Add(Character character)
        {
            string name = character.Name;
            if (m_charactersByName.ContainsKey(name)) m_charactersByName[name] = character;
            else m_charactersByName.Add(name, character);
        }
        public bool Remove(Character character)
        {
            return m_charactersByName.Remove(character.Name);
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

        public Character GetClosest(Vector3 position, out float distance, bool includingPlayer = false)
        {
            float minDistanceSquared = float.MaxValue;
            Character closest = null;
            foreach (var character in m_charactersByName.Values)
            {
                if (!includingPlayer && character.IsPlayer) continue;
                var distanceSquared = (character.transform.position - position).sqrMagnitude;
                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    closest = character;
                }
            }
            if (closest != null) distance = Mathf.Sqrt(minDistanceSquared);
            else distance = float.MaxValue;
            return closest;
        }

        public Character GetCharacter(string name)
        {
            return m_charactersByName[name];
        }
    }
}

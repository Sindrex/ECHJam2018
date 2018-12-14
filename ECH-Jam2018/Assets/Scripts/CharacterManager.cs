﻿namespace GameJam
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

namespace GameJam
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// House related utilities like:
    ///  - Find closest house by position
    /// </summary>
    public sealed class HouseManager : MonoBehaviour
    {
        [NonSerialized]
        Dictionary<string, House> m_housesByName;

        void Awake()
        {
            m_housesByName = new Dictionary<string, House>();
            House[] houses = GetComponentsInChildren<House>();
            foreach (var house in houses)
            {
                if (house == null) continue;
                m_housesByName.Add(house.name, house);
            }
        }

        public House GetClosest(Vector3 position, out float distance)
        {
            float minDistanceSquared = float.MaxValue;
            House closest = null;
            foreach (var house in m_housesByName.Values)
            {
                var distanceSquared = (house.transform.position - position).sqrMagnitude;
                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    closest = house;
                }
            }
            if (closest != null) distance = Mathf.Sqrt(minDistanceSquared);
            else distance = float.MaxValue;
            return closest;
        }

        public House GetCharacter(string name)
        {
            return m_housesByName[name];
        }
    }
}

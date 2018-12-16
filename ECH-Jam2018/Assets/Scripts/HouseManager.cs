namespace GameJam
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// House related utilities like:
    ///  - Find closest house by position
    /// </summary>
    [CreateAssetMenu(fileName = "HouseManager", menuName = "GameJam/House Manager")]
    public sealed class HouseManager : ScriptableObject
    {
        [NonSerialized]
        Dictionary<string, House> m_housesByName;

        void OnEnable()
        {
            if (m_housesByName == null) m_housesByName = new Dictionary<string, House>();
            //House[] houses = GetComponentsInChildren<House>();
            //foreach (var house in houses)
            //{
            //    if (house == null) continue;
            //    m_housesByName.Add(house.name, house);
            //}
        }

        public void Add(House house)
        {
            string name = house.OwnerName;
            if (m_housesByName.ContainsKey(name)) m_housesByName[name] = house;
            else m_housesByName.Add(name, house);
        }
        public bool Remove(House house)
        {
            return m_housesByName.Remove(house.OwnerName);
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

        public House GetHouse(string name)
        {
            return m_housesByName[name];
        }
    }
}

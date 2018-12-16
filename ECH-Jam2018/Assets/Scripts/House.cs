namespace GameJam
{
    using System;
    using System.Collections;
    using UnityEngine;

    public sealed class House : MonoBehaviour
    {
        [SerializeField]
        string m_ownerName;
        public string OwnerName
        {
            get { return m_ownerName; }
            set { m_ownerName = value; }
        }

        [SerializeField]
        bool m_isHome = false;
        public bool IsHome
        {
            get { return m_isHome; }
            set { m_isHome = value; }
        }
    }
}

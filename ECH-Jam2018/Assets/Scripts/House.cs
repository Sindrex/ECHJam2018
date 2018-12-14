namespace GameJam
{
    using System;
    using System.Collections;
    using UnityEngine;

    public sealed class House : MonoBehaviour
    {
        [SerializeField]
        float m_doorSpeed = 1f;
        public float DoorSpeed
        {
            get { return m_doorSpeed; }
            set { m_doorSpeed = value; }
        }

        [SerializeField]
        Transform m_door;
        public Transform Door
        {
            get { return m_door; }
            set { m_door = value; }
        }

        [SerializeField]
        bool m_isDoorOpen;
        public bool DoorIsOpen
        {
            get { return m_isDoorOpen; }
        }

        [NonSerialized]
        float m_originalScaleX;
        [NonSerialized]
        Vector3 m_targetScale;
        void Awake()
        {
            m_originalScaleX = Door.localScale.x;
            m_targetScale = Door.localScale;
        }

        public void ToggleDoor()
        {
            m_isDoorOpen = !DoorIsOpen;
            m_targetScale = Door.localScale;
            if (DoorIsOpen) m_targetScale.x = -m_originalScaleX;
            else m_targetScale.x = m_originalScaleX;
        }

        void Update()
        {
            Door.localScale = Vector3.MoveTowards(Door.localScale, m_targetScale, m_doorSpeed * Time.deltaTime);
        }
    }
}

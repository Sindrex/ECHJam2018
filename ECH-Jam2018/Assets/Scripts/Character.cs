namespace GameJam
{
    using System;
    using UnityEngine;

    public sealed class Character : MonoBehaviour
    {
        [SerializeField]
        string m_name;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        [SerializeField]
        bool m_isPlayer;
        public bool IsPlayer
        {
            get { return m_isPlayer; }
            set { m_isPlayer = value; }
        }

        [SerializeField]
        SpriteRenderer m_renderer;

        public void Face(Transform other)
        {
            m_renderer.flipX = (other.position.x < transform.position.x);
        }
    }
}

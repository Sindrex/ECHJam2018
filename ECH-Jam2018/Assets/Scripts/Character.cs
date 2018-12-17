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
        bool m_isCat = false;
        public bool IsCat
        {
            get { return m_isCat; }
        }

        [SerializeField]
        Transform m_selectionParent;
        public Transform SelectionParent
        {
            get { return m_selectionParent; }
        }
        [SerializeField]
        SpriteRenderer m_renderer;
        [SerializeField]
        Animator m_animator;

        [SerializeField]
        CharacterManager m_manager;
        void OnEnable()
        {
            m_manager.Add(this);
        }
        void OnDisable()
        {
            m_manager.Remove(this);
        }

        public CharacterFacing Facing
        {
            get
            {
                if (m_renderer == null) return CharacterFacing.Right;
                bool isFlipped = m_renderer.flipX;
                if (IsCat) isFlipped = !isFlipped;
                return (isFlipped) ? CharacterFacing.Left : CharacterFacing.Right;
            }
            set
            {
                if (m_renderer == null) return;
                bool mustFlip = (value == CharacterFacing.Left);
                if (IsCat) mustFlip = !mustFlip;
                m_renderer.flipX = mustFlip;
            }
        }

        public bool Visible
        {
            get {
                if (m_renderer == null) return true;
                return m_renderer.enabled;
            }
            set {
                if (m_renderer == null) return;
                m_renderer.enabled = value;
            }
        }

        public void Face(Transform other)
        {
            Facing = (other.position.x < transform.position.x) ? CharacterFacing.Left : CharacterFacing.Right;
        }
        public void PauseAnimations()
        {
            if (m_animator != null) m_animator.enabled = false;
        }
        public void ResumeAnimations()
        {
            if (m_animator != null) m_animator.enabled = true;
        }

        public void StandStill(bool still)
        {
            if (IsPlayer) return;
            if(m_animator != null) m_animator.SetBool("StandStill", still);
        }
    }
}

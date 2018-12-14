using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public sealed class CloseUp : MonoBehaviour
    {
        [SerializeField]
        Image m_head;
        public Sprite Head
        {
            get { return m_head.sprite; }
        }
        [SerializeField]
        Image m_eyes;
        public Sprite Eyes
        {
            get { return m_eyes.sprite; }
        }
        [SerializeField]
        Image m_nose;
        public Sprite Nose
        {
            get { return m_nose.sprite; }
        }
        [SerializeField]
        Image m_mouth;
        public Sprite Mouth
        {
            get { return m_mouth.sprite; }
        }
        [SerializeField]
        Image m_ears;
        public Sprite Ears
        {
            get { return m_ears.sprite; }
        }
        [SerializeField]
        Image m_hairs;
        public Sprite Hairs
        {
            get { return m_hairs.sprite; }
        }
        public void Clear()
        {
            CopyFrom(null);
        }
        public void CopyFrom(CloseUp other)
        {
            if(other != null)
            {
                m_hairs.sprite = other.Hairs;
                m_eyes.sprite = other.Eyes;
                m_nose.sprite = other.Nose;
                m_mouth.sprite = other.Mouth;
                m_ears.sprite = other.Ears;
                m_head.sprite = other.Head;
            }
            else
            {
                m_hairs.sprite = m_eyes.sprite = m_nose.sprite = m_mouth.sprite = m_ears.sprite = m_head.sprite = null;
            }
        }

        public static CloseUp FromAsset(string closeUpName)
        {
            return Resources.Load<CloseUp>("CloseUp/" + closeUpName);
        }
    }
}

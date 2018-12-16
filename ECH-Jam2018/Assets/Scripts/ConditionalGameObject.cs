namespace GameJam
{
    using UnityEngine;

    public abstract class ConditionalGameObject : MonoBehaviour
    {
        [SerializeField]
        GameObject m_target;
        public GameObject Target
        {
            get { return m_target; }
            set { m_target = value; }
        }

        [SerializeField]
        GameState m_gameState;
        protected GameState GameState
        {
            get { return m_gameState; }
        }

        void Awake()
        {
            m_gameState.Changed += CheckConditionForActivation;
        }
        void Destroy()
        {
            m_gameState.Changed -= CheckConditionForActivation;
        }

        void CheckConditionForActivation()
        {
            Target.SetActive(Condition());
        }

        protected abstract bool Condition();
    }
}

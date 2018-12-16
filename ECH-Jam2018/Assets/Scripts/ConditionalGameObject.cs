namespace GameJam
{
    using UnityEngine;

    public abstract class ConditionalGameObject : MonoBehaviour
    {
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
            gameObject.SetActive(Condition());
        }

        protected abstract bool Condition();
    }
}

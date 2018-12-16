namespace GameJam
{
    using UnityEngine;

    /// <summary>
    /// Automatically activates/deactivates on the selected phase
    /// </summary>
    public sealed class PhaseDependentObject : ConditionalGameObject
    {
        [SerializeField]
        GamePhase m_phase;
        public GamePhase Phase
        {
            get { return m_phase; }
            set { m_phase = value; }
        }

        protected override bool Condition()
        {
            return GameState.Phase == Phase;
        }
    }
}

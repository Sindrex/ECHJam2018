namespace GameJam
{
    using UnityEngine;

    public sealed class CharacterSelection : MonoBehaviour
    {
        [SerializeField]
        GameState m_gameState;
        [SerializeField]
        Transform m_selection;

        void Awake()
        {
            m_gameState.ActiveCharacterChanged += MoveOnActiveCharacter;
            MoveOnActiveCharacter();
        }
        void OnDestroy()
        {
            m_gameState.ActiveCharacterChanged -= MoveOnActiveCharacter;
        }

        void MoveOnActiveCharacter()
        {
            if(m_gameState.ActiveCharacter != null)
            {
                m_selection.SetParent(m_gameState.ActiveCharacter.SelectionParent, false);
                //m_selection.gameObject.SetActive(true);
            }
            else
            {
                m_selection.SetParent(transform, false);
                //m_selection.gameObject.SetActive(false);
            }
        }
        void Update()
        {
            bool invisible = m_gameState.ActiveCharacter == null || !m_gameState.ActiveCharacter.Visible;
            m_selection.gameObject.SetActive(!invisible);
        }
    }
}

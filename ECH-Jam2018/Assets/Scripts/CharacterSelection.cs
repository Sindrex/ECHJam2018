namespace GameJam
{
    using UnityEngine;

    public sealed class CharacterSelection : MonoBehaviour
    {
        [SerializeField]
        GameState m_gameState;

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
            gameObject.SetActive(m_gameState.ActiveCharacter != null);
            if(m_gameState.ActiveCharacter != null) transform.position = m_gameState.ActiveCharacter.transform.position + 2f * Vector3.up;
        }
    }
}

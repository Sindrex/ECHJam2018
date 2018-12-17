namespace GameJam
{
    using System;
    using System.Collections;
    using UnityEngine;

    public sealed class EndingManager : MonoBehaviour
    {
        [SerializeField]
        GameController m_gameController;
        [SerializeField]
        GameState m_gameState;
        [SerializeField]
        CharacterManager m_characterManager;

        [SerializeField]
        Transform m_richPosition;
        [SerializeField]
        Transform m_mitraPosition;
        [SerializeField]
        Transform m_oneilPosition;
        [SerializeField]
        Transform m_cainPosition;
        [SerializeField]
        Transform m_playerPosition;

        [SerializeField]
        GameOverGui m_gameOverGui;

        [NonSerialized]
        Fader m_fader;
        void Awake()
        {
            m_fader = FindObjectOfType<Fader>();
            if (m_fader == null) throw new Exception("Cannot find Fader");
        }
        void OnEnable()
        {
            m_gameState.EventHappened += CheckEvent;
            m_gameState.StoppedTalking += ShowGameOverScreen; ;

            m_gameController.GameOver();
            StartCoroutine(SequenceAsync());
        }
        void OnDisable()
        {
            m_gameState.EventHappened -= CheckEvent;
            m_gameState.StoppedTalking -= ShowGameOverScreen;
        }

        void CheckEvent(string eventName)
        {
            if (eventName == "@ENTER_HOUSE")
            {
                StartCoroutine(EnterHouseAsync());
            }
        }

        void ShowGameOverScreen()
        {
            StartCoroutine(m_gameOverGui.ShowAsync());
        }

        IEnumerator SequenceAsync()
        {
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(m_gameController.TalkToSelfAsync());
        }

        IEnumerator EnterHouseAsync()
        {
            var player = m_gameController.Player;
            var rich = m_characterManager.GetCharacter("Rich");
            var mitra = m_characterManager.GetCharacter("Mitra");
            var cain = m_characterManager.GetCharacter("Cain");
            var oneil = m_characterManager.GetCharacter("O'Neil");
            m_gameController.TemporarilyDisabled = true;
            yield return StartCoroutine(m_fader.ShowAsync());

            player.transform.position = m_playerPosition.position;
            rich.transform.position = m_richPosition.position;
            mitra.transform.position = m_mitraPosition.position;
            cain.transform.position = m_cainPosition.position;
            oneil.transform.position = m_oneilPosition.position;

            player.Facing = CharacterFacing.Left;
            rich.Facing = CharacterFacing.Right;
            mitra.Facing = CharacterFacing.Right;
            cain.Facing = CharacterFacing.Right;
            oneil.Facing = CharacterFacing.Right;

            yield return StartCoroutine(m_fader.HideAsync());
            m_gameController.TemporarilyDisabled = false;
        }
    }
}

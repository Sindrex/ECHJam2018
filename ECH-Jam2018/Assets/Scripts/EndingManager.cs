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

        [SerializeField]
        Fader m_fader;

        [NonSerialized]
        ShootingStarScript m_star;
        void Awake()
        {
            m_star = FindObjectOfType<ShootingStarScript>();
            if (m_star == null) throw new Exception("Can't find ShootingStar");
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
            if (eventName == "ENTER_HOUSE")
            {
                StartCoroutine(EnterHouseAsync());
            }
        }

        void ShowGameOverScreen()
        {
            StartCoroutine(StarThenGameOver());
        }
        IEnumerator StarThenGameOver()
        {
            m_gameController.GameOver();
            m_gameController.TemporarilyDisabled = true;
            m_star.startShootingStar();
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(m_gameOverGui.ShowAsync());
        }

        IEnumerator SequenceAsync()
        {
            m_gameController.TemporarilyDisabled = true;
            yield return new WaitForSeconds(2f);
            m_gameController.TemporarilyDisabled = false;
            yield return StartCoroutine(m_gameController.TalkToSelfAsync());
        }

        IEnumerator EnterHouseAsync()
        {
            m_gameController.TemporarilyDisabled = true;
            yield return StartCoroutine(m_fader.ShowAsync());

            var player = m_gameController.Player;
            var rich = m_characterManager.GetCharacter("Rich");
            var mitra = m_characterManager.GetCharacter("Mitra");
            var cain = m_characterManager.GetCharacter("Cain");
            var oneil = m_characterManager.GetCharacter("O'Neil");

            player.transform.position = m_playerPosition.position;
            rich.transform.position = m_richPosition.position;
            mitra.transform.position = m_mitraPosition.position;
            cain.transform.position = m_cainPosition.position;
            oneil.transform.position = m_oneilPosition.position;

            mitra.StandStill(true);
            cain.StandStill(true);
            oneil.StandStill(true);
            rich.StandStill(true);
            rich.Visible = true;

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

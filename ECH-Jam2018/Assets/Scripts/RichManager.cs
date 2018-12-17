namespace GameJam
{
    using System;
    using System.Collections;
    using UnityEngine;

    public sealed class RichManager : MonoBehaviour
    {
        [SerializeField]
        Character m_rich;
        [SerializeField]
        GameState m_gameState;
        [SerializeField]
        Fader m_fader;

        [SerializeField]
        Transform m_richPosition;
        [SerializeField]
        Transform m_playerPosition;

        [NonSerialized]
        GameController m_gameController;
        void Awake()
        {
            m_gameController = FindObjectOfType<GameController>();
            if (m_gameController == null) throw new Exception("Cannot find GameController");
        }
        void OnEnable()
        {
            m_gameState.DoorKnocked += MaybeStartDialogue;
            m_gameState.EventHappened += CheckEvent;
            m_gameState.StoppedTalking += RickGoesBackInside;
        }

        void OnDisable()
        {
            m_gameState.DoorKnocked -= MaybeStartDialogue;
            m_gameState.EventHappened -= CheckEvent;
            m_gameState.StoppedTalking -= RickGoesBackInside;
        }
        void MaybeStartDialogue()
        {
            StartCoroutine(MaybeStartDialogueAsync());
        }
        IEnumerator MaybeStartDialogueAsync()
        {
            if (m_gameState.ActiveHouse.OwnerName == "Rich")
            {
                m_rich.Visible = true;
                m_rich.PauseAnimations();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return StartCoroutine(m_gameController.TalkToActiveCharacterAsync());
            }
        }

        void CheckEvent(string eventName)
        {
            if (eventName == "ENTER_HOUSE" && m_gameState.ActiveHouse.OwnerName == "Rich")
            {
                StartCoroutine(EnterHouseAsync());
            }
            else if (eventName == "EXIT_HOUSE" && m_gameState.ActiveHouse.OwnerName == "Rich")
            {
                StartCoroutine(ExitHouseAsync());
            }
        }
        [NonSerialized]
        Vector3 m_oldRichPosition;
        [NonSerialized]
        Vector3 m_oldPlayerPosition;
        [NonSerialized]
        CharacterFacing m_richFacing;
        [NonSerialized]
        CharacterFacing m_playerFacing;

        IEnumerator EnterHouseAsync()
        {
            m_gameController.TemporarilyDisabled = true;
            yield return StartCoroutine(m_fader.ShowAsync());

            m_gameController.EnterHouse();

            m_oldRichPosition = m_rich.transform.position;
            m_oldPlayerPosition = m_gameController.Player.transform.position;
            m_rich.transform.position = m_richPosition.position;
            m_gameController.Player.transform.position = m_playerPosition.position;
            m_richFacing = m_rich.Facing;
            m_playerFacing = m_gameController.Player.Facing;
            m_rich.Facing = CharacterFacing.Right;
            m_gameController.Player.Facing = CharacterFacing.Left;

            yield return StartCoroutine(m_fader.HideAsync());
            m_gameController.TemporarilyDisabled = false;
        }

        IEnumerator ExitHouseAsync()
        {
            m_gameController.TemporarilyDisabled = true;
            yield return StartCoroutine(m_fader.ShowAsync());
            m_gameController.ExitHouse();

            m_rich.transform.position = m_oldRichPosition;
            m_gameController.Player.transform.position = m_oldPlayerPosition;
            m_rich.Facing = m_richFacing;
            m_gameController.Player.Facing = m_playerFacing;

            yield return StartCoroutine(m_fader.HideAsync());
            m_gameController.TemporarilyDisabled = false;
        }

        void RickGoesBackInside()
        {
            if (m_gameState.ActiveHouse != null && m_gameState.ActiveHouse.OwnerName == "Rich")
            {
                m_rich.PauseAnimations();
                m_rich.Visible = false;
            }
        }
    }
}

using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using Localization;
using mini_game;
using Player;
using UnityEngine;

namespace GameScene.Interactables.Openables
{
    public class Safe : Openable
    {
        [SerializeField] private CipherBreaker cipherBreaker;
        [SerializeField] private GameObject safeOpenSound;
        
        [Header("Animation")]
        [SerializeField] private Animator safeDoor;
        [SerializeField] private BoxCollider boxCollider;
        
        [SerializeField] private Movement player;
        [SerializeField] private Visual visual;

        [SerializeField] private MultipleDialogue multipleDialogue;
        [SerializeField] private MultipleDialogue multipleDialogue1;

        [SerializeField] private Radio radio;

        private AudioClip _sawAmultet;
        private static readonly int Open = Animator.StringToHash("Open");

        private void Start()
        {
            _sawAmultet = Resources.Load<AudioClip>("Audio/saw-amulet");
            _sawAmultet.LoadAudioData();
        }

        protected override void OnInteracted()
        {
            if (AssignmentsDataHandler.Instance.IsCompleted(9))
            {
                OpenSafe();
            }
            else
            {
                if (!AssignmentsDataHandler.Instance.IsCompleted(6)) // books assignments
                {
                    multipleDialogue.Play();
                }
                else if (radio.IsPlaying())
                {
                    multipleDialogue1.Play();
                }
                else
                {
                    cipherBreaker.StartNewGame();
                    player.SetMoveStatus(false);
                    visual.SetRotateStatus(false);
                }
            }
        }
        
        public void OpenSafe()
        {
            player.SetMoveStatus(true);
            visual.SetRotateStatus(true);
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Instantiate(safeOpenSound, transform.position, Quaternion.identity);
            safeDoor.SetTrigger(Open);
            
            if (!AssignmentsDataHandler.Instance.IsCompleted(9))
            {
                AssignmentsDataHandler.Instance.Complete(9);
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("54") ,_sawAmultet, 0.5f);
            }
            
            gameObject.tag = "Untagged";
            Destroy(boxCollider);
        }
    }
}
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Interactables.Objects.AncientCubes.Dependencies;
using Localization;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Interactables.Objects.AncientCubes
{
    public class AncientCube : InteractableObject
    {
        [SerializeField] private LockDrawer lockDrawer;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject keyModel;

        private AudioClip _lockKeyOne;
        private AudioClip _lockKeyTwo;
        private AudioClip _lockKeyThree;
        private AudioClip _objectNotUseful;

        private bool _isTaken;
        
        [FormerlySerializedAs("OpenSound")] [SerializeField] private GameObject openSound;

        private void Start()
        {
            base.Initialize();
            _lockKeyOne = Resources.Load<AudioClip>("Audio/lock-key-finded1");
            _lockKeyTwo = Resources.Load<AudioClip>("Audio/lock-key-finded2");
            _lockKeyThree = Resources.Load<AudioClip>("Audio/lock-key-finded3");
            _objectNotUseful = Resources.Load<AudioClip>("Audio/object-not-useful");
            
            _lockKeyOne.LoadAudioData();
            _lockKeyTwo.LoadAudioData();
            _lockKeyThree.LoadAudioData();
            _objectNotUseful.LoadAudioData();
        }

        protected override void Interact()
        {
            if (AssignmentsDataHandler.Instance.Exists(14))
            {
                base.Interact();
                Instantiate(openSound, transform.position, Quaternion.identity);
                animator.SetTrigger("Open");
                keyModel.SetActive(false);

                lockDrawer.IncrementUnlockedLocks();
                gameObject.tag = "Untagged";
                ObjectStateDataHandler.Instance.UpdateObjectState(gameObject.name , "inactive");

                switch (lockDrawer.GetUnlockedLocks())
                {
                    case 1:
                        SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("93"), _lockKeyOne, 0.3f);
                        break;
                    case 2:
                        SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("94"), _lockKeyTwo, 0.3f);
                        break;
                    case 3:
                        SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("95"), _lockKeyThree, 0.3f);
                        var assignment = new Assignment(16, LocalizationManager.Instance.GetContent("178"), null, true, false);
                        AssignmentsDataHandler.Instance.Create(assignment, 14);
                        break;
                }

                Destroy(this);
            }
            else
            {
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("117"), _objectNotUseful, 0.3f);
            }
        }
    }
}
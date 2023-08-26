using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Objects.Rituals
{
    public class RitualObject : InteractableObject
    {
        [SerializeField] private RitualObjectController controller;
        private float _canInteract = -1f;
        
        private AudioClip _objectNotUseful;

        private void Start()
        {
            base.Initialize();
            _objectNotUseful = Resources.Load<AudioClip>("Audio/object-not-useful");
            _objectNotUseful.LoadAudioData();
        }

        protected override void Interact()
        {
            if (!(Time.time > _canInteract)) return;
            
            if (AssignmentsDataHandler.Instance.Exists(18))
            {
                base.Interact();
                ObjectStateDataHandler.Instance.UpdateObjectState(gameObject.name, "inactive");
                controller.UpdateHintCanvas();
                
                if (RitualObjectController.AllTaken())
                {
                    controller.OnComplete();
                }
                
                Destroy(gameObject);
            }
            else
            {
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("117"), _objectNotUseful, 0.3f);
            }
                
            _canInteract = Time.time + 3f;
        }
    }
}
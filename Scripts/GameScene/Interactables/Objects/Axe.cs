using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Objects
{
    public class Axe : InteractableObject
    {
        [SerializeField] private MeshRenderer mesh;
        private AudioClip _takeAxe;

        private void Start()
        {
            base.Initialize();
            _takeAxe = Resources.Load<AudioClip>("Audio/take-axe");
            _takeAxe.LoadAudioData();
        }

        protected override void Interact()
        {
            base.Interact();            
            SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("5"), _takeAxe, 0.5f);
            
            if (!AssignmentsDataHandler.Instance.Exists(3))
            {
                var assignment = new Assignment(3, LocalizationManager.Instance.GetContent("6"), null, true, false);
                AssignmentsDataHandler.Instance.Create(assignment);
            }
            
            mesh.enabled = false;
            gameObject.tag = "Untagged";
        }
    }
}
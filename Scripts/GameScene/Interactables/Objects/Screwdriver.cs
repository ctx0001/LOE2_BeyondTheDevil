using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Objects
{
    public class Screwdriver : InteractableObject
    {
        [SerializeField] private MeshRenderer mesh;

        private void Start()
        {
            base.Initialize();
        }

        protected override void Interact()
        {
            base.Interact();
            
            if (!AssignmentsDataHandler.Instance.Exists(5))
            {
                var assignment = new Assignment(5, LocalizationManager.Instance.GetContent("13"), null, true, false);
                AssignmentsDataHandler.Instance.Create(assignment, 4);
            }
            
            mesh.enabled = false;
            gameObject.tag = "Untagged";
        }
    }
}
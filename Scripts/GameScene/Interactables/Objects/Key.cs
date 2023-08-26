using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using JetBrains.Annotations;
using Jumpscares;
using Localization;
using Managers;
using UnityEngine;
using Assignment = GameScene.Data.Handlers.Dependencies.Assignment;

namespace GameScene.Interactables.Objects
{
    public class Key : InteractableObject
    {

        protected void Start()
        {
            base.Initialize();
        }

        [UsedImplicitly]
        protected override void Interact()
        {
            base.Interact();

            switch (base.GetItemName())
            {
                case "offices":
                {
                    var assignment = new Assignment(8,  LocalizationManager.Instance.GetContent("55"), null, true, false);
                    AssignmentsDataHandler.Instance.Create(assignment);
                    break;
                }
                case "stock":
                    HallJumpscareController.Instance.EnableJumpscare();
                    break;
                case "bathroom":
                {
                    var assignment = new Assignment(15, LocalizationManager.Instance.GetContent("180"), null, true, false);
                    AssignmentsDataHandler.Instance.Create(assignment);
                
                    AudioManager.Instance.EnableBathroomSingSound();
                    break;
                }
            }
            
            Destroy(gameObject);
        }
    }
}
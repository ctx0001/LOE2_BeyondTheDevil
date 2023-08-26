using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using UnityEngine;

namespace GameScene.Interactables.Readables
{
    public class VisualizableForNotebook : Visualizable
    {
        [SerializeField] private MultipleDialogue multipleDialogue;
        private bool _firstTimeNotebook = true;

        protected override void OnClose()
        {
            if (_firstTimeNotebook && !AssignmentsDataHandler.Instance.IsCompleted(6))
            {
                _firstTimeNotebook = false;
                AssignmentsDataHandler.Instance.Complete(6);
                AssignmentsDataHandler.Instance.UpdateNewActiveAssignment();
                multipleDialogue.Play();
            }
        }
    }
}
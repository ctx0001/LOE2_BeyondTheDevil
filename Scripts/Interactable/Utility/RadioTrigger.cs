using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Interactables;
using Localization;
using UnityEngine;

namespace Interactable.Utility
{
    public class RadioTrigger : MonoBehaviour
    {
        [SerializeField] private Radio radio;
        private bool _firstTime = true;
        [SerializeField] private MultipleDialogue multipleDialogue;
        [SerializeField] private GameObject radioJumpScareTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _firstTime && AssignmentsDataHandler.Instance.IsCompleted(6))
            {
                radio.enabled = true;
                radio.gameObject.tag = "Interactable";
                radio.PlayTrack();
                multipleDialogue.Play();

                var assignment = new Assignment(2, LocalizationManager.Instance.GetContent("4"), null, true, false);
                AssignmentsDataHandler.Instance.Create(assignment);
                AssignmentsDataHandler.Instance.UpdateNewActiveAssignment();
                _firstTime = false;
                radioJumpScareTrigger.SetActive(true);
            }
        }
    }
}
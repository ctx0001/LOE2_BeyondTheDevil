using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Interactable.Utility
{

    public class EnteringSewersTrigger : MonoBehaviour
    {
        [SerializeField] private MultipleDialogue multipleDialogue;
        private bool _firstTime = true;

        private IEnumerator Start()
        {
            // waiting for data to load
            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            // prevent from creating multiple instances of a assignment
            if (AssignmentsDataHandler.Instance.IsCompleted(39))
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (!_firstTime) return;

            var assignment = new Assignment(39, LocalizationManager.Instance.GetContent("237"), null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment);
            multipleDialogue.Play();
            _firstTime = false;
        }
    }
}

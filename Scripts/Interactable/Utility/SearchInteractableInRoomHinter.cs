using System;
using System.Collections;
using System.Collections.Generic;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using Localization;
using UnityEngine;

namespace Interactable.Utility
{
    public class SearchInteractableInRoomHinter : MonoBehaviour
    {
        [SerializeField] private int missionForSearchId;
        [SerializeField] private GameObject[] interactables;
        [SerializeField] private MultipleDialogue multipleDialogue;

        private IEnumerator Start()
        {
            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            if (AssignmentsDataHandler.Instance.IsCompleted(missionForSearchId))
            {
                Destroy(gameObject);
            }
        }

        private bool CheckIfInteractablesTaken()
        {
            foreach (var interactable in interactables)
            {
                if (interactable == null) continue;
                
                if (!interactable.activeSelf == false)
                {
                    return false;
                }
            }

            return true;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (!AssignmentsDataHandler.Instance.Exists(missionForSearchId)) return;
            
            
            if (!CheckIfInteractablesTaken())
            {
                multipleDialogue.Play();
            }
        }
    }
}
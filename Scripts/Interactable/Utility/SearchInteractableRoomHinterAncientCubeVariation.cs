using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Interactables.Objects.AncientCubes;
using UnityEngine;

namespace Interactable.Utility
{
    public class SearchInteractableRoomHinterAncientCubeVariation : MonoBehaviour
    {
        [SerializeField] private int missionForSearchId;
        [SerializeField] private AncientCube[] interactables;
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
            foreach (var cube in interactables)
            {
                if (cube == null) continue;
                
                if (cube != null)
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
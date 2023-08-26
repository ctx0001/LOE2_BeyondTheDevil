using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using UnityEngine;

namespace Assets.Scripts.Interactable.Utility
{
    public class SawElectricSwitchTrigger : MonoBehaviour
    {
        private bool _firstTime = true;

        private void OnTriggerEnter(Collider other)
        {
            // check if it is the first time
            if (!other.CompareTag("Player")) return;
            if (!_firstTime) return;
            _firstTime = false;

            // checks if assignment already exists
            if (AssignmentsDataHandler.Instance.Exists(37)){
                Destroy(gameObject);
                return;
            }

            // Create mission if not exist and destroy this
            var assignment = new Assignment(37, "Search for useful objects", null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment);
            Destroy(gameObject);
        }

    }
}

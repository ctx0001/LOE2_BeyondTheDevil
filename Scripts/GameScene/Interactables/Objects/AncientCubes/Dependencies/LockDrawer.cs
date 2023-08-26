using System.Collections;
using System.Collections.Generic;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using JetBrains.Annotations;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Objects.AncientCubes.Dependencies
{
    public class LockDrawer : MonoBehaviour
    {
        [SerializeField] private List<Lock> locks;
        [SerializeField] private GameObject openedMesh;

        [Header("Sounds")]
        [SerializeField] private GameObject drawerOpeningSound;
        [SerializeField] private GameObject drawerBlockedSound;

        private int _unlockedLocks;
        private bool _canUnlock = true;

        private IEnumerator Start()
        {
            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            if (AssignmentsDataHandler.Instance.IsCompleted(16))
            {
                openedMesh.SetActive(true);
                Destroy(gameObject);
            }
        }

        [UsedImplicitly]
        private void Interact()
        {
            StartCoroutine(TryUnlock());
        }
        
        private IEnumerator TryUnlock()
        {
            if (!_canUnlock) yield break;
            _canUnlock = false;
            
            var canUnlockDrawer = true;
            
            foreach (var singleLock in locks)
            {
                if (!singleLock.IsUnlocked())
                    canUnlockDrawer = false;
            }

            if (canUnlockDrawer)
            {
                Instantiate(drawerOpeningSound, transform.position, Quaternion.identity);
                var assignment = new Assignment(17, LocalizationManager.Instance.GetContent("179"), null, true, false);
                AssignmentsDataHandler.Instance.Create(assignment, 16);
                openedMesh.SetActive(true);
                Destroy(gameObject);
            }
            else
            {
                
                // Play an audio saying that i need first to insert the keys
                Instantiate(drawerBlockedSound, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(3f);
                _canUnlock = true;
            }
            
        }

        public void IncrementUnlockedLocks()
        {
            _unlockedLocks++;
        }

        public int GetUnlockedLocks()
        {
            return _unlockedLocks;
        }
        
    }
}
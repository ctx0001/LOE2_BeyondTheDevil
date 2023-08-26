using System;
using System.Collections;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Openables.CrossQuest
{
    public class SecretPassage : MonoBehaviour
    {
        private bool _isUnlocked;
        [SerializeField] private GameObject unlockSound;
        
        public void Unlock()
        {
            if (_isUnlocked) return;
            
            _isUnlocked = true;
            var position = transform.position;
            Instantiate(unlockSound, position, Quaternion.identity);
            
            var assignment = new Assignment(21, LocalizationManager.Instance.GetContent("183"), null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment, 20);
            
            StartCoroutine(MoveObject(new Vector3(23.06f, position.y, position.z), 5f));
        }

        private IEnumerator Start()
        {
            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            if (AssignmentsDataHandler.Instance.IsCompleted(20))
            {
                var position = transform.position;
                transform.position = new Vector3(23.06f, position.y, position.z);
            }
        }

        private IEnumerator MoveObject(Vector3 newPosition, float duration)
        {
            var elapsedTime = 0f;
            var startingPosition = transform.position;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var t = Mathf.Clamp01(elapsedTime / duration);
                transform.position = Vector3.Lerp(startingPosition, newPosition, t);
                yield return null;
            }

            transform.position = newPosition;
        }
    }
}
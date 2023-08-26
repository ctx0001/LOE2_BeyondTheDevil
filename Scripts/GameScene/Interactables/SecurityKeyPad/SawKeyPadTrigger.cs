using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Interactables.Objects.AncientCubes.Dependencies;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.SecurityKeyPad
{
    public class SawKeyPadTrigger : MonoBehaviour
    {
        [SerializeField] private MultipleDialogue multipleDialogue;
        [SerializeField] private SawDrawerTrigger sawDrawerTrigger;

        private bool _executed;
        private bool _firstTime = true;

        private IEnumerator WaitForDataLoad()
        {
            while (!DataManager.Instance.IsLoaded())
            {
                yield return null;
            }
            
            if(AssignmentsDataHandler.Instance.Exists(13)) Destroy(gameObject);
        }
        
        private void Start()
        {
            StartCoroutine(WaitForDataLoad());
        }

        public bool IsExecuted()
        {
            return _executed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_firstTime && other.CompareTag("Player"))
            {
                _firstTime = false;
                multipleDialogue.Play();
                StartCoroutine(WaitForEndOfSpeak());
            }
        }

        private IEnumerator WaitForEndOfSpeak()
        {
            yield return new WaitForSeconds(multipleDialogue.GetDialogueTime());
            var assignment = new Assignment(13, LocalizationManager.Instance.GetContent("104"), null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment);
            _executed = true;
            sawDrawerTrigger.SetListeningStatus(true);
        }
    }
}
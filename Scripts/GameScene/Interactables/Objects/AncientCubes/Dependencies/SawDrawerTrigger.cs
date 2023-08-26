using System;
using System.Collections;
using System.Collections.Generic;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Interactables.SecurityKeyPad;
using Interactable.SecurityKeyPad;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Objects.AncientCubes.Dependencies
{
    public class SawDrawerTrigger : MonoBehaviour
    {
        [SerializeField] private SawKeyPadTrigger sawKeyPadTrigger;
        [SerializeField] private MultipleDialogue multipleDialogue;
        private bool _firstTime = true;
        private bool _canListen;

        [SerializeField] private GameObject firstJumpscareFlash;

        private IEnumerator WaitForDataLoad()
        {
            while (!DataManager.Instance.IsLoaded())
            {
                yield return null;
            }
            
            if (AssignmentsDataHandler.Instance.IsCompleted(13)) Destroy(gameObject);
            if(AssignmentsDataHandler.Instance.IsCompleted(14)) Destroy(gameObject);
        }

        public void SetListeningStatus(bool canListen)
        {
            _canListen = canListen;
        }

        private void Start() => StartCoroutine(WaitForDataLoad());

        private void OnTriggerStay(Collider other)
        {
            if (!_canListen) return;
            if (!_firstTime || !other.CompareTag("Player")) return;
            if (!sawKeyPadTrigger.IsExecuted()) return;
            if (!AssignmentsDataHandler.Instance.Exists(13)) return;
            
            multipleDialogue.Play();
            firstJumpscareFlash.SetActive(true);

            var assignment = new Assignment(14, LocalizationManager.Instance.GetContent("101"), null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment, 13);
            _firstTime = false;
        }
    }
}
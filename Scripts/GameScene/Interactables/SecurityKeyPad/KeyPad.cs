using System;
using System.Collections;
using System.Collections.Generic;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using Localization;
using TMPro;
using UnityEngine;

namespace GameScene.Interactables.SecurityKeyPad
{
    public class KeyPad : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI codeText;
        [SerializeField] private GameObject openedDoorSound;
        [SerializeField] private Animator doorAnimator;
        [SerializeField] private string code = "7489";
        [SerializeField] private MultipleDialogue multipleDialogue1;
        [SerializeField] private MultipleDialogue multipleDialogue2;
        [SerializeField] private MultipleDialogue multipleDialogue3;

        private bool _canInsert = true;
        private string _currentCode = "";
        private bool _opened;
        private static readonly int Open = Animator.StringToHash("Open");

        private IEnumerator Start()
        {
            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }
            
            if(AssignmentsDataHandler.Instance.IsCompleted(17))
            {
                OpenDoor();
            }
        }

        private void TryCode()
        {
            if (_canInsert)
                StartCoroutine(TryOpen());
        }

        public void AddNumber(string num)
        {
            if (_currentCode.Length < 4)
            {
                _currentCode += num;
                UpdateCode();   
            }

            if(_currentCode.Length == 4)
                TryCode();
        }

        private void UpdateCode()
        {
            codeText.text = _currentCode;
        }
        
        public IEnumerator HintForDrawerCombination()
        {
            multipleDialogue1.Play();
            yield return new WaitForSeconds(30f);

            if (!_opened)
            {
                multipleDialogue2.Play();
                yield return new WaitForSeconds(35f);
            }
            
            if (!_opened)
            {
                multipleDialogue3.Play();
            }
        }
        
        private IEnumerator TryOpen()
        {
            _canInsert = false;
            if (codeText.text == code)
            {
                codeText.text = LocalizationManager.Instance.GetContent("105");
                _opened = true;
            }
            else
            {
                codeText.text = LocalizationManager.Instance.GetContent("106");
            }
            yield return new WaitForSeconds(3f);
            // TODO: play unlocked audio sound before wait
            if (!_opened)
            {
                _canInsert = true;
                _currentCode = "";
                codeText.text = "0";   
            }
            else
            {
                OpenDoor();
            }
        }

        private void OpenDoor()
        {
            doorAnimator.SetTrigger(Open);
            if (AssignmentsDataHandler.Instance.IsCompleted(17)) return;
            
            Instantiate(openedDoorSound, transform.position, Quaternion.identity);
            AssignmentsDataHandler.Instance.Complete(17);
        }
    }
}
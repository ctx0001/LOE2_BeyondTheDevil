using System;
using System.Collections;
using System.Collections.Generic;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using UnityEngine;

namespace Interactable.Utility
{
    public class BookFallTrigger : MonoBehaviour
    {
        [SerializeField] private Rigidbody book1;
        [SerializeField] private Rigidbody book2;
        [SerializeField] private Rigidbody book3;
        [SerializeField] private Rigidbody book4;
        [SerializeField] private Rigidbody book5;
        [SerializeField] private Rigidbody book6;

        [SerializeField] private Camera mainCamera;
        
        [SerializeField] private GameObject noteBook;
        [SerializeField] private GameObject officeDoll;
        [SerializeField] private Movement movement;
        [SerializeField] private MultipleDialogue multipleDialogue;
        
        private bool _firstTime = true;

        private AudioClip _checkBetter;

        private IEnumerator Start()
        {
            _checkBetter = Resources.Load<AudioClip>("Audio/check-better");
            _checkBetter.LoadAudioData();

            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            if (AssignmentsDataHandler.Instance.IsCompleted(8))
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && PlayerPrefs.GetInt("Balance") >= 200 && _firstTime)
            {
                if(!AssignmentsDataHandler.Instance.IsCompleted(6))
                    StartCoroutine(CreateAnimation());
                _firstTime = false;
            }else if (other.CompareTag("Player") && PlayerPrefs.GetInt("Balance") < 200 && AssignmentsDataHandler.Instance.Exists(8))
            {
                if (AssignmentsDataHandler.Instance.IsCompleted(6)) return;
                var cameraForward = mainCamera.transform.forward;
                
                if (Vector3.Dot(cameraForward, Vector3.right) > 0.8f) // check if door is open
                    SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("47"), _checkBetter);
            }
        }

        private IEnumerator CreateAnimation()
        {
            movement.SetMoveStatus(false);
            const float interval = 0.5f;
            var forward = transform.forward;
            
            book1.AddForce(forward * 5, ForceMode.Impulse);
            yield return new WaitForSeconds(interval);
            book2.AddForce(forward * 5, ForceMode.Impulse);
            yield return new WaitForSeconds(interval);
            book3.AddForce(forward * 5, ForceMode.Impulse);
            yield return new WaitForSeconds(interval);
            book4.AddForce(forward * 5, ForceMode.Impulse);
            yield return new WaitForSeconds(interval);
            book5.AddForce(forward * 5, ForceMode.Impulse);
            yield return new WaitForSeconds(interval);
            book6.AddForce(forward * 5, ForceMode.Impulse);
            
            movement.SetMoveStatus(true);
            AfterAnimation();
        }

        private IEnumerator Optimize()
        {
            yield return new WaitForSeconds(5f);
            Destroy(book1.GetComponent<BookFallListenerAudioPlayer>());
            Destroy(book2.GetComponent<BookFallListenerAudioPlayer>());
            Destroy(book3.GetComponent<BookFallListenerAudioPlayer>());
            Destroy(book4.GetComponent<BookFallListenerAudioPlayer>());
            Destroy(book5.GetComponent<BookFallListenerAudioPlayer>());
            Destroy(book6.GetComponent<BookFallListenerAudioPlayer>());
            
            Destroy(book1);
            Destroy(book2);
            Destroy(book3);
            Destroy(book4);
            Destroy(book5);
            Destroy(book6);
        }
        
        private void AfterAnimation()
        {
            multipleDialogue.Play();
            var assignment = new Assignment(6, LocalizationManager.Instance.GetContent("56"), null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment, 8);
            noteBook.SetActive(true);
            officeDoll.SetActive(false);
            
            //StartCoroutine(Optimize());
            Destroy(this, 10f);
        }
    }
}
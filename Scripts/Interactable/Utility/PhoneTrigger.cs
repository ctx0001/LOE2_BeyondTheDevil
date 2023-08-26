using System;
using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Interactables;
using Localization;
using UnityEngine;

namespace Interactable.Utility
{
    public class PhoneTrigger : MonoBehaviour
    {
        [SerializeField] private Telephone telephone;

        private bool _firstTime = true;

        private AudioClip _damnBetterAns;

        private IEnumerator Start()
        {
            
            _damnBetterAns = Resources.Load<AudioClip>("Audio/damn-better-ans");
            _damnBetterAns.LoadAudioData();

            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            if (AssignmentsDataHandler.Instance.IsCompleted(1))
            {
                telephone.gameObject.tag = "Untagged";
                Destroy(gameObject);
                Destroy(telephone);
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _firstTime)
            {
                if (!AssignmentsDataHandler.Instance.IsCompleted(1))
                {
                    if (!AssignmentsDataHandler.Instance.Exists(1))
                    {
                        var assignment = new Assignment(1, LocalizationManager.Instance.GetContent("3"), null, true, false);
                        AssignmentsDataHandler.Instance.Create(assignment, 0);   
                    }
                    telephone.TriggerPhoneCall();
                    var text = LocalizationManager.Instance.GetContent("1");
                    SpeakManager.Instance.SpeakSingle(text, _damnBetterAns, 1.5f);
                    _firstTime = false;   
                }
                else
                {
                    telephone.gameObject.tag = "Untagged";
                    Destroy(telephone);
                    Destroy(gameObject);
                }
            }
        }
    }
}
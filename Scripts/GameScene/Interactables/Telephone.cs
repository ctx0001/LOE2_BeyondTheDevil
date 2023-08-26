using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using JetBrains.Annotations;
using UnityEngine;

namespace GameScene.Interactables
{
    public class Telephone : MonoBehaviour
    {
        [SerializeField] private AudioSource ringtone;
        [SerializeField] private AudioSource message;
        [SerializeField] private GameObject radioTrigger;
        [SerializeField] private Movement movement;
        [SerializeField] private MultipleDialogue multipleDialogue;
        
        private bool _acceptedCall;

        internal void TriggerPhoneCall() => StartCoroutine(PhoneRoutine());

        private IEnumerator PhoneRoutine()
        {
            ringtone.Play();
            while (!_acceptedCall)
            {
                yield return new WaitForSeconds(0.1f);
            }
            ringtone.Stop();
            movement.SetMoveStatus(false);
            message.Play();
            AssignmentsDataHandler.Instance.Complete(1);
            AssignmentsDataHandler.Instance.UpdateNewActiveAssignment();
            yield return new WaitForSeconds(message.clip.length);
            radioTrigger.SetActive(true);
            movement.SetMoveStatus(true);
            multipleDialogue.Play();
            
            gameObject.tag = "Untagged";
            Destroy(this);
        }

        [UsedImplicitly]
        private void Interact()
        {
            _acceptedCall = true;
        }
    }
}
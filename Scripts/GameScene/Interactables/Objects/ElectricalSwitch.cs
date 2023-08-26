using Assets.Scripts.GameScene.Interactables.Openables.Doors;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using System.Collections;
using UnityEngine;
using Localization;

namespace Assets.Scripts.GameScene.Interactables.Objects
{
    public class ElectricalSwitch : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject handleSwitchSound;
        [SerializeField] private GameObject electricitySound;
        [SerializeField] private DrainingRoomDoor drainingRoomDoor;

        private AudioClip _switchedLight;

        private IEnumerator Start()
        {
            _switchedLight = Resources.Load<AudioClip>("Audio/after-electric-switch");
            _switchedLight.LoadAudioData();

            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            if (AssignmentsDataHandler.Instance.IsCompleted(37))
            {
                animator.SetTrigger("Open");
                gameObject.tag = "Untagged";
                drainingRoomDoor.Open();
            }
        }

        private void Interact()
        {
            animator.SetTrigger("Open");
            Instantiate(handleSwitchSound, transform.position, Quaternion.identity);
            gameObject.tag = "Untagged";

            if (!AssignmentsDataHandler.Instance.IsCompleted(37))
            {
                AssignmentsDataHandler.Instance.Complete(37);
                AssignmentsDataHandler.Instance.UpdateNewActiveAssignment();
            }

            StartCoroutine(OpenRoomRoutine());
        }

        private IEnumerator OpenRoomRoutine()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(electricitySound, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            drainingRoomDoor.Open();
            yield return new WaitForSeconds(1.5f);
            SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("233"), _switchedLight, 0.5f);
        }

    }
}

using System;
using System.Collections;
using GameScene.Data.Handlers;
using Managers;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Jumpscares.CorridorJumpscare
{
    public class CorridorJumpscareTrigger : MonoBehaviour
    {
        [SerializeField] private PlayableDirector cutscene;
        [SerializeField] private DollDataHandler dollDataHandler;
        [SerializeField] private Jumpscare jumpscare;
        [FormerlySerializedAs("jumpscareManager")] [SerializeField] private JumpscareDataHandler jumpscareDataHandler;
        [SerializeField] private ExitDoor exitDoor;
        private bool _firstTime = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag.Equals("Player") && _firstTime && AssignmentsDataHandler.Instance.Exists(18))
            {
                if (jumpscareDataHandler.SearchJumpScareStatus(0))
                {
                    StartCoroutine(JumpscareRoutine());
                    _firstTime = false;   
                }
            }
        }

        private IEnumerator JumpscareRoutine()
        {
            cutscene.gameObject.SetActive(true);
            cutscene.Play();
            yield return new WaitForSeconds((float) cutscene.duration);
            cutscene.Stop();
            yield return new WaitForSeconds(1f);
            dollDataHandler.InstantiateDoll(new Vector3(-10, 0, 19));
            MapManager.Instance.EnableLights();
            exitDoor.ActivatePlanks();
            jumpscare.UpdateStatus(false);
            Destroy(this);
        }
    }
}
using GameScene.Data.Handlers;
using Jumpscares;
using Managers;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class DoctorRoomExitJumpscareTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutscene;
    [SerializeField] private Jumpscare jumpscare;
    [SerializeField] private JumpscareDataHandler jumpscareDataHandler;
    private bool _firstTime = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player") && _firstTime && AssignmentsDataHandler.Instance.Exists(18))
        {
            if (jumpscareDataHandler.SearchJumpScareStatus(jumpscare.GetId()))
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
        yield return new WaitForSeconds((float)cutscene.duration);
        cutscene.Stop();
        jumpscare.UpdateStatus(false);
        Destroy(this);
    }
}
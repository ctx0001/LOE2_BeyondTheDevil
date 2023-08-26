using System.Collections;
using Common.Localization.Speaking;
using UnityEngine;
using UnityEngine.Playables;

namespace Jumpscares.HallJumpScare
{
    public class HallJumpscare : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private PlayableDirector jumpScareCutscene;
        [SerializeField] private MultipleDialogue multipleDialogue;

        public float dotThreshold = -0.6f;
        private bool _called;

        private void Update()
        {
            if (_called) return;
            var dotProduct = Vector3.Dot( mainCamera.transform.forward, Vector3.forward);

            if (!(dotProduct < dotThreshold)) return;
            StartCoroutine(StartJumpScare());
            _called = true;
        }

        private IEnumerator StartJumpScare()
        {
            
            jumpScareCutscene.Play();
            yield return new WaitForSeconds((float)jumpScareCutscene.duration);
            jumpScareCutscene.Stop();
            multipleDialogue.Play();
        }
    }
}
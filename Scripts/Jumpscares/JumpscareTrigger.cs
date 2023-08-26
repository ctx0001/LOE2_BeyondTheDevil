using GameScene.Data.Handlers;
using Jumpscares;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.Jumpscares
{
    public class JumpscareTrigger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayableDirector cutscene;
        [SerializeField] private Jumpscare jumpscare;
        private bool _firstTime = true;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !_firstTime) return;

            jumpscare.UpdateStatus(false);
            StartCoroutine(StartJumpscare());
            _firstTime = false;
        }

        private IEnumerator StartJumpscare()
        {
            cutscene.gameObject.SetActive(true);
            cutscene.Play();
            yield return new WaitForSeconds((float)cutscene.duration);
            cutscene.Stop();
            OnCutsceneEnd();
        }

        protected void OnCutsceneEnd()
        {
            Destroy(cutscene.gameObject);
        }
    }
}

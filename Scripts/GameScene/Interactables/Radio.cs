using System.Collections;
using GameScene.Data.Handlers;
using JetBrains.Annotations;
using UnityEngine;

namespace GameScene.Interactables
{
    public class Radio : MonoBehaviour
    {
        [SerializeField] private AudioSource radioTrack;

        public bool IsPlaying()
        {
            return radioTrack.isPlaying;
        }

        public void PlayTrack() => radioTrack.Play();

        private IEnumerator WaitForDataLoad()
        {
            while (!DataManager.Instance.IsLoaded())
            {
                yield return null;
            }
            
            if (AssignmentsDataHandler.Instance.IsCompleted(2))
            {
                gameObject.tag = "Untagged";
                Destroy(this);
            }
        }

        private void Start()
        {
            StartCoroutine(WaitForDataLoad());
        }

        [UsedImplicitly]
        private void Interact()
        {
            AssignmentsDataHandler.Instance.Complete(2);
            AssignmentsDataHandler.Instance.UpdateNewActiveAssignment();
            radioTrack.Stop();
            gameObject.tag = "Untagged";
            Destroy(this);
        }

    }
}
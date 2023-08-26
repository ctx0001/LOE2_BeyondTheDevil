using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Jumpscares.Flashes
{
    public class FlashJumpscareTrigger : MonoBehaviour
    {
        [SerializeField] private PlayableDirector cutscene;
        private bool _firstTime = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _firstTime)
            {
                cutscene.gameObject.SetActive(true);
                StartCoroutine(FlashJumpscare());
            }
        }

        private IEnumerator FlashJumpscare()
        {
            cutscene.Play();
            yield return new WaitForSeconds((float)cutscene.duration);
            cutscene.Stop();
            cutscene.gameObject.SetActive(false);
            _firstTime = false;

        }
        
    }
}
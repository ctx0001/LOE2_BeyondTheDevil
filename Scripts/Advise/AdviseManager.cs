using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Advise
{
    public class AdviseManager : MonoBehaviour
    {
        [SerializeField] private PlayableDirector adviseCutscene;
    
        private void Start()
        {
            StartCoroutine(ShowAdvise());
        }

        private IEnumerator ShowAdvise()
        {
            var sceneLoad = SceneManager.LoadSceneAsync("Menu");
            sceneLoad.allowSceneActivation = false;
        
            adviseCutscene.Play();
            yield return new WaitForSeconds((float)adviseCutscene.duration);
            adviseCutscene.Stop();

            sceneLoad.allowSceneActivation = true;
        }
    }
}

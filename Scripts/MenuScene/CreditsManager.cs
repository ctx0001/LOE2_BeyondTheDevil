using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Managers.Menu
{
    public class CreditsManager : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] private CameraShake cameraShake;
        [SerializeField] private Animator cameraAnimator;
        
        private AsyncOperation _sceneLoad;
        
        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            PreloadScene();
            StartCoroutine(CreditsRoutine());
        }

        private IEnumerator CreditsRoutine()
        {
            playableDirector.Play();
            // wait until the jumpScare
            yield return new WaitForSeconds((float)playableDirector.duration - 8.3f);
            Destroy(cameraAnimator);
            yield return StartCoroutine(cameraShake.Shake());
            yield return new WaitForSeconds(8.3f - cameraShake.GetDuration());
            playableDirector.Stop();

            if (PlayerPrefs.HasKey("RedirectToReviewPage"))
            {
                if (PlayerPrefs.GetInt("RedirectToReviewPage") == 1)
                {
                    Application.OpenURL("https://ctx1000.itch.io/legacy-of-evil-beyond-the-devil/rate");
                    PlayerPrefs.SetInt("RedirectToReviewPage", 0);
                }
            }

            // redirect to menu
            _sceneLoad.allowSceneActivation = true;
        }

        private void PreloadScene()
        {
            _sceneLoad = SceneManager.LoadSceneAsync("Loader");
            _sceneLoad.allowSceneActivation = false;
            PlayerPrefs.SetString("SceneToLoad", "Menu");
        }
    }
}
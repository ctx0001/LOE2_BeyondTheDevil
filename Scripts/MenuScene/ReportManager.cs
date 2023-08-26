using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MenuScene
{
    public class ReportManager : MonoBehaviour
    {
        private AsyncOperation _sceneLoad;

        private void Start()
        {
            _sceneLoad = SceneManager.LoadSceneAsync("Loader");
            _sceneLoad.allowSceneActivation = false;
        }

        public void BackToMenu()
        {
            PlayerPrefs.SetString("SceneToLoad", "Menu");
            _sceneLoad.allowSceneActivation = true;
        }
    }
}

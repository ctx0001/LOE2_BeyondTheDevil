using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoaderScene
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loadingProgressText;
        [SerializeField] private Slider loadingProgressSlider;

        private void Start()
        {
            if (PlayerPrefs.HasKey("QualityLevel"))
            {
                QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"), true);   
            }
            else
            {
                QualitySettings.SetQualityLevel(1, true);   
            }
            
            
            LoadScene();
        }
        
        private void LoadScene()
        {
            var sceneNames = new List<string>() {"Menu", "Loader", "Game", "Credits", "Advise", "Report"}; 
            var sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
            Debug.Log("Loading scene: " + sceneToLoad);
            
            
            StartCoroutine(sceneNames.Contains(sceneToLoad)
                ? LoadSceneRoutine(sceneToLoad)
                : LoadSceneRoutine("Menu"));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            PlayerPrefs.SetString("SceneToLoad", "none");
            var operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                yield return null;
                loadingProgressSlider.value = operation.progress;
                loadingProgressText.text = Math.Ceiling(operation.progress * 100) + "%";
            }
            
            operation.allowSceneActivation = true;
        }
    }
}
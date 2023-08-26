using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private GameObject pauseMenuSound;
        [SerializeField] private AudioMixerGroup dollVoice;

        private AsyncOperation _loaderSceneLoad;
        
        private void Start()
        {
            _loaderSceneLoad = SceneManager.LoadSceneAsync("Loader");
            _loaderSceneLoad.allowSceneActivation = false;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.P)) return;
            
            if(Math.Abs(Time.timeScale - 1) < 0.1)
                EnablePauseState();
        }

        public void BackToMenu()
        {
            DisablePauseState();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerPrefs.SetString("SceneToLoad", "Menu");
            _loaderSceneLoad.allowSceneActivation = true;
        }

        public void ReportProblem()
        {
            DisablePauseState();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerPrefs.SetString("SceneToLoad", "Report");
            _loaderSceneLoad.allowSceneActivation = true;
        }

        private IEnumerator ReloadSceneRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            PlayerPrefs.SetString("SceneToLoad", "Game");
            _loaderSceneLoad.allowSceneActivation = true;
        }
        
        public void SendToScene(string sceneName)
        {
            PlayerPrefs.SetString("SceneToLoad", sceneName);
            _loaderSceneLoad.allowSceneActivation = true;
        }

        public void ReloadScene(float delay)
        {
            StartCoroutine(ReloadSceneRoutine(delay));
        }

        public void ExitFromGame() => Application.Quit();

        private void PlaySound()
        {
            var pauseSound = Instantiate(pauseMenuSound, Vector3.zero, quaternion.identity);
            Destroy(pauseSound, 3f);
        }
        
        private void EnablePauseState()
        {
            dollVoice.audioMixer.SetFloat("DollVoice", -80);
            PlaySound();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            audioManager.EnablePauseAudio();
            pauseMenu.SetActive(true);
        }

        public void DisablePauseState()
        {
            dollVoice.audioMixer.SetFloat("DollVoice", -3);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            audioManager.EnableAmbienceAudio();
            pauseMenu.SetActive(false);
        }
    }
}
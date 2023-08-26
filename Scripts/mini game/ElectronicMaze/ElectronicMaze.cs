using System.Collections;
using Interactable.Utility;
using Managers;
using UnityEngine;

namespace mini_game.ElectronicMaze
{
    public class ElectronicMaze : MonoBehaviour
    {
        [SerializeField] private Transform ballTransformParent;
        [SerializeField] private GameObject ballPrefab;

        [SerializeField] private GameObject mazeCanvas;
        [SerializeField] private AudioSource gameMusic;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private MetalDoorControllerExternal metalDoorControllerExternal;
        [SerializeField] private GameObject successAlert;

        private bool _running;

        private GameObject _ball;
        
        public void StartGame()
        {
            _running = true;
            mazeCanvas.SetActive(true);
            _ball = Instantiate(ballPrefab, new Vector2(150, 135), Quaternion.identity);
            _ball.transform.SetParent(ballTransformParent);
            
            successAlert.SetActive(false);
            audioManager.DisableAllAudio();
            gameMusic.Play();
        }

        public bool IsRunning()
        {
            return _running;
        }
        
        public void ResetAndClose()
        {
            successAlert.SetActive(true);
            gameMusic.Stop();
            audioManager.EnableAmbienceAudio();
            mazeCanvas.SetActive(false);
            _running = false;
            Destroy(_ball);
        }

        public void Win()
        {
            StartCoroutine(WinRoutine());
        }
        
        private IEnumerator WinRoutine()
        {
            successAlert.SetActive(true);
            gameMusic.Stop();
            audioManager.EnableAmbienceAudio();
            yield return new WaitForSeconds(2.3f);
            mazeCanvas.SetActive(false);
            metalDoorControllerExternal.Unlock();
            _running = false;
            Destroy(_ball);
        }
        
    }
}
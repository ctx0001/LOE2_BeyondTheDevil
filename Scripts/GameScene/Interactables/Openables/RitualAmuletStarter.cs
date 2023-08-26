using System;
using System.Collections;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using Managers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace GameScene.Interactables.Openables
{
    public class RitualAmuletStarter : MonoBehaviour
    {
        [SerializeField] private PlayableDirector endOfGameCutscene;
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject canvasManager;
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject dollDead;
        [SerializeField] private AudioMixerGroup dollMessages;
        
        private IEnumerator Start()
        {
            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            if (AssignmentsDataHandler.Instance.Exists(23)){
                gameObject.tag = "Untagged";
                dollDead.SetActive(true);
            }
        }

        private void Interact()
        {
            AssignmentsDataHandler.Instance.Complete(22);
            
            canvasManager.SetActive(false);
            player.SetActive(false);
            dollMessages.audioMixer.SetFloat("DollVoice", -80);
            
            gameObject.tag = "Untagged";
            StartCoroutine(ShowAnimation());
        }

        private IEnumerator ShowAnimation()
        {
            endOfGameCutscene.Play();
            yield return new WaitForSeconds((float)endOfGameCutscene.duration);
            endOfGameCutscene.Stop();

            var assignment = new Assignment(23, LocalizationManager.Instance.GetContent("211"), null, true, false);
            if(!AssignmentsDataHandler.Instance.Exists(23))
                AssignmentsDataHandler.Instance.Create(assignment);

            dollDead.SetActive(true);
            canvas.SetActive(false);
            canvasManager.SetActive(true);
            player.SetActive(true);
        }
    }
}
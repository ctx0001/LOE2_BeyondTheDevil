using System.Collections;
using DefaultNamespace;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using JetBrains.Annotations;
using Localization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace GameScene.Interactables.Openables
{
    public class RitualObjectPlacer : MonoBehaviour
    {
        [SerializeField] private PlayableDirector placeObjectCutscene;
        [SerializeField] private GameObject ritualsObjects;
        
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private GameObject canvasManager;
        [SerializeField] private AudioMixerGroup dollMessages;
        [SerializeField] private DollDataHandler dollDataHandler;


        private IEnumerator Start()
        {
            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            if (AssignmentsDataHandler.Instance.IsCompleted(21))
            {
                AfterInteraction();
            }
        }

        [UsedImplicitly]
        private void Interact()
        {
            canvasManager.SetActive(false);
            mainCamera.SetActive(false);
            dollMessages.audioMixer.SetFloat("DollVoice", -80);

            // Destroy the doll to prevent doing something
            Transform dollReference = GameObject.Find("EnemyDoll_new(Clone)").GetComponent<Transform>();
            dollReference.gameObject.SetActive(false);
            dollDataHandler.SetDollStatus(false);

            StartCoroutine(ShowAnimation());
        }

        private void AfterInteraction()
        {
            ritualsObjects.SetActive(true);
            Destroy(placeObjectCutscene.gameObject);
            gameObject.tag = "Untagged";
        }

        private IEnumerator ShowAnimation()
        {
            placeObjectCutscene.Play();
            yield return new WaitForSeconds((float)placeObjectCutscene.duration);
            placeObjectCutscene.Stop();
            canvasManager.SetActive(true);
            mainCamera.SetActive(true);
            dollMessages.audioMixer.SetFloat("DollVoice", -3);
            
            InventoryDataHandler.Instance.RemoveItemByName("amulet", 1); // this might create a glitch if is not the proper name of the item
            InventoryDataHandler.Instance.RemoveItemByName("Candle", 6);
            InventoryDataHandler.Instance.RemoveItemByName("Incense", 3);
            AfterInteraction();

            var assignment = new Assignment(22, LocalizationManager.Instance.GetContent("199"), null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment, 21);
        }
    }
}
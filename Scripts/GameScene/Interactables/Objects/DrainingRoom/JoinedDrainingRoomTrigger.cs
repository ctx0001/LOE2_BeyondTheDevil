using Assets.Scripts.GameScene.Interactables.Openables.Doors;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.GameScene.Interactables.Objects.DrainingRoom
{
    internal class JoinedDrainingRoomTrigger : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource waterRaisingAmbience;
        [SerializeField] private AudioSource hydropumpSound;
        [SerializeField] private AudioSource alarmAmbience;
        [SerializeField] private GameObject openedDoorSound;

        [Header("Effects")]
        [SerializeField] private Light drainingRoomLight;
        [SerializeField] private GameObject water;

        [Header("Dialogues")]
        [SerializeField] private MultipleDialogue multipleDialogue;
        [SerializeField] private MultipleDialogue multipleDialogue1;

        [Header("References")]
        [SerializeField] private DrainingRoomDoor drainingRoomDoor;
        [SerializeField] private Movement movement;

        [Header("Patches")]
        [SerializeField] private List<GameObject> keypadKeys;
        [SerializeField] private Transform playerPositionReference;
        [SerializeField] private GameObject morgueKey;

        [Header("Cutscenes")]
        [SerializeField] private PlayableDirector drownedCutscene;

        private bool _inQuest;
        private bool _firstTime = true;
        private bool _drawned;

        private IEnumerator Start()
        {
            // Wait for data load
            while(!AssignmentsDataHandler.Instance.IsLoaded()) yield return null;

            if (AssignmentsDataHandler.Instance.Exists(38))
            {
                if (AssignmentsDataHandler.Instance.IsCompleted(38))
                {
                    DisableKeypad();
                    morgueKey.tag = "Interactable";
                    Destroy(gameObject);
                }
                else
                {
                    movement.transform.position = playerPositionReference.position;
                    StartCoroutine(StartDrainingRoomQuest());
                }
            }
        }

        private void DisableKeypad()
        {
            foreach (var key in keypadKeys)
            {
                key.tag = "Untagged";
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (!_firstTime) return;
            if (_inQuest) return;

            _firstTime = false;
            StartCoroutine(StartDrainingRoomQuest());
        }

        private IEnumerator StartDrainingRoomQuest()
        {
            _inQuest = true;

            // create assignment if not exist
            if (!AssignmentsDataHandler.Instance.Exists(38))
            {
                var assignment = new Assignment(38, LocalizationManager.Instance.GetContent("229"), null, true, false);
                AssignmentsDataHandler.Instance.Create(assignment);
            }
         
            drainingRoomDoor.Close();
            yield return new WaitForSeconds(2f);

            // Activate AudioSources
            hydropumpSound.Play();
            alarmAmbience.Play();
            waterRaisingAmbience.Play();

            // Start Routines
            StartCoroutine(LightScatteringRoutine());
            StartCoroutine(RaiseWaterLevel());

            yield return new WaitForSeconds(1f);
            multipleDialogue.Play();
        }

        public IEnumerator OnRiddleSolved()
        {
            if (_drawned) yield break;
            StopAllCoroutines();
            drainingRoomLight.color = new Color(1, 1, 1);
            drainingRoomLight.intensity = 1f;
            hydropumpSound.Stop();
            alarmAmbience.Stop();

            StartCoroutine(LowerWaterLevel());
        }

        private IEnumerator LowerWaterLevel()
        {
            waterRaisingAmbience.volume = 0.5f;
            float targetHeight = 3.025f;
            float durationInSeconds = 20f;
            float startingHeight = water.transform.position.y;
            float timeElapsed = 0f;

            while (timeElapsed < durationInSeconds)
            {
                timeElapsed += Time.deltaTime;
                float t = timeElapsed / durationInSeconds;
                float newY = Mathf.Lerp(startingHeight, targetHeight, t);
                water.transform.position = new Vector3(water.transform.position.x, newY, water.transform.position.z);
                yield return null;
            }

            // y = 3.025f
            waterRaisingAmbience.Stop();
            water.SetActive(false);
            water.transform.position = new Vector3(water.transform.position.x, 3.025f, water.transform.position.z);

            drainingRoomDoor.Open();
            Instantiate(openedDoorSound, transform.position, Quaternion.identity);
            DisableKeypad();

            AssignmentsDataHandler.Instance.Complete(38);
            multipleDialogue1.Play();
            morgueKey.tag = "Interactable";
        }

        private IEnumerator RaiseWaterLevel()
        {
            water.SetActive(true);
            float targetHeight = 1.5f;
            float durationInMinutes = 2f;
            float startingHeight = water.transform.position.y;
            float timeElapsed = 0f;

            while (timeElapsed < durationInMinutes * 60f)
            {
                timeElapsed += Time.deltaTime;
                float t = timeElapsed / (durationInMinutes * 60f);
                float newY = Mathf.Lerp(startingHeight, startingHeight + targetHeight, t);
                water.transform.position = new Vector3(water.transform.position.x, newY, water.transform.position.z);
                yield return null;
            }

            // set water height
            water.transform.position = new Vector3(water.transform.position.x, startingHeight + targetHeight, water.transform.position.z);

            _drawned = true;
            drownedCutscene.gameObject.SetActive(true);
            drownedCutscene.transform.GetChild(0).position = new Vector3(movement.transform.position.x, movement.transform.position.y + 1.5f, movement.transform.position.z);
            movement.Die();

            drownedCutscene.Play();
            yield return new WaitForSeconds((float) drownedCutscene.duration);
            drownedCutscene.Stop();
            alarmAmbience.Stop();
            waterRaisingAmbience.Stop();
            hydropumpSound.Stop();
        }

        private IEnumerator LightScatteringRoutine()
        {
            drainingRoomLight.color = new Color(0.945f, 0.301f, 0.301f);
            float transitionDuration = 1f;

            while (_inQuest)
            {
                float elapsedTime = 0f;
                float startIntensity = 0f;
                float targetIntensity = 1f;

                while (elapsedTime < transitionDuration)
                {
                    drainingRoomLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / transitionDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                drainingRoomLight.intensity = targetIntensity;

                yield return new WaitForSeconds(1f);

                elapsedTime = 0f;
                startIntensity = 1f;
                targetIntensity = 0f;

                while (elapsedTime < transitionDuration)
                {
                    drainingRoomLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / transitionDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                drainingRoomLight.intensity = targetIntensity;

                yield return new WaitForSeconds(1f);
            }
        }

    }
}

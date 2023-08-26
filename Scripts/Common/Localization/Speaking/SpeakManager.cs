using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameScene.Player;
using Localization;
using TMPro;
using UnityEngine;

namespace Common.Localization.Speaking
{
    public class SpeakManager : MonoBehaviour
    {
        [SerializeField] private GameObject speakPanel;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private AudioSource dialogueSource;

        [SerializeField] private Interaction _interaction;
        public static SpeakManager Instance;
        private bool _busy;
        private List<DialogueQueueItem> _queue = new List<DialogueQueueItem>();

        private AudioClip _noSecuritySystem;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void SetBusyState(bool state)
        {
            _busy = state;
        }

        public bool GetBusyState()
        {
            return _busy;
        }

        public void ActivateSpeakPanel()
        {
            speakPanel.SetActive(true);
            subtitleText.text = "";
        }

        public void DisableSpeakPanel()
        {
            speakPanel.SetActive(false);
        }

        public void AddCharacterToSubtitle(char character)
        {
            subtitleText.text += character;
        }

        public void ActivateInteraction()
        {
            _interaction.AllowInteraction(true);
        }
        
        public void DisableInteraction()
        {
            _interaction.AllowInteraction(false);
        }

        private IEnumerator WaitForDataLoad()
        {
            while (!LocalizationManager.Instance.IsLoaded())
            {
                yield return null;
            }
            
            if (PlayerPrefs.GetInt("FirstGameLoad") == 1)
            {
                var text = LocalizationManager.Instance.GetContent("0");
                SpeakSingle(text, _noSecuritySystem, 2f);
                PlayerPrefs.SetInt("FirstGameLoad", 0);
            }
        }
        
        private void Start()
        {
            _noSecuritySystem = Resources.Load<AudioClip>("Audio/no-sec-sys");
            _noSecuritySystem.LoadAudioData();
            
            StartCoroutine(WaitForDataLoad());
        }

        /// <summary>
        /// Starts a coroutine that speaks a single line of dialogue with the specified text and audio clip.
        /// </summary>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="audioClip">The audio clip to be played.</param>
        /// <param name="delayBeforePlay">The delay before playing the audio clip (default: 0f).</param>
        /// <returns>An IEnumerator that can be used to wait for the coroutine to complete.</returns>
        public void SpeakSingle(string text, AudioClip audioClip, float delayBeforePlay = 0f)
        {
            StartCoroutine(CreateNewDialogueRoutine(text, audioClip, delayBeforePlay));
        }

        /// <summary>
        /// Coroutine that creates a new dialogue with text and audio clip. Waits for a specified delay before playing
        /// the audio clip and displaying the text.
        /// </summary>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="audioClip">The audio clip to be played.</param>
        /// <param name="delayBeforePlay">The delay before playing the audio clip (default: 0f).</param>
        /// <returns>
        /// An IEnumerator that can be used to wait for the coroutine to complete.
        /// </returns>
        private IEnumerator CreateNewDialogueRoutine(string text, AudioClip audioClip, float delayBeforePlay=0f)
        {
            if (!_busy)
            {
                _busy = true;
                yield return new WaitForSeconds(delayBeforePlay);

                speakPanel.SetActive(true);
                dialogueSource.clip = audioClip;
                dialogueSource.Play();

                // Show the text passed
                var time = 0f;
                subtitleText.text = "";
                foreach (var character in text)
                {
                    subtitleText.text += character;
                    yield return new WaitForSeconds(0.05f);
                    time += 0.05f;
                }
            
                // Checks if dialogue audio is still reproducing
                if(dialogueSource.clip.length - time > 0)
                    yield return new WaitForSeconds(dialogueSource.clip.length - time);
            
                dialogueSource.Stop();
                speakPanel.SetActive(false);

                _busy = false;
                if (_queue.Count > 0)
                {
                    DialogueQueueItem queueItem = _queue.First();
                    StartCoroutine(CreateNewDialogueRoutine(queueItem.GetText(), queueItem.GetAudioClip(),
                        queueItem.GetDelay()));
                    _queue.Remove(queueItem);
                }
            }
            else
            {
                //Debug.Log("Added to queue: " + text);
                _queue.Add(new DialogueQueueItem(text, audioClip, delayBeforePlay));
            }
        }
        
        public class DialogueQueueItem
        {
            private string _text;
            private AudioClip _audioClip;
            private float _delay;

            public DialogueQueueItem(string text, AudioClip audioClip, float delay)
            {
                _text = text;
                _audioClip = audioClip;
                _delay = delay;
            }
            
            public string GetText() => _text;
            public AudioClip GetAudioClip() => _audioClip;
            public float GetDelay() => _delay;
        }
    }
}
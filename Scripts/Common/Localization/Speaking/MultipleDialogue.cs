using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Localization;
using UnityEngine;

namespace Common.Localization.Speaking
{
    public class MultipleDialogue : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> audioClips;
        [SerializeField] private List<int> subtitlesId;
        [SerializeField] private AudioSource audioSource;

        [Header("Settings")] 
        [SerializeField][Range(0, 5f)] private float delayBeforePlay;
        [SerializeField] private bool initializeData = true;
        [SerializeField][Range(0, 1f)] private float intervalFromClips = 0.3f;

        private void Start()
        {
            if (!initializeData) return;
            foreach (var audioClip in audioClips)
            {
                audioClip.LoadAudioData();
            }
        }

        private static IEnumerator ShowSubtitles(float audioDuration, string text)
        {
            var timeForCharacter = audioDuration / text.Length;

            if (timeForCharacter > 0.05f)
                timeForCharacter = 0.05f;
            
            SpeakManager.Instance.ActivateSpeakPanel();
            foreach (var character in text)
            {
                SpeakManager.Instance.AddCharacterToSubtitle(character);
                yield return new WaitForSeconds(timeForCharacter);
            }
        }

        public void Play()
        {
            StartCoroutine(PlayDialoguesRoutine());
        }

        public float GetDialogueTime()
        {
            return audioClips.Sum(clip => clip.length);
        }
        
        private IEnumerator PlayDialoguesRoutine()
        {
            SpeakManager.Instance.DisableInteraction();
            yield return new WaitForSeconds(delayBeforePlay);
            
            foreach (var audioClip in audioClips)
            {
                var subtitleId = subtitlesId[audioClips.IndexOf(audioClip)];
                var text = LocalizationManager.Instance.GetContent(subtitleId.ToString());
                
                var subtitleRoutine = StartCoroutine(ShowSubtitles(audioClip.length, text));
                audioSource.clip = audioClip;
                audioSource.Play();
                yield return new WaitForSeconds(audioClip.length + intervalFromClips);
                audioSource.Stop();
                StopCoroutine(subtitleRoutine);
                SpeakManager.Instance.DisableSpeakPanel();
            }
            SpeakManager.Instance.ActivateInteraction();
        }
    }
}
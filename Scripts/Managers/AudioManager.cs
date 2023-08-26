using System;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource ambienceSource;
        
        [SerializeField] private AudioClip ambienceAudio;
        [SerializeField] private AudioClip pauseAudio;

        [SerializeField] private AudioSource bathroomSingSound;
        [SerializeField] private AudioSource playerScaredSound;
        
        public static AudioManager Instance;

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

        private void Start()
        {
            EnableAmbienceAudio();
        }

        public void EnableAmbienceAudio()
        {
            ambienceSource.clip = ambienceAudio;
            ambienceSource.Play();
        }
        
        public void EnablePauseAudio()
        {
            ambienceSource.clip = pauseAudio;
            ambienceSource.Play();
        }

        public void DisableAllAudio()
        {
            ambienceSource.Stop();
        }

        public void EnableBathroomSingSound()
        {
            bathroomSingSound.Play();   
        }
        
        public void DisableBathroomSingSound()
        {
            bathroomSingSound.Stop();   
        }
        
        public void EnableScaredSound()
        {
            playerScaredSound.Play();   
        }
        
        public void DisableScaredSound()
        {
            playerScaredSound.Stop();   
        }

    }
}
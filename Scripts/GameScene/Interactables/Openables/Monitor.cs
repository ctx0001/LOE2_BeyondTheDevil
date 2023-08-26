using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Enemy;
using GameScene.Pc;
using Localization;
using Managers;
using UnityEngine;
using UnityEngine.Audio;

namespace GameScene.Interactables.Openables
{
    public class Monitor : Openable
    {
        [SerializeField] private GameObject computerCanvas;
        [SerializeField] private DesktopHandler desktopHandler;
        [SerializeField] private AudioMixerGroup dollVoice;
        
        private bool _isOn;
        private AudioClip _objectNotUseful;
        private float _canInteract1 = -1f; // this might cause glitches

        private void Start()
        {
            _objectNotUseful = Resources.Load<AudioClip>("Audio/object-not-useful");
            _objectNotUseful.LoadAudioData();
        }

        protected override void Interact()
        { 
            if (!(Time.time > _canInteract1)) return;
            if (AssignmentsDataHandler.Instance.Exists(19) && !_isOn)
            {
                dollVoice.audioMixer.SetFloat("DollVoice", -80);
                var doll = GameObject.Find("EnemyDoll_new(Clone)").GetComponent<DollEnemy>();
                doll.SetTalkState(false);
                computerCanvas.SetActive(true);  
                AudioManager.Instance.DisableAllAudio();
                _isOn = true;
            
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                StartCoroutine(desktopHandler.UpdateComputerTime());
            }
            else if(!AssignmentsDataHandler.Instance.Exists(19))
            {
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("117"), _objectNotUseful, 0.3f);
            }
            _canInteract1 = Time.time + 3f;
        }

        private void Update()
        {
            if (!_isOn) return;
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            
            dollVoice.audioMixer.SetFloat("DollVoice", -3);
            var doll = GameObject.Find("EnemyDoll_new(Clone)").GetComponent<DollEnemy>();
            doll.SetTalkState(true);
            computerCanvas.SetActive(false);
            AudioManager.Instance.EnableAmbienceAudio();
            _isOn = false;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
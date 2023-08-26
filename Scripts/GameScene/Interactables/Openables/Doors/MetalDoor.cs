using System;
using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using Localization;
using UnityEngine;
using UnityEngine.AI;

namespace GameScene.Interactables.Openables.Doors
{
    public class MetalDoor : Openable
    {
        [Header("Settings")] 
        [SerializeField] internal string id;

        [Header("Debug Settings")]
        [SerializeField] internal bool unlocked;

        [Header("Animation References")] 
        [SerializeField] private Animator doorAnimator;
        [Tooltip("The box collider attached to the door component")]
        [SerializeField] private BoxCollider boxCollider;

        private bool _isOpen;
        private AudioClip _metalDoorLocked;

        [SerializeField] private PowerBox powerBox;
        [SerializeField] private NavMeshObstacle navMeshObstacle;

        private static readonly int Open1 = Animator.StringToHash("Open");

        private IEnumerator Start()
        {
            while (!DoorDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }
            
            unlocked = DoorDataHandler.Instance.IsDoorUnlocked(id);
            _metalDoorLocked = Resources.Load<AudioClip>("Audio/metal-door-blocked");
            _metalDoorLocked.LoadAudioData();

            if (unlocked)
            {
                Open();
            }
        }

        protected override void Interact()
        {
            if (_isOpen) return;
            if(_isOpen)
                Open();
            else
                TryToOpen();
        }

        private void TryToOpen()
        {
            if (!CanInteract()) return;
            if (powerBox.IsOpened()) return;
            SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("10"), _metalDoorLocked, 0.5f);
            UpdateCanInteract();
        }
        
        public void Open()
        {
            if (!unlocked && !_isOpen) return;
            doorAnimator.SetTrigger(Open1);
            Destroy(boxCollider);
            Destroy(navMeshObstacle);
            _isOpen = true;
        }
    }
}
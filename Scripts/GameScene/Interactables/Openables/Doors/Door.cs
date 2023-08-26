using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using UnityEngine;
using UnityEngine.AI;

namespace GameScene.Interactables.Openables.Doors
{
    public class Door : Openable
    {
        [Header("Settings")] 
        [SerializeField] private string id;
        [SerializeField] private string doorName;
        
        [Header("Debug Settings")]
        [SerializeField] private bool unlocked;

        [Header("Animation References")]
        [SerializeField] private GameObject closedDoor;
        [Tooltip("The box collider attached to the door component")]
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private NavMeshObstacle navMeshObstacle;

        private bool _isOpen;

        [Header("Audio")]
        [Tooltip("Played when player try to open a door that is locked")]
        [SerializeField] private GameObject lockedDoorSound;
        [Tooltip("Played when player open a door with the key")]
        [SerializeField] private GameObject openedDoorSound;

        [SerializeField] private Animator leftDoor;
        [SerializeField] private Animator rightDoor;
        
        private AudioClip _objHoldHand;
        private AudioClip _lockedDoor;
        
        private IEnumerator Start()
        {
            while (!DoorDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }
            
            // Initialize from data
            unlocked = DoorDataHandler.Instance.IsDoorUnlocked(id);
            
            if (unlocked)
            {
                Open(false);
            }

            _objHoldHand = Resources.Load<AudioClip>("Audio/obj-hold-hand");
            _objHoldHand.LoadAudioData();

            _lockedDoor = Resources.Load<AudioClip>("Audio/locked-door");
            _lockedDoor.LoadAudioData();
        }

        protected override void Interact()
        {
            if (!Input.GetKeyDown(KeyCode.E)) return;
            Unlock();
        }
        
        private void Unlock()
        {
            if (!CanInteract()) return;

            if (HasItem(doorName))
            {
                if (HasItemInHand(doorName))
                {
                    unlocked = true;
                    DoorDataHandler.Instance.UpdateDoorState(id, unlocked, false);
                    RemoveItem(doorName);
                    
                    if (doorName == "hall")
                    {
                        if (!AssignmentsDataHandler.Instance.Exists(9))
                        {
                            var assignment = new Assignment(9, LocalizationManager.Instance.GetContent("91"), null, true, false);
                            AssignmentsDataHandler.Instance.Create(assignment);
                        }
                    }
                    
                    Open();
                }
                else
                {
                    SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("7"), _objHoldHand, 0.5f);
                    UpdateCanInteract();
                }
            }
            else
            {
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("9"),_lockedDoor, 0.5f);
                Instantiate(lockedDoorSound, transform.position, Quaternion.identity);
                UpdateCanInteract();
            }
        }
        
        private void Open(bool unlockWithSound=true)
        {
            if (!unlocked && !_isOpen) return;
            
            if(unlockWithSound)
                Instantiate(openedDoorSound, transform.position, Quaternion.identity);
            
            closedDoor.SetActive(true);
            leftDoor.SetTrigger("Open");
            rightDoor.SetTrigger("Open");
            Destroy(boxCollider);
            Destroy(navMeshObstacle);
            _isOpen = true;
        }
    }
}
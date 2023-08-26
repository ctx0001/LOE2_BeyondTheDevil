using System;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Openables
{
    public class PowerBox : Openable
    {
        [Header("Debug Settings")] 
        [SerializeField] private bool isOpened;

        [SerializeField] private GameObject closed;
        [SerializeField] private GameObject opened;
        [SerializeField] private BoxCollider boxCollider;
        
        private AudioClip _objHoldHand;
        private AudioClip _powerBoxNotOpen;
        private AudioClip _objectNotUseful;
        
        public bool IsOpened() => isOpened;

        private void Start()
        {
            try
            {
                _objHoldHand = Resources.Load<AudioClip>("Audio/obj-hold-hand");
                _objHoldHand.LoadAudioData();
                
                _powerBoxNotOpen = Resources.Load<AudioClip>("Audio/powerbox-not-open");
                _powerBoxNotOpen.LoadAudioData();

                _objectNotUseful = Resources.Load<AudioClip>("Audio/object-not-useful");
                _objectNotUseful.LoadAudioData();
            }
            catch (NullReferenceException)
            {
                //TODO: Handle the exception in production
                Debug.Log("No audio found in -> " + gameObject.name);
            }
        }

        protected override void Interact()
        {
            isOpened = DoorDataHandler.Instance.IsDoorUnlocked("5");
            if (!isOpened)
            {
                TryOpen();
            }
            else
            {
                Open();
            }
        }

        private void TryOpen()
        {
            if (!CanInteract()) return;

            var hasScrewdriver = InventoryDataHandler.Instance.CheckIfItemIsInInventory("screwdriver");
            if (hasScrewdriver)
            {
                var item = InventoryDataHandler.Instance.SearchItem("screwdriver");
                if (InventoryDataHandler.Instance.CheckIfItemIsInHand(item))
                {
                    isOpened = true;
                    DoorDataHandler.Instance.UpdateDoorState("5", isOpened, false);
                    InventoryDataHandler.Instance.RemoveItemByName("screwdriver");
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
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("11"), _powerBoxNotOpen, 0.5f);
                if (AssignmentsDataHandler.Instance.IsCompleted(38))
                {
                    if (!AssignmentsDataHandler.Instance.Exists(4))
                    {
                        var assignment = new Assignment(4, LocalizationManager.Instance.GetContent("12"), null, true, false);
                        AssignmentsDataHandler.Instance.Create(assignment);
                    }

                }
                else
                {
                    SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("117"), _objectNotUseful, 0.3f);
                }

                UpdateCanInteract();
            }
        }

        private void Open()
        {
            closed.SetActive(false);
            opened.SetActive(true);

            Destroy(boxCollider);
            gameObject.tag = "Untagged";
            Destroy(this);
        }
        
    }
}
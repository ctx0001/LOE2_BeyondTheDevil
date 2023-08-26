using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using JetBrains.Annotations;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Objects.AncientCubes.Dependencies
{
    public class Lock : MonoBehaviour
    {
        [SerializeField] private GameObject keyModel;

        private AudioClip _objHoldHand;
        private AudioClip _firstFindTheKey;
        
        private bool _isUnlocked;
        private bool _canInteract = true;

        [SerializeField] private GameObject UnlockLockSound;
        
        private void Start()
        {
            _objHoldHand = Resources.Load<AudioClip>("Audio/obj-hold-hand");
            _firstFindTheKey = Resources.Load<AudioClip>("Audio/first-find-key");
            
            _objHoldHand.LoadAudioData();
            _firstFindTheKey.LoadAudioData();
            
            // fetch isUnlocked from data
            // if mission completed unlock else not
        }

        [UsedImplicitly]
        private void Interact()
        {
            StartCoroutine(TryUnlock());
        }

        private IEnumerator TryUnlock()
        {
            if (!_canInteract) yield break;
            var hasKey = InventoryDataHandler.Instance.CheckIfItemIsInInventory("lockKey");
            if (!hasKey)
            {
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("96"), _firstFindTheKey);
                _canInteract = false;
                yield return new WaitForSeconds(3f);
                _canInteract = true;
            }
            else
            {
                var item = InventoryDataHandler.Instance.SearchItem("lockKey");
                if (InventoryDataHandler.Instance.CheckIfItemIsInHand(item))
                {
                    // Update Lock Status
                    InventoryDataHandler.Instance.RemoveItemByName("lockKey");
                    _isUnlocked = true;
                    keyModel.SetActive(true);
                    gameObject.tag = "Untagged";
                    Instantiate(UnlockLockSound, transform.position, Quaternion.identity);
                }
                else
                {
                    SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("7"), _objHoldHand, 0.5f);
                    _canInteract = false;
                    yield return new WaitForSeconds(3f);
                    _canInteract = true;
                }
            }
        }

        public bool IsUnlocked()
        {
            return _isUnlocked;
        }
        
    }
}
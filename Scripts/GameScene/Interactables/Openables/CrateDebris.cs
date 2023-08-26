using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using Interactable.Utility;
using Localization;
using UnityEngine;
using UnityEngine.Playables;

namespace GameScene.Interactables.Openables
{
    public class CrateDebris : Openable
    {
        [SerializeField] private CrashCrateSimple[] crates;
        [SerializeField] private BoxCollider collider1;
        [SerializeField] private PlayableDirector afterPassageJumpscareCutscene;
        
        private AudioClip _objHoldHand;
        private AudioClip _hallTrigger;

        private IEnumerator Start()
        {
            while (!DataManager.Instance.IsLoaded())
            {
                yield return null;
            }
            
            if (AssignmentsDataHandler.Instance.IsCompleted(3))
            {
                Destroy(gameObject);
            }
            else
            {
                foreach (var crate in crates)
                {
                    crate.gameObject.SetActive(true);
                }
            }
            
            _objHoldHand = Resources.Load<AudioClip>("Audio/obj-hold-hand");
            _objHoldHand.LoadAudioData();
            
            _hallTrigger = Resources.Load<AudioClip>("Audio/hall-trigger");
            _hallTrigger.LoadAudioData();
        }

        protected override void Interact()
        {
            StartCoroutine(TryDestroy());
        }
        
        private IEnumerator TryDestroy()
        {
            if (!CanInteract()) yield break;
            
            if (HasItem("axe"))
            {
                if (HasItemInHand("axe"))
                {
                    Destroy(collider1);
                    foreach (var crate in crates)
                    {
                        crate.Crash();
                        Destroy(crate.gameObject, 3.5f);
                    }

                    afterPassageJumpscareCutscene.Play();
                    yield return new WaitForSeconds((float)afterPassageJumpscareCutscene.duration);
                    afterPassageJumpscareCutscene.Stop();
                    AssignmentsDataHandler.Instance.Complete(3);
                }
                else
                {
                    SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("7"), _objHoldHand, 0.5f);
                    UpdateCanInteract();
                }
            }
            else
            {
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("8"), _hallTrigger, 0.5f);
                UpdateCanInteract();
            }
        }


        
    }
}
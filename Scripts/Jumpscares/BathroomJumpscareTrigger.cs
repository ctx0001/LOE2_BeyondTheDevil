using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Player;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.Playables;

namespace Jumpscares
{
    public class BathroomJumpscareTrigger : MonoBehaviour
    {
        [SerializeField] private PlayableDirector cutscene;
        [SerializeField] private GameObject corridorJumpscare;
        [SerializeField] private Visual visual;
        [SerializeField] private Movement movement;
        [SerializeField] private GameObject dollCutscene;
        [SerializeField] private Transform lookAtRef;
        [SerializeField] private Torch torch;

        private bool _firstTime = true;
        private bool _watchingMirror;

        private IEnumerator Start()
        {
            while (!AssignmentsDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }
            
            if (AssignmentsDataHandler.Instance.IsCompleted(15))
            {
                dollCutscene.SetActive(false);
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.tag.Equals("Player") && _firstTime)
            {
                if (!AssignmentsDataHandler.Instance.IsCompleted(15))
                {
                    _firstTime = false;
                    visual.SetRotateStatus(false);
                    movement.SetMoveStatus(false);
                    StartCoroutine(visual.ForceVisualToTransform(lookAtRef));
                    StartCoroutine(JumpscareRoutine());
                }
            }
        }

        private IEnumerator JumpscareRoutine()
        {
            AudioManager.Instance.DisableBathroomSingSound();
            cutscene.Play();
            yield return new WaitForSeconds((float) cutscene.duration / 2);
            MapManager.Instance.DisableLights();

            InventoryDataHandler.Instance.SelectedSlot = 1;
            InventoryGraphicsHandler.Instance.UpdateInventoryCanvas();
            InventoryGraphicsHandler.Instance.ChangeSelection();
            torch.EnableTorch();

            yield return new WaitForSeconds((float) cutscene.duration / 2);
            cutscene.Stop();
            visual.SetRotateStatus(true);
            movement.SetMoveStatus(true);
            AssignmentsDataHandler.Instance.Complete(15);
            corridorJumpscare.SetActive(true);
            
            AudioManager.Instance.EnableScaredSound();
            yield return new WaitForSeconds(10f);
            AudioManager.Instance.DisableScaredSound();
            Destroy(this, 1f);
        }
    }
}
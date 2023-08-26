using GameScene.Data.Handlers;
using UnityEngine;
using UnityEngine.Playables;

namespace GameScene.Interactables.Objects.Torn_Diary
{
    public class DiaryLetters : InteractableObject
    {
        [SerializeField] private PlayableDirector pickedCutscene;
        
        private void Start()
        {
            base.Initialize();
        }

        protected override void Interact()
        {
            base.Interact();
            pickedCutscene.Play();
            ObjectStateDataHandler.Instance.UpdateObjectState(gameObject.name , "inactive");
            Destroy(gameObject);
        }
    }
}
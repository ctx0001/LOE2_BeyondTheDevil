using JetBrains.Annotations;
using Player;
using UnityEngine;

namespace GameScene.Interactables.Readables
{
    public class Visualizable : MonoBehaviour
    {
        [SerializeField] private GameObject view;
        [SerializeField] private GameObject paperSound;
        [SerializeField] private Movement movement;
        [SerializeField] private Visual visual;
        
        [UsedImplicitly]
        protected virtual void Interact()
        {
            if (view.activeSelf)
            {
                view.SetActive(false);
                movement.SetMoveStatus(true);
                visual.SetRotateStatus(true);
                OnClose();
            }
            else
            {
                view.SetActive(true);
                movement.SetMoveStatus(false);
                visual.SetRotateStatus(false);
            }

            Instantiate(paperSound, transform.position, Quaternion.identity);
        }

        protected virtual void OnClose() { /* override in other classes*/ }
    }
}
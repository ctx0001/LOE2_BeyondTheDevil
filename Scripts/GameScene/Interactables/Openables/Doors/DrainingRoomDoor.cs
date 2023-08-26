using GameScene.Data.Handlers;
using GameScene.Interactables.Openables;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;

namespace Assets.Scripts.GameScene.Interactables.Openables.Doors
{
    internal class DrainingRoomDoor : Openable
    {
        [Header("Settings")]
        [SerializeField] internal string id;

        [Header("Debug Settings")]
        [SerializeField] internal bool unlocked;

        [Header("Animation References")]
        [SerializeField] private Animator doorAnimator;
        [Tooltip("The box collider attached to the door component")]
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private NavMeshObstacle navMeshObstacle;

        private IEnumerator Start()
        {
            while (!DoorDataHandler.Instance.IsLoaded())
            {
                yield return null;
            }

            unlocked = DoorDataHandler.Instance.IsDoorUnlocked(id);

            if (unlocked)
            {
                Open();
            }
        }

        public void Open()
        {
            doorAnimator.SetTrigger("Open");
            //Destroy(boxCollider);
            Destroy(navMeshObstacle);
        }

        public void Close()
        {
            doorAnimator.SetTrigger("Close");
        }
    }
}

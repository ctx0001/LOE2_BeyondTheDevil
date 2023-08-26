using GameScene.Data.Handlers;
using GameScene.Interactables.Openables.Doors;
using mini_game.ElectronicMaze;
using Player;
using UnityEngine;

namespace Interactable.Utility
{
    public class MetalDoorControllerExternal : MonoBehaviour
    {
        [SerializeField] private MetalDoor metalDoor;
        [SerializeField] private GameObject metalDoorUnlock;

        [SerializeField] private Movement player;
        [SerializeField] private Visual visual;

        [SerializeField] private ElectronicMaze electronicMaze;

        private void Interact()
        {
            if (AssignmentsDataHandler.Instance.IsCompleted(5))
            {
                Unlock();
            }
            else
            {
                if (electronicMaze.IsRunning()) return;
                player.SetMoveStatus(false);
                visual.SetRotateStatus(false);
                electronicMaze.StartGame();
            }
        }

        public void Unlock()
        {
            player.SetMoveStatus(true);
            visual.SetRotateStatus(true);
            
            metalDoor.unlocked = true;
            DoorDataHandler.Instance.UpdateDoorState(metalDoor.id, metalDoor.unlocked, false);
            
            if (!AssignmentsDataHandler.Instance.IsCompleted(5))
                AssignmentsDataHandler.Instance.Complete(5);
            
            Instantiate(metalDoorUnlock, transform.position, Quaternion.identity);
            metalDoor.Open();
            gameObject.tag = "Untagged";
            Destroy(this);
        }
    }
}
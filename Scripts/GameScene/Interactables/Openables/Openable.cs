using GameScene.Data.Handlers;
using UnityEngine;

namespace GameScene.Interactables.Openables
{
    public class Openable : MonoBehaviour
    {
        private float _canInteract;
        private const int InteractionInterval = 2;

        protected virtual void Interact()
        {
            if (!CanInteract()) return;
            OnInteracted();
            _canInteract = Time.time + InteractionInterval;
        }

        protected virtual void OnInteracted() { }

        protected bool CanInteract()
        {
            return Time.time > _canInteract;
        }

        protected void UpdateCanInteract()
        {
            _canInteract = Time.time + InteractionInterval;
        }
        
        protected int GetInteractionInterval()
        {
            return InteractionInterval;
        }

        protected static bool HasItem(string itemName)
        {
            var hasItem = InventoryDataHandler.Instance.CheckIfItemIsInInventory(itemName);
            return hasItem;
        }

        protected static bool HasItemInHand(string itemName)
        {
            return InventoryDataHandler.Instance.CheckIfItemIsInHand(InventoryDataHandler.Instance.SearchItem(itemName));
        }

        protected static void RemoveItem(string itemName)
        {
            InventoryDataHandler.Instance.RemoveItemByName(itemName);
        }
    }
}
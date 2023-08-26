using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using UnityEngine;

namespace GameScene.Interactables
{
    public class InteractableObject : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private ItemProprety item;
        [SerializeField] private GameObject pickUpSound;

        private Item _item;

        protected virtual void Initialize()
        {
             _item = new Item(item.GetId(), item.GetName(), item.GetQuantity(), item.GetSpritePath(), item.GetGameObjectName());
        }

        protected virtual void Interact()
        {
            if (!InventoryDataHandler.Instance.HasSpace()) return;
            Instantiate(pickUpSound, transform.position, Quaternion.identity);
            InventoryDataHandler.Instance.AddNewItem(_item);
        }

        protected virtual string GetItemName()
        {
            return item.GetName();
        }

        protected virtual string GetGameObjectName()
        {
            return item.GetGameObjectName();
        }
    }
}
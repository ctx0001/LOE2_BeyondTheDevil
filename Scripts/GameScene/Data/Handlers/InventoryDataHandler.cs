using System.Collections.Generic;
using System.Linq;
using GameScene.Data.Handlers.Dependencies;
using GameScene.Player;
using UnityEngine;

namespace GameScene.Data.Handlers
{
    public class InventoryDataHandler : DataHandler
    {
        public List<Item> Items = new List<Item>(7);
        public static InventoryDataHandler Instance;
        internal int SelectedSlot = 1;
        private Item _inHandItem;
        
        private void Awake() 
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this;
            }
        }

        private void Start()
        {
            SetPath("ItemsData.json");

            Items = FetchData<List<Item>>(GetPath());
            MarkLoaded();
            InventoryGraphicsHandler.Instance.UpdateInventoryCanvas();
        }
        
        internal Item SearchItem(string itemName)
        {
            foreach (var item in Items)
            {
                if (item.CompareName(itemName))
                {
                    return item;
                }
            }

            return null; 
        }
        
        internal bool CheckIfItemIsInInventory(string itemName)
        {
            return Items.Any(item => item.CompareName(itemName));
        }

        private Item GetItemByName(string itemName)
        {
            return Items.FirstOrDefault(item => item.CompareName(itemName));
        }
        
        internal void RemoveItemByName(string itemName, int quantity=1)
        {
            var item = GetItemByName(itemName);
            item.quantity -= quantity;
            if(item.quantity == 0)
                Items.Remove(item);
            
            InventoryGraphicsHandler.Instance.RemoveGraphicImage();
            InventoryGraphicsHandler.Instance.UpdateInventoryCanvas();
            UpdateData(GetPath(), Items);
        }
        
        internal bool HasSpace()
        {
            return Items.Count < 7;
        }

        private readonly Dictionary<KeyCode, int> _keyToSlot = new Dictionary<KeyCode, int>
        {
            {KeyCode.Alpha1, 1},
            {KeyCode.Alpha2, 2},
            {KeyCode.Alpha3, 3},
            {KeyCode.Alpha4, 4},
            {KeyCode.Alpha5, 5},
            {KeyCode.Alpha6, 6},
            {KeyCode.Alpha7, 7}
        };

        private void Update()
        {
            foreach (var kvp in _keyToSlot)
            {
                if (Input.GetKeyDown(kvp.Key))
                {
                    SelectedSlot = kvp.Value;
                    ChangeSelectedItem(SelectedSlot);
                    InventoryGraphicsHandler.Instance.ChangeSelection();
                    break; // exit from loop after first key pressed
                }
            }
        }

        private void ChangeSelectedItem(int index)
        {
            _inHandItem = Items.ElementAtOrDefault(index - 1);
            if (_inHandItem == null || !(_inHandItem.CompareName("torch")))
                Torch.Instance.DisableTorch();
        }

        public bool CheckIfItemIsInHand(Item item)
        {
            ChangeSelectedItem(SelectedSlot);
            if (item != null && _inHandItem != null)
            {
                return item.CompareName(_inHandItem.name);
            }
            
            return false;
            
        }
        
        public bool CheckIfItemIsInHand(string itemName)
        {
            ChangeSelectedItem(SelectedSlot);
            return _inHandItem != null && itemName.Equals(_inHandItem.name);
        }
        
        public void AddNewItem(Item item)
        {
            if (!HasSpace()) return;
            
            // Updates the object to be inactive for every new scene load
            ObjectStateDataHandler.Instance.UpdateObjectState(item.gameObjectName, "inactive");
            // Checks if didnt have the objects in inventory, if yes add one to quantity
            if (!CheckIfItemIsAlreadyInInventory(item))
            {
                Items.Add(item);
            }
            UpdateData(GetPath(), Items);
            InventoryGraphicsHandler.Instance.UpdateInventoryCanvas();
        }

        private bool CheckIfItemIsAlreadyInInventory(Item itemToCheck)
        {
            foreach (var item in Items)
            {
                if (!item.CompareName(itemToCheck.name)) continue;
                item.quantity += 1;
                return true;
            }
            return false;
        }
    }
}
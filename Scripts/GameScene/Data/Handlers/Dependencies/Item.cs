using UnityEngine;

namespace GameScene.Data.Handlers.Dependencies
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sprite { get; set; }
        public string gameObjectName { get; set; }
        public int quantity { get; set; }

        public Item(int id, string name, int quantity, string imagePath, string gameObjectName)
        {
            this.id = id;
            this.name = name;
            this.quantity = quantity;
            sprite = imagePath;
            this.gameObjectName = gameObjectName;
        }

        public Sprite GetImage()
        {
            var texture = Resources.Load<Sprite>(sprite);
            return texture;
        }

        /*public void Use(int amount)
        {
            quantity -= amount;
        }*/

        public bool CompareId(int itemId)
        {
            return id == itemId;
        }
        
        public bool CompareName(string itemName)
        {
            return name == itemName;
        }
    }
}
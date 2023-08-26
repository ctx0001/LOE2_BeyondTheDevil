using UnityEngine;

namespace GameScene.Data.Handlers.Dependencies
{
    public class ItemProprety : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private string name;
        [SerializeField] private int quantity;
        [SerializeField] private string spritePath;
        [SerializeField] private string gameObjectName;

        internal int GetId() => id;
        internal string GetName() => name;
        internal int GetQuantity() => quantity;
        internal string GetSpritePath() => spritePath;
        internal string GetGameObjectName() => gameObjectName;


    }
}
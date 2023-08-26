using GameScene.Data.Handlers;
using Managers;
using UnityEngine;

namespace GameScene.Interactables
{
    public class StealableItem : MonoBehaviour
    {
        [SerializeField] private int dollarValue;
        [SerializeField] private GameObject collectPrefabSound;

        private void Interact()
        {
            Instantiate(collectPrefabSound, transform.position, Quaternion.identity);
            BalanceManager.Instance.AddBalance(dollarValue);
            ObjectStateDataHandler.Instance.UpdateObjectState(gameObject.name, "inactive");
            Destroy(gameObject);
        }
    }
}
using System.Collections.Generic;
using GameScene.Data.Handlers;
using UnityEngine;

namespace GameScene.Interactables.Objects.Torn_Diary
{
    public class TornDiaryManager : MonoBehaviour
    {
        [SerializeField] private GameObject view;
        [SerializeField] private List<GameObject> pages;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if(!view.activeSelf)
                    Show();
                else
                    view.SetActive(false);
            }
        }

        private void Show()
        {
            if (!InventoryDataHandler.Instance.CheckIfItemIsInInventory("DiaryLetter")) return;
            var pagesAmount = InventoryDataHandler.Instance.SearchItem("DiaryLetter").quantity;

            var counter = 1;
            foreach (var page in pages)
            {
                if (!(counter > pagesAmount))
                {
                    page.SetActive(true);
                    counter++;
                }
                else
                {
                    Debug.Log($"{counter} > {pagesAmount}");
                }
            }
            
            view.SetActive(true);
        }
    }
}
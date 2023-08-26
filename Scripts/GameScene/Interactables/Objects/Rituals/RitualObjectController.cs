using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using GameScene.Data.Handlers.Dependencies;
using Localization;
using TMPro;
using UnityEngine;

namespace GameScene.Interactables.Objects.Rituals
{
    public class RitualObjectController : MonoBehaviour
    {
        [SerializeField] private MultipleDialogue multipleDialogue1;
        [SerializeField] private GameObject ritualSearchingHintCanvas;
        [SerializeField] private TextMeshProUGUI candlesProgress;
        [SerializeField] private TextMeshProUGUI incenseProgress;
        [SerializeField] private GameObject incenseCheckmark;
        [SerializeField] private GameObject candleCheckmark;
        [SerializeField] private GameObject moreSpecificHints;

        public static bool AllTaken()
        {
            if (InventoryDataHandler.Instance.CheckIfItemIsInInventory("Candle") &&
                InventoryDataHandler.Instance.CheckIfItemIsInInventory("Incense"))
            {
                var incenseReached = InventoryDataHandler.Instance.SearchItem("Incense").quantity == 3;
                var candleReached = InventoryDataHandler.Instance.SearchItem("Candle").quantity == 6;
                if (incenseReached && candleReached) return true;
            }
            
            return false;
        }
        
        public void OnComplete()
        {
            ritualSearchingHintCanvas.SetActive(false);
            var assignment = new Assignment(19, LocalizationManager.Instance.GetContent("182"), null, true, false);
            AssignmentsDataHandler.Instance.Create(assignment, 18);
            multipleDialogue1.Play();
        }

        private IEnumerator Start()
        {
            while (!AssignmentsDataHandler.Instance.IsLoaded())
                yield return null;

            if (AssignmentsDataHandler.Instance.Exists(18) && !AssignmentsDataHandler.Instance.IsCompleted(18))
            {
                ritualSearchingHintCanvas.SetActive(true);
                UpdateHintCanvas();
            }
        }

        public void UpdateHintCanvas()
        {
            if (InventoryDataHandler.Instance.CheckIfItemIsInInventory("Incense"))
            {
                var incense = InventoryDataHandler.Instance.SearchItem("Incense");
                incenseProgress.text = $"{incense.quantity}/3";

                if (incense.quantity == 3)
                {
                    incenseCheckmark.SetActive(true);
                }
            }
            
            if (InventoryDataHandler.Instance.CheckIfItemIsInInventory("Candle"))
            {
                var candle= InventoryDataHandler.Instance.SearchItem("Candle");
                candlesProgress.text = $"{candle.quantity}/6";

                if (candle.quantity == 6)
                {
                   candleCheckmark.SetActive(true); 
                }
            }
        }

        private IEnumerator ShowMoreSpecificHints()
        {
            yield return new WaitForSeconds(45f);
            moreSpecificHints.SetActive(true);
        }
        
        public void StartSearching()
        {
            ritualSearchingHintCanvas.SetActive(true);
            StartCoroutine(ShowMoreSpecificHints());
        }
    }
}
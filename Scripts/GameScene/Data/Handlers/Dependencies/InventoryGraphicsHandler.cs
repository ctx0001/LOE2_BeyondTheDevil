using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Data.Handlers.Dependencies
{
    public class InventoryGraphicsHandler : MonoBehaviour
    {
        [Header("Images")]
        [SerializeField] private Image slot1Image;
        [SerializeField] private Image slot2Image;
        [SerializeField] private Image slot3Image;
        [SerializeField] private Image slot4Image;
        [SerializeField] private Image slot5Image;
        [SerializeField] private Image slot6Image;
        [SerializeField] private Image slot7Image;
        
        [Header("Borders")]
        [Tooltip("The borders of the inventory slot")]
        [SerializeField] private Image slot1Border;
        [SerializeField] private Image slot2Border;
        [SerializeField] private Image slot3Border;
        [SerializeField] private Image slot4Border;
        [SerializeField] private Image slot5Border;
        [SerializeField] private Image slot6Border;
        [SerializeField] private Image slot7Border;

        [Header("Quantity Text")]
        [Tooltip("The text representing the quantity of an item in a slot")]
        [SerializeField] private TextMeshProUGUI slot1Quantity;
        [SerializeField] private TextMeshProUGUI slot2Quantity;
        [SerializeField] private TextMeshProUGUI slot3Quantity;
        [SerializeField] private TextMeshProUGUI slot4Quantity;
        [SerializeField] private TextMeshProUGUI slot5Quantity;
        [SerializeField] private TextMeshProUGUI slot6Quantity;
        [SerializeField] private TextMeshProUGUI slot7Quantity;
        
        [Header("Settings")]
        [SerializeField] private Color32 borderDefaultColor = new Color32(25, 25, 25, 255);
        [SerializeField] private Color32 borderHoverColor = new Color32(204, 204, 204, 255);
        
        private List<Image> _images = new List<Image>(7);
        public static InventoryGraphicsHandler Instance;
        
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
            _images.Add(slot1Image);
            _images.Add(slot2Image);
            _images.Add(slot3Image);
            _images.Add(slot4Image);
            _images.Add(slot5Image);
            _images.Add(slot6Image);
            _images.Add(slot7Image);
            
            UpdateInventoryCanvas();
            ChangeSelection();
        }

        public void ChangeSelection()
        {
            ResetInventorySelection();
            
            switch (InventoryDataHandler.Instance.SelectedSlot)
            {
                case 1:
                    slot1Border.color = borderHoverColor;
                    break;
                case 2:
                    slot2Border.color = borderHoverColor;
                    break;
                case 3:
                    slot3Border.color = borderHoverColor;
                    break;
                case 4:
                    slot4Border.color = borderHoverColor;
                    break;
                case 5:
                    slot5Border.color = borderHoverColor;
                    break;
                case 6:
                    slot6Border.color = borderHoverColor;
                    break;
                case 7:
                    slot7Border.color = borderHoverColor;
                    break;
            }
            
        }

        private void ResetInventorySelection()
        {
            slot1Border.color = borderDefaultColor;
            slot2Border.color = borderDefaultColor;
            slot3Border.color = borderDefaultColor;
            slot4Border.color = borderDefaultColor;
            slot5Border.color = borderDefaultColor;
            slot6Border.color = borderDefaultColor;
            slot7Border.color = borderDefaultColor;
        }
        
        internal void UpdateInventoryCanvas()
        {
            foreach (var item in InventoryDataHandler.Instance.Items)
            {
                Debug.Log("Active Item Name: " + item.name);
                var position = InventoryDataHandler.Instance.Items.IndexOf(item);
                switch (position)
                {
                    case 0:
                        slot1Image.sprite = item.GetImage();
                        slot1Quantity.gameObject.SetActive(true);
                        slot1Quantity.text = item.quantity.ToString();
                        break;
                    case 1:
                        slot2Image.sprite = item.GetImage();
                        slot2Quantity.gameObject.SetActive(true);
                        slot2Quantity.text = item.quantity.ToString();
                        break;
                    case 2:
                        slot3Image.sprite = item.GetImage();
                        slot3Quantity.gameObject.SetActive(true);
                        slot3Quantity.text = item.quantity.ToString();
                        break;
                    case 3:
                        slot4Image.sprite = item.GetImage();
                        slot4Quantity.gameObject.SetActive(true);
                        slot4Quantity.text = item.quantity.ToString();
                        break;
                    case 4:
                        slot5Image.sprite = item.GetImage();
                        slot5Quantity.gameObject.SetActive(true);
                        slot5Quantity.text = item.quantity.ToString();
                        break;
                    case 5:
                        slot6Image.sprite = item.GetImage();
                        slot6Quantity.gameObject.SetActive(true);
                        slot6Quantity.text = item.quantity.ToString();
                        break;
                    case 6:
                        slot7Image.sprite = item.GetImage();
                        slot7Quantity.gameObject.SetActive(true);
                        slot7Quantity.text = item.quantity.ToString();
                        break;
                }
            }
            ResetImages();
        }
        
        private void ResetImages()
        {
            foreach (var image in _images)
                image.enabled = image.sprite != null;
        }
        
        internal void RemoveGraphicImage()
        {
            var activeItems = InventoryDataHandler.Instance.Items.Select(item => InventoryDataHandler.Instance.Items.IndexOf(item)).ToList();

            for (var i = 0; i < 7; i++)
            {
                if (!activeItems.Contains(i))
                {
                    switch (i)
                    {
                        case 0:
                            slot1Image.sprite = null;
                            slot1Quantity.gameObject.SetActive(false);
                            slot1Quantity.text = "";
                            break;
                        case 1:
                            slot2Image.sprite = null;
                            slot2Quantity.gameObject.SetActive(false);
                            slot2Quantity.text = "";
                            break;
                        case 2:
                            slot3Image.sprite = null;
                            slot3Quantity.gameObject.SetActive(false);
                            slot3Quantity.text = "";
                            break;
                        case 3:
                            slot4Image.sprite = null;
                            slot4Quantity.gameObject.SetActive(false);
                            slot4Quantity.text = "";
                            break;
                        case 4:
                            slot5Image.sprite = null;
                            slot5Quantity.gameObject.SetActive(false);
                            slot5Quantity.text = "";
                            break;
                        case 5:
                            slot6Image.sprite = null;
                            slot6Quantity.gameObject.SetActive(false);
                            slot6Quantity.text = "";
                            break;
                        case 6:
                            slot7Image.sprite = null;
                            slot7Quantity.gameObject.SetActive(false);
                            slot7Quantity.text = "";
                            break;
                    }
                }
            }
        }
        
    }
}
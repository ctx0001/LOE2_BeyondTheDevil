using System.Collections;
using Localization;
using Managers;
using UnityEngine;

namespace GameScene.Player
{
    public class Interaction : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask layerMask;

        private string _isADoorText = "";
        private string _isAKeyText = "";
        private string _isAReadableText = "";
        private string _isATelephoneText = "";
        private string _isGramophoneText = "";
        private string _isMoneyText = "";
        private string _isSafeText = "";
        private string _isAGenericText = "";
        private string _isACrateText = "";
        private string _isADisabledText = "";
        private string _isAUnlockText = "";
        
        private const float Range = 3f;
        private bool _canInteract = true;
        
        private void Start()
        {
            StartCoroutine(WaitForDataLoad());
        }

        public void AllowInteraction(bool canInteract)
        {
            _canInteract = canInteract;
        }
        
        private IEnumerator WaitForDataLoad()
        {
            while (!LocalizationManager.Instance.IsLoaded())
            {
                yield return null;
            }
            
            _isAGenericText = LocalizationManager.Instance.GetContent("19");
            _isADoorText = LocalizationManager.Instance.GetContent("20");
            _isAKeyText = LocalizationManager.Instance.GetContent("21");
            _isACrateText = LocalizationManager.Instance.GetContent("131");
            _isADisabledText = LocalizationManager.Instance.GetContent("132");
            
            _isGramophoneText = LocalizationManager.Instance.GetContent("53");
            _isMoneyText = LocalizationManager.Instance.GetContent("46");
            _isSafeText = LocalizationManager.Instance.GetContent("44");
            _isATelephoneText = LocalizationManager.Instance.GetContent("45");
            _isAReadableText = LocalizationManager.Instance.GetContent("43");
            _isAUnlockText = LocalizationManager.Instance.GetContent("167");
        }

        private void Update()
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out var hit, Range, layerMask))
            {
                if (!hit.transform.CompareTag("Interactable"))
                {
                    CanvasManager.Instance.DisableInteractionText();
                }
                else
                {
                    FetchName(hit.transform.name);
                    if(Input.GetKeyDown(KeyCode.E) && _canInteract)
                        hit.transform.SendMessage("Interact");
                }
            }
            else
            {
                CanvasManager.Instance.DisableInteractionText();
            }
        }

        /*private void AddOutline(GameObject target)
        {
            if (target == null) return;
            var outlineComponent = target.GetComponent<Outline>();

            if (outlineComponent != null)
            {
                return;
            }

            var outline = target.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 1f;
        }*/

        /*private void RemoveOutline(GameObject target)
        {
            if (target == null) return;
            var outlineComponent = target.GetComponent<Outline>();

            if (outlineComponent != null)
            {
                Destroy(outlineComponent);
            }
        }*/

        private void FetchName(string name1)
        {
            if (_canInteract)
            {
                if(name1.Contains("Door") || name1.Contains("Drawer"))
                {
                    CanvasManager.Instance.EnableInteractionText(_isADoorText);
                }
                else if(name1.Contains("Key"))
                {
                    CanvasManager.Instance.EnableInteractionText(_isAKeyText);
                }
                else if(name1.Contains("Letter") || name1.Contains("Book") || name.Contains("Page"))
                {
                    CanvasManager.Instance.EnableInteractionText(_isAReadableText);
                }
                else if (name1.Contains("Telephone"))
                {
                    CanvasManager.Instance.EnableInteractionText(_isATelephoneText);
                }
                else if (name1.Contains("gramophone"))
                {
                    CanvasManager.Instance.EnableInteractionText(_isGramophoneText);
                }
                else if (name1.Contains("money") || name1.Contains("Candle") || name1.Contains("Incense") || name1.Contains("AncientCube") || name1.Contains("axe") || name1.Equals("Screwdriver"))
                {
                    CanvasManager.Instance.EnableInteractionText(_isMoneyText);
                }else if(name1.Contains("Lock")){
                    CanvasManager.Instance.EnableInteractionText(_isAUnlockText);
                }
                else if(name1.Contains("Safe"))
                {
                    CanvasManager.Instance.EnableInteractionText(_isSafeText);
                }else if (name1.Equals("CrashCrateDebris"))
                {
                    CanvasManager.Instance.EnableInteractionText(_isACrateText);
                }
                else
                {
                    CanvasManager.Instance.EnableInteractionText(_isAGenericText);
                }   
            }
            else
            {
                CanvasManager.Instance.EnableInteractionText(_isADisabledText);
            }
        }
    }
}
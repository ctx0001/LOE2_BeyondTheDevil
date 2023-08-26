using System.Collections.Generic;
using JetBrains.Annotations;
using Player;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Interactables.Readables
{
    public class MultipleVisualizable : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject view;
        [SerializeField] private List<GameObject> pages;
        [SerializeField] private TextMeshProUGUI currentPageText;
        
        [Header("Audio")]
        [SerializeField] private GameObject paperSound;
        [SerializeField] private GameObject changePageSound;
        
        [Header("Player Patch")]
        [SerializeField] private Movement movement;
        [SerializeField] private Visual visual;

        [Header("Action Buttons")] 
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;

        private int _currentPageIndex;

        public void PreviousPage()
        {
            if (_currentPageIndex > 0)
            {
                ResetPage();
                _currentPageIndex--;
                pages[_currentPageIndex].gameObject.SetActive(true);
                CheckDisableButtons();
                currentPageText.text = $"{_currentPageIndex+1}/{pages.Count}";
                Instantiate(changePageSound, transform.position, Quaternion.identity);
            }
        }

        public void NextPage()
        {
            if (_currentPageIndex < pages.Count - 1)
            {
                ResetPage();
                _currentPageIndex++;
                pages[_currentPageIndex].gameObject.SetActive(true);
                CheckDisableButtons();
                currentPageText.text = $"{_currentPageIndex+1}/{pages.Count}";
                Instantiate(changePageSound, transform.position, Quaternion.identity);
            }
        }

        private void CheckDisableButtons()
        {
            nextButton.interactable = _currentPageIndex < pages.Count - 1;
            previousButton.interactable = _currentPageIndex > 0;
        }

        public void ResetPage()
        {
            foreach (var page in pages)
            {
                page.gameObject.SetActive(false);
            }
        }

        [UsedImplicitly]
        protected virtual void Interact()
        {
            if (view.activeSelf)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                view.SetActive(false);
                movement.SetMoveStatus(true);
                visual.SetRotateStatus(true);
                OnClose();
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                currentPageText.text = $"{_currentPageIndex+1}/{pages.Count}";
                view.SetActive(true);
                movement.SetMoveStatus(false);
                visual.SetRotateStatus(false);
            }

            Instantiate(paperSound, transform.position, Quaternion.identity);
        }

        protected virtual void OnClose() { /* override in other classes*/ }
    }
}
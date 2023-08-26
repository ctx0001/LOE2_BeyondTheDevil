using System;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Pc
{
    public class ComputerFile : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fileNameText;

        [SerializeField] private string fileName;
        [SerializeField] private string contentId;
        private DesktopHandler _desktopHandler;

        private void Start()
        {
            _desktopHandler = GameObject.Find("Desktop").GetComponent<DesktopHandler>();
            Initialize();
        }

        public void SetName(string newName)
        {
            fileName = newName;
        }

        public void SetContent(string newContent)
        {
            contentId = newContent;
        }

        public void Initialize()
        {
            fileNameText.text = fileName;
        }

        public void ViewContent()
        {
            var content = LocalizationManager.Instance.GetContent(contentId);
            _desktopHandler.ViewFileContent(fileName, content);
        }
    }
}
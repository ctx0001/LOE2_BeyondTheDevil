using Localization;
using TMPro;
using UnityEngine;

namespace GameScene.Pc
{
    public class ComputerSheet : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fileNameText;

        [SerializeField] private string fileName;
        [SerializeField] private string csvContentId;
        private DesktopHandler _desktopHandler;

        private void Start()
        {
            _desktopHandler = GameObject.Find("Desktop").GetComponent<DesktopHandler>();
            Initialize();
        }

        public void SetName(string newName)
        {
            fileName = newName + ".csv";
        }

        public void SetContent(string newContentId)
        {
            csvContentId = newContentId;
        }

        public void Initialize()
        {
            fileNameText.text = fileName;
        }

        public void ViewContent()
        {
            var csvContent = LocalizationManager.Instance.GetContent(csvContentId);
            _desktopHandler.ViewSheetPreview(fileName, csvContent);
        }
    }
}